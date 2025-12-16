using UnityEngine;

public class TowerController : MonoBehaviour
{
    [HideInInspector] public GameObject towerPrefab;   // prefab used to spawn this tower
    [HideInInspector] public GameObject towerInstance; // actual GameObject (this one)

    [SerializeField] private TowerCategoryData_SO thisTowerUpgradeCards_SO; 
    public Health TowerHealth;
    
    private void Awake()
    {
        towerInstance = gameObject;
    }

    public TowerCategoryData_SO GetUpgradeData() => thisTowerUpgradeCards_SO;
}