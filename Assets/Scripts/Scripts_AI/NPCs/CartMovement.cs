using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CartMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private int waypointIndex = 0;

    [Header("Cart")]
    [SerializeField] private GameObject cartAsset;

    [Header("Mineral Holder")]
    [SerializeField] protected List<GameObject> _resources;
    [SerializeField] protected GameObject _inventoryContainer; // The Empty GameObject child
    [SerializeField] private int _holderLimit = 10;
    [SerializeField] public bool _isFull = false;

    [SerializeField] protected Transform resourceSpawnPoint;
    [SerializeField] protected float _spawnDelaySeconds = 2f;
    [SerializeField] protected float _upwardForce = 2f;

    [SerializeField] protected int _ejectDelayMs = 150;
    [SerializeField] private bool hasSpawned = false; // Flag to prevent multiple triggerings
    [SerializeField] private bool willRespawn = true;

    private bool _isEjecting = false;

    [Header("UI Info")]
    [SerializeField] private TextMeshProUGUI _resourceInfoText;

    private async void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            await EjectItemsWithDelay();
        }
    }

    private void Start()
    {
        _resourceInfoText.text = _resources.Count.ToString();
    }

    private void UpdateUIResource()
    {
        _resourceInfoText.text = _resources.Count.ToString();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isEjecting) return;

        if (_resources.Count < _holderLimit)
        {
            if (other.GetComponent<MineralObject>() && !_resources.Contains(other.GetComponent<MineralObject>().gameObject))
            {
                GameObject targetGarbageObj = other.GetComponent<MineralObject>().gameObject;

                _resources.Add(targetGarbageObj);

                // Parent and snap
                targetGarbageObj.transform.SetParent(_inventoryContainer.transform);
                targetGarbageObj.transform.position = _inventoryContainer.transform.position;

                // Turn the object off entirely
                targetGarbageObj.SetActive(false);
            }
        }
        else if (_resources.Count >= _holderLimit)
        {
            _resourceInfoText.text = "Full";
            _isFull = true;
        }
    }

    private async Task EjectItemsWithDelay()
    {
        if (_isEjecting) return;
        _isEjecting = true;

        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            GameObject obj = _resources[i];
            if (obj == null) continue;

            // 1. Remove from list first so OnTriggerStay definitely ignores it
            _resources.RemoveAt(i);

            // 2. Prepare the object
            obj.transform.SetParent(null);
            obj.transform.position = _inventoryContainer.transform.position;
            obj.SetActive(true);

            // 3. Apply Physics
            if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero; // Reset old movement (Unity 2023+ uses linearVelocity)
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(_inventoryContainer.transform.forward * 10f, ForceMode.Impulse);
            }

            UpdateUIResource(); // Update UI every time one pops out

            await Task.Delay(_ejectDelayMs);
        }

        // Wait a extra bit so the last item can fly away before we allow re-collection
        await Task.Delay(1000);

        _isEjecting = false;
        _isFull = false;
        UpdateUIResource();
    }

    private IEnumerator SpawnSequence()
    {
        hasSpawned = true; // Block further execution

        for (int i = 0; i < _holderLimit; i++)
        {
            yield return new WaitForSeconds(_spawnDelaySeconds);
        }
    }

    public void SpawnResource(int targetType)
    {
        if (_resources == null || _resources.Count <= targetType) return;

        GameObject prefab = _resources[targetType];
        GameObject spawnedObj = Instantiate(prefab, resourceSpawnPoint.transform.position, Quaternion.identity);

        DisableCollision(0.5f, spawnedObj);
    }
    private IEnumerator DisableCollision(float timer, GameObject ore)
    {
        ore.GetComponent<MeshCollider>().enabled = false;
        ore.GetComponent<SphereCollider>().enabled = false;
        if (ore.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(Vector3.up * _upwardForce, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(timer);
        ore.GetComponent<MeshCollider>().enabled = true;
        ore.GetComponent<SphereCollider>().enabled = true;
    }


    public void MoveToWaypoint(int Pos)
    {
        waypointIndex += Pos;
        waypointIndex = Mathf.Clamp(waypointIndex, 0, waypoints.Count - 1);

        Debug.Log(waypointIndex);
    }

    private void ChangePosToWaypoint(int Index)
    {
        Vector3 dir = waypoints[waypointIndex].position - transform.position;

        /*float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, angleZ);*/

        transform.position = Vector3.MoveTowards(transform.position, waypoints[Index].position, speed * Time.deltaTime);

        LookAtWaypoint(waypointIndex);
    }

    private void LookAtWaypoint(int wpIndex)
    {
        Vector3 targetPos = waypoints[wpIndex].position;

        // Keep Y the same as the cart to prevent tilting
        targetPos.y = cartAsset.transform.position.y;

        // Calculate direction to target
        Vector3 direction = targetPos - cartAsset.transform.position;

        if (direction != Vector3.zero) // avoid zero-length error
        {
            // Create rotation looking at the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate only on Y axis
            Vector3 euler = cartAsset.transform.eulerAngles;
            euler.y = Mathf.MoveTowardsAngle(
                euler.y,
                targetRotation.eulerAngles.y +90,
                180f * Time.deltaTime // rotation speed in degrees/sec
            );

            cartAsset.transform.eulerAngles = euler;
        }
    }
}