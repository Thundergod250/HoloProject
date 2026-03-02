using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCardManager : MonoBehaviour, IPoolable
{
    public Image TowerImage;
    public TextMeshProUGUI TowerName;
    public TextMeshProUGUI Description;
    public Image OreCostImage;
    public TextMeshProUGUI OreCost;
    public Button Button;
    public GameObject TowerPrefab;
    public GameObject lockFilter;

    private GameObject sourcePrefab;
    
    [Header("Tower Stats")]
    public int TowerHealth;
    public int CurrentDamage;
    public int DamageIncrease;
    public int CurrentFireRate;
    public int FireRateIncrease;

    [SerializeField] public upgradeResourceType upgradeResourceType;

    public int GetCostValue()
    {
        return int.TryParse(OreCost.text, out int value) ? value : 0;
    }

    public void ResetCard(CardInfo info)
    {
        upgradeResourceType = info.neededUpgradeResourceType;
        TowerName.text = info.towerName;
        Description.text = info.description;
        OreCostImage.sprite = info.oreIcon;
        OreCost.text = info.oreCost.ToString();
        TowerImage.sprite = info.towerIcon;
        TowerPrefab = info.towerPrefab;
    }
    

    public void SetSourcePrefab(GameObject prefab) => sourcePrefab = prefab;
    public GameObject GetSourcePrefab() => sourcePrefab;
}
