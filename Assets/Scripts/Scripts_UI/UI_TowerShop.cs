using UnityEngine;
using System.Collections.Generic;

public class UI_TowerShop : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform cardParent;
    [SerializeField] private TowerCategoryData_SO towerUpgradesData;
    [SerializeField] private TowerCategoryData_SO offensiveTowersData;
    [SerializeField] private TowerCategoryData_SO defensiveTowersData;
    [SerializeField] private TowerCategoryData_SO utilityTowersData;

    [Header("Shop Buttons")]
    [SerializeField] private GameObject towerUpgradesButton;
    [SerializeField] private GameObject offensiveButton;
    [SerializeField] private GameObject defensiveButton;
    [SerializeField] private GameObject utilityButton;

    private Dictionary<string, GameObject> shopButtons;
    private List<GameObject> activeCards = new List<GameObject>();
    private TowerCategoryData_SO currentCategory = null;

    private void Awake()
    {
        shopButtons = new Dictionary<string, GameObject>
        {
            { "Upgrades", towerUpgradesButton },
            { "Offensive", offensiveButton },
            { "Defensive", defensiveButton },
            { "Utility", utilityButton }
        };
    }

    public void OpenTowerUpgrades() => TrySpawnCategory(towerUpgradesData);
    public void OpenOffensiveTowers() => TrySpawnCategory(offensiveTowersData);
    public void OpenDefensiveTowers() => TrySpawnCategory(defensiveTowersData);
    public void OpenUtilityTowers() => TrySpawnCategory(utilityTowersData);

    private void TrySpawnCategory(TowerCategoryData_SO data)
    {
        if (currentCategory == data) return;
        currentCategory = data;
        SpawnCards(data);
    }

    private void SpawnCards(TowerCategoryData_SO data)
    {
        foreach (var card in activeCards)
        {
            TowerCardManager manager = card.GetComponent<TowerCardManager>();
            if (manager != null && manager.GetSourcePrefab() != null)
                ObjectPooling.Instance.Return(manager.GetSourcePrefab(), card);
            else
                Destroy(card);
        }
        activeCards.Clear();

        foreach (var cardInfo in data.cards)
        {
            GameObject cardGO = ObjectPooling.Instance.Get(cardInfo.towerCardPrefab, cardParent);
            cardGO.SetActive(true);

            TowerCardManager card = cardGO.GetComponent<TowerCardManager>();
            card.ResetCard(cardInfo);
            card.SetSourcePrefab(cardInfo.towerCardPrefab);

            BuyTower buyTower = cardGO.GetComponent<BuyTower>();
            if (buyTower != null)
                buyTower.TowerCardManager = card;

            activeCards.Add(cardGO);
        }
    }

    public void ShowShopButtons(bool showUpgrades)
    {
        shopButtons["Upgrades"].SetActive(showUpgrades);
        shopButtons["Offensive"].SetActive(true);
        shopButtons["Defensive"].SetActive(true);
        shopButtons["Utility"].SetActive(true);
    }
}
