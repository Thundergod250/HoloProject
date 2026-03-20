using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class TrashHeap_ResourceSpawner : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _resources;
    [SerializeField] protected Transform resourceSpawnPoint;
    [SerializeField] protected float _spawnDelaySeconds = 2f;
    [SerializeField] protected float _upwardForce = 2f;

    [SerializeField] protected int _howManyToSpawn = 1;

    [SerializeField] public GarbageObject.GarbageGroup _garbageGroupType;
    [SerializeField] public bool _randomized = false;

    [SerializeField] public Health _health;
    [SerializeField] private GameObject _canvasUI;
    [SerializeField] private GameObject _toRotateOnPlayer;
    [SerializeField] private Slider healthSlider;

    private PlayerController _playerController;

    [SerializeField] bool ForTesting = false;
    [SerializeField] private bool hasSpawned = false; // Flag to prevent multiple triggerings
    [SerializeField] private bool willRespawn = true;

    [SerializeField] private VisualEffect _highlightColor;

    [SerializeField] private Animator _animAttackLOreLode;

    private static readonly int GlowColorID = Shader.PropertyToID("Glow Color");
    [SerializeField] private string CopperPropName = "CopperColor";
    [SerializeField] private string IronPropName = "IronColor";
    [SerializeField] private string GoldPropName = "GoldColor"; 
    public Color CopperColor = new Color(0.8f, 0.5f, 0.200f);
    public Color IronColor = new Color(0.6f, 0.6f, 0.65f);
    public Color GoldColor = new Color(1.0f, 0.85f, 0.0f);

    [SerializeField] private Material[] _oreMaterials;
    [SerializeField] private GameObject _oreObjReference;

    private void Start()
    {
        if (_canvasUI != null)
        {
            healthSlider.maxValue = _health.GetMaxHealth();
            _canvasUI.gameObject.SetActive(false);
        }

        SetHeapHighLight();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent<PlayerController>(out var player))
        if (other.GetComponent<PlayerController>())
        {
            _playerController = other.GetComponent<PlayerController>();
           // SetRandomized(); // Fixed logic inside this function
            _canvasUI.gameObject.SetActive(true);

            _howManyToSpawn = Random.Range(1, 5);

            if (ForTesting && !hasSpawned)
            {
                StartCoroutine(SpawnSequence());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _playerController = null;
            _canvasUI.gameObject.SetActive(false);
        }
    }

    private void SetHeapHighLight()
    {
        if (_highlightColor == null) return;

        // Pick the color based on the ore type
        Color targetColor = Color.white;

        if (_garbageGroupType == GarbageObject.GarbageGroup.CopperOre)
        {
            targetColor = CopperColor;
            _oreObjReference.GetComponent<MeshRenderer>().material = _oreMaterials[0];
        }
        else if (_garbageGroupType == GarbageObject.GarbageGroup.IronOre)
        {
            targetColor = IronColor;
            _oreObjReference.GetComponent<MeshRenderer>().material = _oreMaterials[1];
        }
        else if (_garbageGroupType == GarbageObject.GarbageGroup.GoldOre)
        {
            targetColor = GoldColor;
            _oreObjReference.GetComponent<MeshRenderer>().material = _oreMaterials[2];
        }

        // 2. Apply it to the VFX
        float intensity = 5.0f; // Increase for more glow
        _highlightColor.SetVector4(GlowColorID, targetColor * intensity);
    }

    private void SetRandomized()
    {
        // Random.Range(int, int) max is EXCLUSIVE. 
        // To get 1 or 2, you must use (1, 3).
        int testRandom = Random.Range(1, 3);
        _randomized = (testRandom == 1);
    }

    public void AttackTriggerAnimation()
    {
        _animAttackLOreLode.Play("Wiggle");
    }
    public void StopTriggerAnimation()
    {
        _animAttackLOreLode.Play("Idle");
    }


    private void Update()
    {
        healthSlider.value = _health.GetCurrentHealth();

        if (_health.GetCurrentHealth() <= 0 && !hasSpawned)
        {
            StartCoroutine(SpawnSequence());
        }

        if (_playerController != null && _canvasUI.gameObject.activeSelf)
        {
            //_toRotateOnPlayer.transform.LookAt(_playerController.transform.position);
            _toRotateOnPlayer.transform.Rotate(Vector3.up);
        }
    }

    // This handles the timing and the loop
    private IEnumerator SpawnSequence()
    {
        hasSpawned = true; // Block further execution

        for (int i = 0; i < _howManyToSpawn; i++)
        {
            yield return new WaitForSeconds(_spawnDelaySeconds);
            SetResourceType(); // Calls your specific logic
        }

        if (willRespawn)
        {
            DisableThisHeap();
        }
        else if(!willRespawn)
        {
            Destroy(this.gameObject);
        }
    }

    private void DisableThisHeap()
    {
        this.gameObject.SetActive(false);
    }

    public void SetResourceType()
    {
        Debug.Log("Set Resource Logic Running");
        if (!_randomized)
        {
            // Use your Enum-based logic
            if (_garbageGroupType == GarbageObject.GarbageGroup.Plastic) SpawnResource(0);
            else if (_garbageGroupType == GarbageObject.GarbageGroup.Organic) SpawnResource(1);
            else if (_garbageGroupType == GarbageObject.GarbageGroup.Metal) SpawnResource(2);

            else if (_garbageGroupType == GarbageObject.GarbageGroup.CopperOre)
            {
                //i = Random.Range(3, 6);
                SpawnResource(3);
            }
            else if (_garbageGroupType == GarbageObject.GarbageGroup.IronOre)
            {
                //i = Random.Range(3, 6);
                SpawnResource(4);
            }
            else if (_garbageGroupType == GarbageObject.GarbageGroup.GoldOre)
            {
                //i = Random.Range(3, 6);
                SpawnResource(5);
            }
        }
        else
        {
            int randomType = Random.Range(0, 101); // 0 to 100

            // Fixed the range logic and removed the invalid !> operator
            if (randomType <= 33) SpawnResource(4);
            else if (randomType <= 66) SpawnResource(6);
            else SpawnResource(3);
        }
    }

    public void SpawnResource(int targetType)
    {
        if (_resources == null || _resources.Count <= targetType) return;

        GameObject prefab = _resources[targetType];
        GameObject spawnedObj = Instantiate(prefab, resourceSpawnPoint.transform.position, Quaternion.identity);

        DisableCollision(0.5f, spawnedObj);

        /*if (spawnedObj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(Vector3.up * _upwardForce, ForceMode.Impulse);
        }
        */
       
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

    public void ResetBool()
    {
        hasSpawned = false; 
    }
}
