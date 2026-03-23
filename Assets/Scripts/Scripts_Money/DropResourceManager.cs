using System.Collections;
using TMPro;
using UnityEngine;
public enum upgradeResourceType
{
    Copper,
    Iron,
    Mithril,
    Gold
};

[System.Serializable]
public class ResourceData // This is just the "Values"
{
    public int copperResources = 0;
    public int ironResources = 0;
    public int mithrilResources = 0;
    public int goldResources = 0;
}

public class DropResourceManager : MonoBehaviour
{
    public ResourceData resourceData;

    [Header("Text")]
    [SerializeField] GameObject[] addedTextFX; // 0 for Copper, 1 for Iron, 2 for Gold, 3 for Mithril

    public int CopperHold => resourceData.copperResources;
    public int IronHold => resourceData.ironResources;
    public int MythrilHold => resourceData.mithrilResources;
    public int GoldHold => resourceData.goldResources;

    public void AddingToResourceType(upgradeResourceType resourceTarget, int amount)
    {
        if (resourceTarget == upgradeResourceType.Copper)
        {
            resourceData.copperResources += Mathf.Max(0, amount);
            StartCoroutine(ShowAddedTextFX(0));
        }
        else if (resourceTarget == upgradeResourceType.Iron)
        {
            resourceData.ironResources += Mathf.Max(0, amount);
            StartCoroutine(ShowAddedTextFX(1));
        }
        else if (resourceTarget == upgradeResourceType.Gold)
        {
            resourceData.goldResources += Mathf.Max(0, amount);
            StartCoroutine(ShowAddedTextFX(2));
        }
        else if (resourceTarget == upgradeResourceType.Mithril)
        {
            resourceData.mithrilResources += Mathf.Max(0, amount);
            StartCoroutine(ShowAddedTextFX(3));
        }


    }
    public void SpendingToResourceType(upgradeResourceType resourceTarget, int amount)
    {
        if (resourceTarget == upgradeResourceType.Copper && resourceData.copperResources > 0)
        {
            resourceData.copperResources -= Mathf.Max(0, amount);
        }
        else if (resourceTarget == upgradeResourceType.Iron && resourceData.ironResources > 0)
        {
            resourceData.ironResources -= Mathf.Max(0, amount);  
        }
        else if (resourceTarget == upgradeResourceType.Mithril && resourceData.mithrilResources > 0)
        {
            resourceData.mithrilResources -= Mathf.Max(0, amount);
        }
        else if (resourceTarget == upgradeResourceType.Gold && resourceData.goldResources > 0)
        {
            resourceData.goldResources -= Mathf.Max(0, amount);
        }
    }

    public int GetResourceType(upgradeResourceType resourceTarget)
    {
        if (resourceTarget == upgradeResourceType.Copper)
        {
            return resourceData.copperResources;
        }
        else if (resourceTarget == upgradeResourceType.Iron)
        {
            return resourceData.ironResources;
        }
        else if (resourceTarget == upgradeResourceType.Mithril)
        {
            return resourceData.mithrilResources;
        }
        else if (resourceTarget == upgradeResourceType.Gold)
        {
            return resourceData.goldResources;
        }

        else return 0;
    }

    // Idea is to have requirement to make upgrades to Tools

    // Things we can say, Dealing Damage is faster 1 - 5 - 10 - 30 respectively
    // Requirements 5 Cop, 10 Iron, 25 Mythril, 50 Gold
        
    private IEnumerator ShowAddedTextFX(int textint)
    {
        addedTextFX[textint].SetActive(true);

        Debug.Log("ShowText");

        yield return new WaitForSeconds(.75f);

        addedTextFX[textint].SetActive(false);
    }
}
