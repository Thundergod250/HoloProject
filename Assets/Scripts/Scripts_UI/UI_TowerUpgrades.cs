using UnityEngine;
using System.Collections.Generic;

public class UI_TowerUpgrades : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform cardParent;
    private List<GameObject> activeCards = new List<GameObject>();

    private void SpawnCards(TowerUpgradeData_SO data)
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
        foreach (UpgradeInfo cardInfo in data.cards)
        {
            // ✅ Use the prefab defined in each UpgradeInfo
            GameObject cardGO = ObjectPooling.Instance.Get(cardInfo.towerCardPrefab, cardParent);

            TowerCardManager card = cardGO.GetComponent<TowerCardManager>();
            card.ResetUpgradeCard(cardInfo);
            card.SetSourcePrefab(cardInfo.towerCardPrefab); // ✅ track correct prefab

            BuyTower buyTower = cardGO.GetComponent<BuyTower>();
            if (buyTower != null)
                buyTower.TowerCardManager = card;

            activeCards.Add(cardGO);
        }
    }
}