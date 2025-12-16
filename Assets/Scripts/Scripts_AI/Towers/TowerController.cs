using UnityEngine;
using UnityEngine.Events;

public class TowerController : MonoBehaviour
{
    [HideInInspector] public GameObject towerPrefab;   // prefab used to spawn this tower
    [HideInInspector] public GameObject towerInstance; // actual GameObject (this one)
    
    public UnityEvent EvtOnIncreaseTowerMainDamage;
    public UnityEvent EvtOnIncreaseTowerAtkRate; 

    [SerializeField] private TowerCategoryData_SO thisTowerUpgradeCards_SO; 
    public Health TowerHealth;
    
    private void Awake()
    {
        towerInstance = gameObject;
    }

    public void IncreaseTowerMainDamage()
    {
        EvtOnIncreaseTowerMainDamage?.Invoke();
    }

    public TowerCategoryData_SO GetUpgradeData() => thisTowerUpgradeCards_SO;
}