using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Global references
    public PlayerController PlayerController;
    public CameraManager CameraManager;
    public UI_Manager UIManager;
    public GoldManager GoldManager;
    public ObjectPooling ObjectPooling;

    [HideInInspector] public TowerNodeManager CurrentTowerNode;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public GameObject SpawnObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogWarning("No prefab provided to SpawnObject.");
            return null;
        }

        GameObject obj = ObjectPooling.Instance.Get(prefab, parent);

        // Apply transform overrides AFTER parenting
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.localScale = prefab.transform.localScale;

        return obj;
    }
    
    public void SpawnTower(GameObject towerPrefab)
    {
        if (CurrentTowerNode == null || CurrentTowerNode.spawnTransform == null)
        {
            Debug.LogWarning("No tower node or spawn transform assigned.");
            return;
        }

        if (towerPrefab == null)
        {
            Debug.LogWarning("No tower prefab provided.");
            return;
        }

        // Despawn existing tower if present
        if (CurrentTowerNode.towerNodeBuilding != null)
        {
            DespawnTower(CurrentTowerNode.towerNodeBuildingPrefab, CurrentTowerNode.towerNodeBuilding);
        }

        // Use generic spawn
        GameObject tower = SpawnObject(
            towerPrefab,
            CurrentTowerNode.transform,
            CurrentTowerNode.spawnTransform.position,
            towerPrefab.transform.rotation // ✅ preserve prefab rotation
        );

        // Track instance and prefab
        CurrentTowerNode.towerNodeBuilding = tower;
        CurrentTowerNode.towerNodeBuildingPrefab = towerPrefab;

        Debug.Log($"Spawned tower under {CurrentTowerNode.name}");

        CurrentTowerNode = null;
    }

    public void DespawnTower(GameObject towerPrefab, GameObject towerInstance)
    {
        if (towerPrefab == null || towerInstance == null) return;

        ObjectPooling.Instance.Return(towerPrefab, towerInstance);
        Debug.Log($"Tower {towerInstance.name} returned to pool.");
    }
}
