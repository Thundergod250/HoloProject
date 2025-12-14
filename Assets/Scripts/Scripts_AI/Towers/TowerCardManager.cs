using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCardManager : MonoBehaviour, IPoolable
{
    public Image Image;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cost;
    public Button Button;
    public GameObject TowerPrefab;

    private GameObject sourcePrefab;
    
    [Header("Tower Stats")]
    public int TowerHealth;
    public int CurrentDamage;
    public int DamageIncrease;
    public int CurrentFireRate;
    public int FireRateIncrease;

    public int GetCostValue()
    {
        return int.TryParse(Cost.text, out int value) ? value : 0;
    }

    public void ResetCard(CardInfo info)
    {
        Title.text = info.title;
        Description.text = info.description;
        Cost.text = info.cost.ToString();
        Image.sprite = info.icon;
        TowerPrefab = info.towerPrefab;
    }
    
    public void ResetUpgradeCard(UpgradeInfo info)
    {
        Title.text = info.title;
        Description.text = info.description;
        Cost.text = info.cost.ToString();
        Image.sprite = info.icon;
        TowerPrefab = info.towerPrefab;
        TowerHealth = info.TowerHealth;
        CurrentDamage = info.CurrentDamage;
        DamageIncrease = info.DamageIncrease;
        CurrentFireRate = info.CurrentFireRate;
        FireRateIncrease = info.FireRateIncrease;
    }

    public void SetSourcePrefab(GameObject prefab) => sourcePrefab = prefab;
    public GameObject GetSourcePrefab() => sourcePrefab;
}
