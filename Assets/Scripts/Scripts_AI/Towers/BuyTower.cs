using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TowerBuyEvent : UnityEvent<GameObject> { }

public class BuyTower : MonoBehaviour
{
    public TowerCardManager TowerCardManager;
    public TowerBuyEvent EvtOnBuySuccessful;
    
    private TowerNodeManager CurrentTowerNode;
    private int cost;
    public upgradeResourceType targetResourceType;

    private void OnEnable()
    {
        CurrentTowerNode = GameManager.Instance.CurrentTowerNode;
        cost = TowerCardManager.GetCostValue();
    }

    // === Generic gold spending wrapper ===
    private bool TrySpendGold(int amount)
    {
        if (GameManager.Instance.GoldManager?.SpendGold(amount) == true)
            return true;

        Debug.Log("Not enough gold.");
        return false;
    }

    public bool TrySpendOreReqiurement(upgradeResourceType resourceTarget, int cost)
    {
        //GameManager.Instance.DropManager?.SpendingToResourceType(resourceTarget, amount);
        if (GameManager.Instance.DropManager?.GetResourceType(resourceTarget) >= cost)
        {
            Debug.Log("Have ores " + resourceTarget.ToString() + resourceTarget);
            return true;
        }
        else
        {
            Debug.Log("Not enough ores " + resourceTarget.ToString() + resourceTarget);
            return false;
        }
    }

    public void _BuyButtonClicked()
    {
        // OLD
        if (TrySpendGold(cost))
        {
            DespawnCurrentTower();
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        }

        // NOT WORKING
        if (TrySpendOreReqiurement(targetResourceType, cost))
        {
            GameManager.Instance.DropManager?.SpendingToResourceType(targetResourceType, cost);
            Debug.Log("Bought Tower?");
            DespawnCurrentTower();
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        }
    }

    public void _DespawnButtonClicked()
    {
        // OLD
        if (TrySpendGold(cost))
        {
            DespawnCurrentTower();
            GameManager.Instance.GoldManager?.AddGold(cost); // refund
        }

        if (TrySpendOreReqiurement(targetResourceType, cost))
        {
            DespawnCurrentTower();
            GameManager.Instance.DropManager?.AddingToResourceType(targetResourceType, cost); // refund
        }
    }

    public void _RepairButtonClicked()
    {
        // OLD
        if (TrySpendGold(cost))
        {
            CurrentTowerNode?.towerController?.TowerHealth
                ?.Heal(CurrentTowerNode.towerController.TowerHealth.GetMaxHealth());
        }

        if (TrySpendOreReqiurement(targetResourceType, cost))
        {
            CurrentTowerNode?.towerController?.TowerHealth?.Heal(CurrentTowerNode.towerController.TowerHealth.GetMaxHealth());
        }
    }

    public void _IncreaseDamageButtonClicked()
    {
        // OLD
        if (TrySpendGold(cost))
        {
            CurrentTowerNode?.towerController?.IncreaseTowerMainDamage();
            Debug.Log("Tower Damage Level Increased");
        }

        if (TrySpendOreReqiurement(targetResourceType, cost))
        {
            CurrentTowerNode?.towerController?.IncreaseTowerMainDamage();
            Debug.Log("2 Tower Damage Level Increased");
        }
    }

    // === Shared despawn logic ===
    public void DespawnCurrentTower()
    {
        var node = GameManager.Instance.CurrentTowerNode;

        if (node?.towerController != null)
        {
            GameManager.Instance.DespawnTower(node.towerController);
            node.towerController = null;
        }
        else
        {
            Debug.Log("No tower to despawn.");
        }
    }
}
