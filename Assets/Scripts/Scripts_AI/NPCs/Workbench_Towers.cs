using System.Resources;
using TMPro;
using UnityEngine;

public class Workbench_Towers : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TowerCategoryData_SO offensiveTowerData;
    [SerializeField] private DropResourceManager gold;

    [Header("Unlock Tower Vars")]
    [SerializeField] private upgradeResourceType oreToSpend;
    [SerializeField] private int towerUnlockCost;
    [SerializeField] private string towerUnlock;

    private bool playerInside = false;

    private void Start()
    {
        upgradeText.enabled = false;
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            UnlockTowers(towerUnlock, oreToSpend, towerUnlockCost);
        }
    }

    public void UnlockTowers(string towerName, upgradeResourceType oreTypeToSpend, int customCost)
    {
        foreach (CardInfo card in offensiveTowerData.cards)
        {
            if (card.towerName != towerName)
                continue;

            // Already unlocked
            if (!card.islocked)
            {
                Debug.Log(card.towerName + " is already unlocked!");
                return;
            }

            // Check if player has enough of the chosen ore
            int playerAmount = gold.GetResourceType(oreTypeToSpend);

            if (playerAmount >= customCost)
            {
                gold.SpendingToResourceType(oreTypeToSpend, customCost);

                // Unlock the tower
                card.islocked = false;

                Debug.Log("Unlocked " + card.towerName + " using " + customCost + " " + oreTypeToSpend);
            }
            else
            {
                Debug.Log("Not enough " + oreTypeToSpend + " to unlock " + card.towerName);
            }
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = true;

            upgradeText.enabled = true;

            //  pickaxeRendTable.material.mainTexture = pickaxeRend.material.mainTexture;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;

        upgradeText.enabled = false;
    }
}
