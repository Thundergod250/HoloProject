using UnityEngine;

public class DropResourceManager : MonoBehaviour
{
    public enum upgradeResourceType
    {
        Copper,
        Iron,
        Mythril,
        Gold
    };

    [SerializeField] int copperResources = 0;
    [SerializeField] int ironResources = 0;
    [SerializeField] int mythrilResources = 0;
    [SerializeField] int goldResources = 0;

    // Idea is to have requirement to make upgrades to Tools

    // Things we can say, Dealing Damage is faster 1 - 5 - 10 - 30 respectively
    // Requirements 5 Cop, 10 Iron, 25 Mythril, 50 Gold
}
