using System;
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

    public static event Action<string> OnResourceShortage;

    [SerializeField] private AudioClip _buyTowerClip;

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
            // GameManager.Instance.UIManager.UI_Gold.NotEnoughResource(resourceTarget);
            // _promptWarning.SetPromptTextDisplay("Not Enough ores: " + resourceTarget.ToString() );

            string message = "Not Enough ores: " + resourceTarget.ToString();
            OnResourceShortage?.Invoke(message);

            return false;
        }
        Debug.Log("Skipped whole code");
        return false;
    }

    public void _BuyButtonClicked()
    {
        // Get fresh data from the manager in case the card changed
        upgradeResourceType targetType = TowerCardManager.upgradeResourceType;
        int currentCost = TowerCardManager.GetCostValue();

        if (TrySpendOreReqiurement(targetType, currentCost))
        {
            DropResourceManager?.SpendingToResourceType(targetType, currentCost);

            Debug.Log($"Bought Tower using {targetType}");

            if (_buyTowerClip != null)
            {
                AudioManager.Instance?.PlaySFXOnce(_buyTowerClip);
            }

            DespawnCurrentTower();
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        }
    }

    public void _DespawnButtonClicked()
    {
        upgradeResourceType targetType = TowerCardManager.upgradeResourceType;
        int currentCost = TowerCardManager.GetCostValue();

        // Logic: If we despawn, we usually refund. 
        // If you want to spend to despawn, keep TrySpend. If it's a refund, just Add.
        DespawnCurrentTower();
        DropResourceManager?.AddingToResourceType(targetType, currentCost);
        Debug.Log($"Refunded {currentCost} {targetType}");
    }

    public void _RepairButtonClicked()
    {
        upgradeResourceType targetType = TowerCardManager.upgradeResourceType;
        int currentCost = TowerCardManager.GetCostValue();

        if (TrySpendOreReqiurement(targetType, currentCost))
        {
            DropResourceManager?.SpendingToResourceType(targetType, currentCost);
            CurrentTowerNode?.towerController?.TowerHealth?.Heal(
                CurrentTowerNode.towerController.TowerHealth.GetMaxHealth()
            );
        }


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void _IncreaseDamageButtonClicked()
    {
        upgradeResourceType targetType = TowerCardManager.upgradeResourceType;
        int currentCost = TowerCardManager.GetCostValue();

        if (TrySpendOreReqiurement(targetType, currentCost))
        {
            DropResourceManager?.SpendingToResourceType(targetType, currentCost);
            CurrentTowerNode?.towerController?.IncreaseTowerMainDamage();
            Debug.Log("Tower Damage Level Increased");
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
