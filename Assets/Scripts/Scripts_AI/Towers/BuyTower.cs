using UnityEngine;
using UnityEngine.Events;

public class BuyTower : MonoBehaviour
{
    public UnityEvent EvtOnBuySuccessful;
    public TowerCardManager TowerCardManager;

    public void _BuyButtonClicked()
    {
        int cost = TowerCardManager.GetCostValue();

        if (GameManager.Instance.GoldManager?.SpendGold(cost) == true)
        {
            EvtOnBuySuccessful?.Invoke();
        }
        else
        {
            Debug.Log("Not enough gold to buy tower.");
        }
    }
}