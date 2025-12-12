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
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab); 
        }
        else
        {
            Debug.Log("Not enough gold to buy tower.");
        }
    }
}
