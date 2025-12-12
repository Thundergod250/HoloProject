using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Global reference to the PlayerController
    public PlayerController PlayerController;
    public CameraManager CameraManager;
    public UI_Manager UIManager;
    public GoldManager GoldManager;
    public ObjectPooling ObjectPooling;
    
    [HideInInspector] public Transform CurrentTowerSpawn; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnTower(GameObject towerPrefab)
    {
        if (CurrentTowerSpawn == null)
        {
            Debug.LogWarning("No spawn point assigned.");
            return;
        }

        if (towerPrefab == null)
        {
            Debug.LogWarning("No tower prefab provided.");
            return;
        }

        // ✅ Use ObjectPooling instead of Instantiate
        GameObject tower = ObjectPooling.Instance.Get(towerPrefab, CurrentTowerSpawn.parent);
        tower.transform.position = CurrentTowerSpawn.position;
        tower.transform.rotation = CurrentTowerSpawn.rotation;

        Debug.Log($"Spawned tower at {CurrentTowerSpawn.position}");

        // Clear spawn reference
        CurrentTowerSpawn = null;
    }
    
    public void DespawnTower(GameObject towerPrefab, GameObject towerInstance)
    {
        if (towerPrefab == null || towerInstance == null) return;

        ObjectPooling.Instance.Return(towerPrefab, towerInstance);
        Debug.Log($"Tower {towerInstance.name} returned to pool.");
    }
}