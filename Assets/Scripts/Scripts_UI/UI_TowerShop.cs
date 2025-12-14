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

    private List<GameObject> activeCards = new List<GameObject>();
    private TowerCategoryData_SO currentCategory = null;

    public void OpenTowerUpgrades() => TrySpawnCategory(towerUpgradesData);
    public void OpenOffensiveTowers() => TrySpawnCategory(offensiveTowersData);
    public void OpenDefensiveTowers() => TrySpawnCategory(defensiveTowersData);
    public void OpenUtilityTowers() => TrySpawnCategory(utilityTowersData);

    private void TrySpawnCategory(TowerCategoryData_SO data)
    {
        if (currentCategory == data) return; // prevent double-spawn
        currentCategory = data;
        SpawnCards(data);
    }

    private void SpawnCards(TowerCategoryData_SO data)
    {
        // Clear previous cards
        foreach (var card in activeCards)
        {
            TowerCardManager manager = card.GetComponent<TowerCardManager>();
            if (manager != null && manager.GetSourcePrefab() != null)
            {
                ObjectPooling.Instance.Return(manager.GetSourcePrefab(), card);
            }
            else
            {
                Destroy(card); // fallback
            }
        }
        activeCards.Clear();

        // Spawn new cards
        foreach (var cardInfo in data.cards)
        {
            // Use the prefab stored in CardInfo now
            GameObject cardGO = ObjectPooling.Instance.Get(cardInfo.towerCardPrefab, cardParent);

            TowerCardManager card = cardGO.GetComponent<TowerCardManager>();
            card.ResetCard(cardInfo);

            // Track the prefab for pooling (per-card prefab)
            card.SetSourcePrefab(cardInfo.towerCardPrefab);

            BuyTower buyTower = cardGO.GetComponent<BuyTower>();
            if (buyTower != null)
                buyTower.TowerCardManager = card;

            activeCards.Add(cardGO);
        }
    }
}