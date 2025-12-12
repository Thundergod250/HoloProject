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
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);

        foreach (var cardInfo in data.cards)
        {
            GameObject cardGO = Instantiate(data.towerCardPrefab, cardParent);
            TowerCardManager card = cardGO.GetComponent<TowerCardManager>();

            card.Title.text = cardInfo.title;
            card.Description.text = cardInfo.description;
            card.Cost.text = cardInfo.cost.ToString();
            card.Image.sprite = cardInfo.icon;

            BuyTower buyTower = cardGO.GetComponent<BuyTower>();
            if (buyTower != null)
                buyTower.TowerCardManager = card;
        }
    }
}