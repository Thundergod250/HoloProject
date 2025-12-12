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

    [HideInInspector] public TowerNodeManager CurrentTowerNode; // 👈 single source of truth

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
            ObjectPooling.Instance.Return(CurrentTowerNode.towerNodeBuildingPrefab, CurrentTowerNode.towerNodeBuilding);
        }

        // Spawn new tower
        GameObject tower = ObjectPooling.Instance.Get(towerPrefab, CurrentTowerNode.spawnTransform.parent);
        tower.transform.position = CurrentTowerNode.spawnTransform.position;
        tower.transform.rotation = CurrentTowerNode.spawnTransform.rotation;

        CurrentTowerNode.towerNodeBuilding = tower;
        CurrentTowerNode.towerNodeBuildingPrefab = towerPrefab;

        Debug.Log($"Spawned tower at {CurrentTowerNode.name}");

        // Clear reference
        CurrentTowerNode = null;
    }

    public void DespawnTower(GameObject towerPrefab, GameObject towerInstance)
    {
        if (towerPrefab == null || towerInstance == null) return;

        ObjectPooling.Instance.Return(towerPrefab, towerInstance);
        Debug.Log($"Tower {towerInstance.name} returned to pool.");
    }
}
