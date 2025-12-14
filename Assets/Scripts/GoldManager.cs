using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private int playerGold = 100;
    public int PlayerGold => playerGold; // read-only property
    
    public void AddGold(int amount)
    {
        playerGold += Mathf.Max(0, amount);
    }
    
    public bool SpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            playerGold -= amount;
            return true;
        }
        return false;
    }
    
    public void ResetGold()
    {
        playerGold = 0;
    }
    
    public bool HasEnoughGold(int amount)
    {
        return playerGold >= amount;
    }
}