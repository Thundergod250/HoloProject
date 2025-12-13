using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        // Despawn existing tower
        if (CurrentTowerNode.towerNodeBuilding != null)
        {
            DespawnTower(CurrentTowerNode.towerNodeBuildingPrefab, CurrentTowerNode.towerNodeBuilding);
        }

        // Get from pool and parent to node
        GameObject tower = ObjectPooling.Instance.Get(towerPrefab, CurrentTowerNode.transform);
        tower.transform.position = CurrentTowerNode.spawnTransform.position;
        tower.transform.rotation = towerPrefab.transform.rotation;
        tower.transform.localScale = towerPrefab.transform.localScale;

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
