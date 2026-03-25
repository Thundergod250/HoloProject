using UnityEngine;
using UnityEngine.Events;

public class TowerController : MonoBehaviour
{
    [HideInInspector] public GameObject towerPrefab;   // prefab used to spawn this tower
    [HideInInspector] public GameObject towerInstance; // actual GameObject (this one)

    [Header("Identity")]
    [SerializeField] private string towerNameID;

    public UnityEvent EvtOnIncreaseTowerMainDamage;
    public UnityEvent EvtOnIncreaseTowerAtkRate; 

    [SerializeField] private TowerCategoryData_SO thisTowerUpgradeCards_SO; 
    [SerializeField] private TowerCategoryData_SO thisTowerDataCards_SO;
    [SerializeField] private Transform attackLocation; 
    public Health TowerHealth;

    [SerializeField] private ParticleSystem _smokeVFX;
    [SerializeField] private bool _smokeVFXActive = false;

    private void LateUpdate()
    {
        if (_smokeVFX == null)
        {
            return;
        }
        else
        {
            if ((TowerHealth.GetCurrentHealth() < TowerHealth.GetMaxHealth() / 2) && !_smokeVFXActive)
            {
                _smokeVFXActive = true;
                _smokeVFX.Play();
            }

            else if ((TowerHealth.GetCurrentHealth() >= TowerHealth.GetMaxHealth() / 2) && _smokeVFXActive)
            {
                _smokeVFXActive = false;
                _smokeVFX.Stop();
            }
        }
    }

    private void Awake() => towerInstance = gameObject;

    public string GetTowerNameID() => towerNameID;

    public void IncreaseTowerMainDamage() => EvtOnIncreaseTowerMainDamage?.Invoke();

    public void DespawnCurrentTower()
    {
        // Search parent for TowerNodeManager
        TowerNodeManager nodeManager = GetComponentInParent<TowerNodeManager>();
        if (nodeManager != null)
            nodeManager.DespawnTower();
        else
            Debug.LogWarning($"{name} could not find TowerNodeManager in parent hierarchy.");

    }


    public TowerCategoryData_SO GetUpgradeData() => thisTowerUpgradeCards_SO;
    public TowerCategoryData_SO GetTowerData() => thisTowerDataCards_SO;

    public Transform GetAttackLocation() => attackLocation;
}