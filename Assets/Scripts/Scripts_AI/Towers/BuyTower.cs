using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TowerBuyEvent : UnityEvent<GameObject> { }

public class BuyTower : MonoBehaviour
{
    public TowerCardManager TowerCardManager;
    public TowerBuyEvent EvtOnBuySuccessful;
    
    private TowerNodeManager CurrentTowerNode;
    private DropResourceManager DropResourceManager;
    private int cost;
    public upgradeResourceType targetResourceType;

    [SerializeField] private UI_PromtWarnings _promptWarning;

    private void OnEnable()
    {
        CurrentTowerNode = GameManager.Instance.CurrentTowerNode;
        DropResourceManager = GameManager.Instance.DropManager;
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
        Debug.Log(resourceTarget + resourceTarget.ToString());

        if (DropResourceManager?.GetResourceType(resourceTarget) >= cost)
        {
            Debug.Log("Have ores " + resourceTarget.ToString() + resourceTarget);
            return true;
        }
        else if (DropResourceManager?.GetResourceType(resourceTarget) < cost)
        {
            GameManager.Instance.UIManager.UI_Gold.NotEnoughResource(resourceTarget);
            //_promptWarning.SetPromptTextDisplay("Not Enough ores: " + resourceTarget.ToString() );
            Debug.Log("Not enough ores " + resourceTarget.ToString() + resourceTarget);
            return false;
        }
        Debug.Log("Skipped whole code");
        return false;
    }

    public void _BuyButtonClicked()
    {
        // OLD
        //if (TrySpendGold(cost))
        //{
        //    DespawnCurrentTower();
        //    EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        //}

        // NOT WORKING
        if (TrySpendOreReqiurement(targetResourceType, cost))
        {
            //targetResourceType = GetComponentInChildren<CardInfo>().neededUpgradeResourceType;

            DropResourceManager?.SpendingToResourceType(targetResourceType, cost);
            Debug.Log("Bought Tower?");
            DespawnCurrentTower();
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        }
    }

    public void _DespawnButtonClicked()
    {
        // OLD
        //if (TrySpendGold(cost))
        //{
        //    DespawnCurrentTower();
        //    GameManager.Instance.GoldManager?.AddGold(cost); // refund
        //}

        if (TrySpendOreReqiurement(targetResourceType, cost))
        {
            DespawnCurrentTower();
            DropResourceManager?.AddingToResourceType(targetResourceType, cost); // refund
        }
    }

    public void _RepairButtonClicked()
    {
        // OLD
        //if (TrySpendGold(cost))
        //{
        //    CurrentTowerNode?.towerController?.TowerHealth
        //        ?.Heal(CurrentTowerNode.towerController.TowerHealth.GetMaxHealth());
        //}

        if (TrySpendOreReqiurement(targetResourceType, cost))
        {
            CurrentTowerNode?.towerController?.TowerHealth?.Heal(CurrentTowerNode.towerController.TowerHealth.GetMaxHealth());
        }
    }

    public void _IncreaseDamageButtonClicked()
    {
        // OLD
        //if (TrySpendGold(cost))
        //{
        //    CurrentTowerNode?.towerController?.IncreaseTowerMainDamage();
        //    Debug.Log("Tower Damage Level Increased");
        //}

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
