using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrashHeap_ResourceSpawner : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _resources; // will be prefabs
    [SerializeField] protected float _spawnDelaySeconds = 2f;
    [SerializeField] protected float _upwardForce = 2f;

    [SerializeField] protected int _howManyToSpawn = 1; //= Random.Range(1, 5);

    [SerializeField] public GarbageObject.GarbageGroup _garbageGroupType;
    [SerializeField] public bool _randomized = false;

    [SerializeField] public Health _health;
    [SerializeField] private Slider _healthSlider;

    [SerializeField] bool ForTesting = false;
    [SerializeField] bool hasSpawned = true;

    private void Start()
    {
        _healthSlider.maxValue = _health.GetMaxHealth();
    }

    private async void OnTriggerEnter(Collider other)
    {
        // Example condition: only trigger for objects tagged "Player"
        // || (other.GetComponent<NPC_MinerMovement>()) if going to disable these things
        if ( (other.GetComponent<PlayerController>() ))
        {
            SetRandomized();

            _howManyToSpawn = Random.Range(1, 5);

            if (ForTesting)
            {
                Debug.Log("Player Near Heap " + gameObject.name);
                await SpawnResourceWithDelay();

                DisableThisHeap();
            }
        }
    }

    private void SetRandomized()
    {
        int testRandom = Random.Range(1, 2);

        if (testRandom == 1)
        {
            _randomized = true;
        }
        else if (testRandom == 2)
        {
            _randomized = false;
        }
    }

    private async void Update()
    {
        _healthSlider.value = _health.GetCurrentHealth();

        if (_health.GetCurrentHealth() <= 0)
        {
            if (hasSpawned)
            {
                for (int i =0; i<_howManyToSpawn; i++)
                {
                    await SpawnResourceWithDelay();
                }

                DisableThisHeap();
            }
        }
    }


    private void DisableThisHeap()
    {
        hasSpawned = false;
        this.gameObject.SetActive(false);
    }

    public void EnableThisHeap()
    {
        this.gameObject.SetActive(true);
        _health.Heal(_health.startSetHealth);
    }

    private async Task SpawnResourceWithDelay()
    {
        // Convert seconds to milliseconds for Task.Delay
        int delayMs = (int)(_spawnDelaySeconds * 1000);

        await Task.Delay(delayMs);

        if (this == null) return;

        SetResourceType();
    }

    public void SetResourceType()
    {
        if (!_randomized)
        {
            if (_garbageGroupType == GarbageObject.GarbageGroup.Plastic)
            {
                SpawnResource(0);
            }
            else if (_garbageGroupType == GarbageObject.GarbageGroup.Organic)
            {
                SpawnResource(1);
            }
            else if (_garbageGroupType == GarbageObject.GarbageGroup.Metal)
            {
                SpawnResource(2);
            }
        }

        else if (_randomized)
        {
            int randomType = Random.Range(0, 100);
            if (randomType <= 20 && randomType !> 20) // Plastic
            {
                SpawnResource(0);
            }
            else if (randomType <= 40 && randomType !> 40) // Wood
            {
                SpawnResource(1);
            }
            else if (randomType <= 60 && randomType !> 60) // Metal
            {
                SpawnResource(2);
            }
        }
    }


    public void SpawnResource(int targetType)
    {
        if (_resources == null || _resources.Count == 0) return;

        GameObject prefab = null;

        // 1. Pick a resource
        if (!_randomized)
        {
            prefab = _resources[targetType];
        }
        else if (_randomized)
        {
            prefab = _resources[Random.Range(0, _resources.Count)];
        }
        // 2. Spawn the object
        GameObject spawnedObj = Instantiate(prefab, transform.position, Quaternion.identity);

        // 3. Apply force if it has a Rigidbody
        if (spawnedObj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // ForceMode.Impulse is best for instant "burst" movements
            rb.AddForce(Vector3.up * _upwardForce, ForceMode.Impulse);
        }
    }
}
