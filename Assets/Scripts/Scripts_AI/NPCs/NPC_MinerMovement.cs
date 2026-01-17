using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
public class NPC_MinerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject _inventoryContainer; // The Empty GameObject child
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected BoxCollider _miningZone;
    [SerializeField] protected BoxCollider _homeZone;

    [Header("Settings")]
    [SerializeField] protected int _objectLimit = 10;
    [SerializeField] protected int _ejectDelayMs = 150;

    // We store the objects here to keep track of what to eject later
    [SerializeField] protected List<GameObject> _collectedObjects;
    [SerializeField] protected GameObject _currentTargetItem;

    public enum NPCMiningStates { roam, mining, home };
    [SerializeField] private NPCMiningStates _currentState = NPCMiningStates.roam;
    [SerializeField] int _miningDelay = 1000;

    private void Start()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            _agent.Warp(hit.position);
        }

        // Call this ONCE. The internal 'while' loop handles the rest.
        NPCActionPlan();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_collectedObjects.Count < _objectLimit)
        {
            if (other.GetComponent<GarbageObject>() && !_collectedObjects.Contains(other.GetComponent<GarbageObject>().gameObject) && (_currentState != NPCMiningStates.home))
            {
                GameObject targetGarbageObj = other.GetComponent<GarbageObject>().gameObject;

                _collectedObjects.Add(targetGarbageObj);

                // Parent and snap
                targetGarbageObj.transform.SetParent(_inventoryContainer.transform);
                targetGarbageObj.transform.position = Vector3.zero;

                // Turn the object off entirely
                targetGarbageObj.SetActive(false);
            }

            if (_currentState == NPCMiningStates.roam)
            {
                if (other.GetComponent<TrashHeap_ResourceSpawner>())
                {
                    _currentTargetItem = other.gameObject;
                    _currentState = NPCMiningStates.mining;
                }
            }
        }
    }

    private Vector3 CheckAllActiveHeaps()
    {
        // If no zone assigned, stay put
        if (_miningZone == null) return transform.position;

        // Scan for all colliders inside the mining box zone
        Collider[] hitColliders = Physics.OverlapBox(
            _miningZone.bounds.center,
            _miningZone.bounds.extents,
            _miningZone.transform.rotation
        );

        TrashHeap_ResourceSpawner closestHeap = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent(out TrashHeap_ResourceSpawner heap))
            {
                // Optional: check if the heap still has resources here
                float dist = Vector3.Distance(transform.position, heap.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestHeap = heap;
                }
            }
        }

        if (closestHeap != null)
        {
            _currentTargetItem = closestHeap.gameObject;
            _currentState = NPCMiningStates.mining;
            return closestHeap.transform.position;
        }

        // Default to center if no heap found
        return _miningZone.bounds.center;
    }

    private async void NPCActionPlan()
    {
        // Adding a while loop here so it runs continuously without Update()
        while (Application.isPlaying)
        {
            switch (_currentState)
            {
                case NPCMiningStates.roam:
                    // Scan the zone for heaps
                    Vector3 roamTarget = CheckAllActiveHeaps();
                    if (_agent.isOnNavMesh) _agent.SetDestination(roamTarget);
                    break;

                case NPCMiningStates.mining:
                    if (_currentTargetItem != null && _currentTargetItem.activeSelf)
                    {
                        if (_agent.isOnNavMesh) _agent.SetDestination(_currentTargetItem.transform.position);

                        while (_agent.pathPending || _agent.remainingDistance > 1.2f) await Task.Yield();

                        await Task.Delay(_miningDelay);

                        if (_currentTargetItem != null)
                        {
                            var spawner = _currentTargetItem.GetComponent<TrashHeap_ResourceSpawner>();
                            if (spawner != null) spawner.SpawnResource();
                        }

                        _currentTargetItem = null;

                        if (_collectedObjects.Count >= _objectLimit)
                            _currentState = NPCMiningStates.home;
                        else
                            _currentState = NPCMiningStates.roam;
                    }
                    else
                    {
                        _currentState = NPCMiningStates.roam;
                    }
                    break;

                case NPCMiningStates.home:
                    if (_homeZone != null)
                    {
                        if (_agent.isOnNavMesh) _agent.SetDestination(_homeZone.bounds.center);

                        while (_agent.pathPending || _agent.remainingDistance > 1.0f) await Task.Yield();

                        await EjectItemsWithDelay();
                        _currentState = NPCMiningStates.roam;
                    }
                    break;
            }
            await Task.Yield();
        }
    }

    private async Task EjectItemsWithDelay()
    {

        // We count backwards (i--) to safely remove items from a List while iterating
        for (int i = _collectedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = _collectedObjects[i];

            _collectedObjects[i].transform.position = _inventoryContainer.transform.position;
            _collectedObjects[i].SetActive(true);

            if (obj != null)
            {
                // 1. Unparent the object so it stays at the Home Base
                obj.transform.SetParent(null);

                // 2. Re-enable physics so it falls/reacts to the world
                if (obj.GetComponent<Rigidbody>())
                {
                    obj.GetComponent<Rigidbody>().AddForce(_inventoryContainer.transform.forward * 10f, ForceMode.Impulse);
                }
            }

            // 3. Remove from our tracking list
            _collectedObjects.RemoveAt(i);

            // 4. The Delay: This prevents the 'lag' you were worried about
            await Task.Delay(_ejectDelayMs);
        }
        // So would completly clear
        await Task.Delay(2000);
    }
}
// Here are the States, and we can split these to different Functions

// Roam (Not near by a TrashHeap_ResourceSpawner)

// Go to nearest TrashHeap_ResourceSpawner (using Bounding Area Mining Box bounds)



// Mining (Found A TrashHeap_ResourceSpawner)

// Need to find or go to Bounding Area Mining and check if any TrashHeap_ResourceSpawner near by

// then would go near it wait for 2 seconds then collects Object (This should be in Await Delay no Cancel Tokens)

// After getting one need to rotate to another TrashHeap_ResourceSpawner

// repeat until gain equal to object limit



// Home (fulfilled limit)

// Go back home go to Bounding Area Home

// Trigger enter check if TowerBigBase, sends _collectionObjects and moves all to it

// GameObject must be ejected out of this NPC

// So needs to hold the GameObject in the NPC