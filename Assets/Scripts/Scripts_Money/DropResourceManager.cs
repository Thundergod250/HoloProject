using UnityEngine;
public enum upgradeResourceType
{
    Copper,
    Iron,
    Mithril,
    Gold
};
public class DropResourceManager : MonoBehaviour
{
    [SerializeField] int copperResources = 0;
    [SerializeField] int ironResources = 0;
    [SerializeField] int mithrilResources = 0;
    [SerializeField] int goldResources = 0;

    public int CopperHold => copperResources;
    public int IronHold => ironResources;
    public int MythrilHold => mithrilResources;
    public int GoldHold => goldResources;

    public void AddingToResourceType(upgradeResourceType resourceTarget, int amount)
    {
        if (resourceTarget == upgradeResourceType.Copper)
        {
            copperResources += Mathf.Max(0, amount);
        }
        else if (resourceTarget == upgradeResourceType.Iron)
        {
            ironResources += Mathf.Max(0, amount);
        }
        else if (resourceTarget == upgradeResourceType.Mithril)
        {
            mithrilResources += Mathf.Max(0, amount);
        }
        else if (resourceTarget == upgradeResourceType.Gold)
        {
            goldResources += Mathf.Max(0, amount);
        }
    }
    public void SpendingToResourceType(upgradeResourceType resourceTarget, int amount)
    {
        if (resourceTarget == upgradeResourceType.Copper && copperResources > 0)
        {
            copperResources -= Mathf.Max(0, amount);
        }
        else if (resourceTarget == upgradeResourceType.Iron && ironResources > 0)
        {
            ironResources -= Mathf.Max(0, amount);
        }
        else if (resourceTarget == upgradeResourceType.Mithril && mithrilResources > 0)
        {
            mithrilResources -= Mathf.Max(0, amount);
        }
        else if (resourceTarget == upgradeResourceType.Gold && goldResources > 0)
        {
            goldResources -= Mathf.Max(0, amount);
        }
    }

    public int GetResourceType(upgradeResourceType resourceTarget)
    {
        if (resourceTarget == upgradeResourceType.Copper)
        {
            return copperResources;
        }
        else if (resourceTarget == upgradeResourceType.Iron)
        {
            return ironResources;
        }
        else if (resourceTarget == upgradeResourceType.Mithril)
        {
            return mithrilResources;
        }
        else if (resourceTarget == upgradeResourceType.Gold)
        {
            return goldResources;
        }

        else return 0;
    }

    // Idea is to have requirement to make upgrades to Tools

    // Things we can say, Dealing Damage is faster 1 - 5 - 10 - 30 respectively
    // Requirements 5 Cop, 10 Iron, 25 Mythril, 50 Gold
}
