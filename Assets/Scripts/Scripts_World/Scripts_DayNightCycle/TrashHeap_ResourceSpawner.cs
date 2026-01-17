using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TrashHeap_ResourceSpawner : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _resources; // will be prefabs
    [SerializeField] protected float _spawnDelaySeconds = 2f;
    [SerializeField] protected float _upwardForce = 2f;

    [SerializeField] public Health _health;
    [SerializeField] bool ForTesting = false;


    private async void OnTriggerEnter(Collider other)
    {
        // Example condition: only trigger for objects tagged "Player"
        if (other.GetComponent<PlayerController>() && ForTesting)
        {
            Debug.Log("Player Near Heap " + gameObject.name);
            await SpawnResourceWithDelay();

            DisableThisHeap();
        }
    }

    private void DisableThisHeap()
    {
        this.gameObject.SetActive(false);
    }

    private async Task SpawnResourceWithDelay()
    {
        // Convert seconds to milliseconds for Task.Delay
        int delayMs = (int)(_spawnDelaySeconds * 1000);

        await Task.Delay(delayMs);

        if (this == null) return;

        SpawnResource();
    }

    private void SpawnResource()
    {
        if (_resources == null || _resources.Count == 0) return;

        // 1. Pick a random prefab
        GameObject prefab = _resources[Random.Range(0, _resources.Count)];

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
