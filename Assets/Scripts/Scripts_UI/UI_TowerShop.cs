using UnityEngine;

public class UI_TowerShop : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform cardParent;
    [SerializeField] private TowerCategoryData_SO towerUpgradesData;
    [SerializeField] private TowerCategoryData_SO offensiveTowersData;
    [SerializeField] private TowerCategoryData_SO defensiveTowersData;
    [SerializeField] private TowerCategoryData_SO utilityTowersData;

    public void OpenTowerUpgrades() => SpawnCards(towerUpgradesData);
    public void OpenOffensiveTowers() => SpawnCards(offensiveTowersData);
    public void OpenDefensiveTowers() => SpawnCards(defensiveTowersData);
    public void OpenUtilityTowers() => SpawnCards(utilityTowersData);

    private void SpawnCards(TowerCategoryData_SO data)
    {
        // Return old cards to pool instead of destroying
        foreach (Transform child in cardParent)
        {
            var pooledCard = child.gameObject;
            ObjectPooling.Instance.Return(data.towerCardPrefab, pooledCard);
        }

        // Spawn new cards from pool
        foreach (var cardInfo in data.cards)
        {
            GameObject cardGO = ObjectPooling.Instance.Get(data.towerCardPrefab, cardParent);
            TowerCardManager card = cardGO.GetComponent<TowerCardManager>();

            card.Title.text = cardInfo.title;
            card.Description.text = cardInfo.description;
            card.Cost.text = cardInfo.cost.ToString();
            card.Image.sprite = cardInfo.icon;
            card.TowerPrefab = cardInfo.towerPrefab;

            BuyTower buyTower = cardGO.GetComponent<BuyTower>();
            if (buyTower != null)
                buyTower.TowerCardManager = card;
        }
    }
}