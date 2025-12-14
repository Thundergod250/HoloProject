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
            var node = GameManager.Instance.CurrentTowerNode;

            if (node != null && node.towerController != null)
            {
                // ✅ Despawn using the controller
                GameManager.Instance.DespawnTower(node.towerController);
            }

            // ✅ Pass prefab to GameManager via event
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        }
        else
        {
            Debug.Log("Not enough gold to buy tower.");
        }
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
}