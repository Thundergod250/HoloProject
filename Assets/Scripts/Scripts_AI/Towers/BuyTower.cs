using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TowerBuyEvent : UnityEvent<GameObject> { }

public class BuyTower : MonoBehaviour
{
    public TowerCardManager TowerCardManager;
    public TowerBuyEvent EvtOnBuySuccessful;

    public void _BuyButtonClicked()
    {
        int cost = TowerCardManager.GetCostValue();

        if (GameManager.Instance.GoldManager?.SpendGold(cost) == true)
        {
            // ✅ Despawn existing tower before buying
            DespawnCurrentTower();

            // ✅ Pass prefab to GameManager via event
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        }
        else
        {
            Debug.Log("Not enough gold to buy tower.");
        }
    }

    public void _DespawnButtonClicked()
    {
        // ✅ Just despawn, no gold check
        DespawnCurrentTower();
    }

    public void _RepairButtonClicked()
    {
        int cost = TowerCardManager.GetCostValue();

        if (GameManager.Instance.GoldManager?.SpendGold(cost) == true)
        {
            // ✅ Pass prefab to GameManager via event
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        }
        else
        {
            Debug.Log("Not enough gold to repair.");
        }
    }

    // === Shared despawn logic ===
    public void DespawnCurrentTower()
    {
        var node = GameManager.Instance.CurrentTowerNode;

        if (node != null && node.towerController != null)
        {
            GameManager.Instance.DespawnTower(node.towerController);
        }
        else
        {
            Debug.Log("No tower to despawn.");
        }
    }
}