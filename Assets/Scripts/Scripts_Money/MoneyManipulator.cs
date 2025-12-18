using UnityEngine;

public class MoneyManipulator : MonoBehaviour
{
    public void _AddMoney(int amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.GoldManager != null)
        {
            GameManager.Instance.GoldManager.AddGold(amount);
            Debug.Log($"Added {amount} gold. Current gold: {GameManager.Instance.GoldManager.PlayerGold}");
        }
    }
    
    public void _SpendMoney(int amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.GoldManager != null)
        {
            bool success = GameManager.Instance.GoldManager.SpendGold(amount);
            if (success)
                Debug.Log($"Spent {amount} gold. Current gold: {GameManager.Instance.GoldManager.PlayerGold}");
            else
                Debug.LogWarning($"Not enough gold to spend {amount}. Current gold: {GameManager.Instance.GoldManager.PlayerGold}");
        }
    }

    public void _ReduceMoney(int amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.GoldManager != null)
        {
            GameManager.Instance.GoldManager.ReduceGold(amount);
            Debug.Log($"Reduced {amount} gold (forced). Current gold: {GameManager.Instance.GoldManager.PlayerGold}");
        }
    }
}