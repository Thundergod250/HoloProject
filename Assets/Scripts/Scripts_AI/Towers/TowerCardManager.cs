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

    public PageData towerPageData;

    private GameObject sourcePrefab;
    
    [Header("Tower Stats")]
    public int TowerHealth;
    public int CurrentDamage;
    public int DamageIncrease;
    public int CurrentFireRate;
    public int FireRateIncrease;

    [SerializeField] public upgradeResourceType upgradeResourceType;
    [SerializeField] private bool _enableDescription = false;
    [SerializeField] private GameObject _descriptionHolderObj;
    [SerializeField] private Image[] _hideUnHideImages;

    public int GetCostValue()
    {
        return int.TryParse(OreCost.text, out int value) ? value : 0;
    }

    public void ResetCard(CardInfo info)
    {
        upgradeResourceType = info.neededUpgradeResourceType;
        TowerName.text = info.towerName;
        //Description.text = info.description;

        Description.text = info.pageData.pageDescription;
        OreCostImage.sprite = info.oreIcon;
        OreCost.text = info.oreCost.ToString();
        TowerImage.sprite = info.towerIcon;
        TowerPrefab = info.towerPrefab;
        towerPageData = info.pageData;
    }
    
    public void OnDescriptionButtonClicked()
    {
        if (!_enableDescription) // true
        {
            _descriptionHolderObj.SetActive(true);
            _hideUnHideImages[1].gameObject.SetActive(true);
            _hideUnHideImages[0].gameObject.SetActive(false);

            _enableDescription = true;
        }
        else if(_enableDescription)
        {
            _descriptionHolderObj.SetActive(false);
            _hideUnHideImages[1].gameObject.SetActive(false);
            _hideUnHideImages[0].gameObject.SetActive(true);
            _enableDescription = false;
        }
    }

    public void SetSourcePrefab(GameObject prefab) => sourcePrefab = prefab;
    public GameObject GetSourcePrefab() => sourcePrefab;
}
