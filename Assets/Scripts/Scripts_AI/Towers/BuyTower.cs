using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TowerBuyEvent : UnityEvent<GameObject> { }

public class BuyTower : MonoBehaviour
{
    public TowerCardManager TowerCardManager;
    public TowerBuyEvent EvtOnBuySuccessful;
    
    private TowerNodeManager CurrentTowerNode;
    
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

    public void _DespawnButtonClicked(int cost)
    {
        // ✅ Just despawn, no gold check
        DespawnCurrentTower();
        GameManager.Instance.GoldManager?.AddGold(cost);
    }

    public void _RepairButtonClicked(int cost)
    {
        if (GameManager.Instance.GoldManager?.SpendGold(cost) == true)
        {
            CurrentTowerNode.towerController.TowerHealth.Heal(CurrentTowerNode.towerController.TowerHealth.GetMaxHealth());
        }
        else
        {
            Debug.Log("Not enough gold to repair.");
        }
    }
    
    private void OnEnable()
    {
        CurrentTowerNode = GameManager.Instance.CurrentTowerNode;
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