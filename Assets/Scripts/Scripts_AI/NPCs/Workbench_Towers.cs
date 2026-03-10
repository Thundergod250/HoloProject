using System.Resources;
using TMPro;
using UnityEngine;

public class Workbench_Towers : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI reclaimText;
    [SerializeField] private TowerCategoryData_SO offensiveTowerData;
    [SerializeField] private DropResourceManager gold;

    [Header("Unlock Tower Vars")]
    [SerializeField] private upgradeResourceType oreToSpendTower;
    [SerializeField] private int towerUnlockCost;
    [SerializeField] private string towerUnlock;

    [Header("Reclaim Tower")]
    [SerializeField] private GameObject destroyedState;
    [SerializeField] private GameObject fixedState;


    private bool isReclaimed = false;
    private bool playerInside = false;

    private void Start()
    {
        upgradeText.enabled = false;
        reclaimText.enabled = false;

        // Reset to Destroyed
        destroyedState.SetActive(true);
        fixedState.SetActive(false);
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            UnlockTowers(towerUnlock, oreToSpendTower, towerUnlockCost);
        }
    }

    public void UnlockTowers(string towerName, upgradeResourceType oreTypeToSpend, int customCost)
    {
        foreach (CardInfo card in offensiveTowerData.cards)
        {
            if (towerName.ToLower() == "all")
            {
                if (card.islocked)
                {
                    card.islocked = false;
                    Debug.Log("Cheat Unlock: Unlocked " + card.towerName + " for free!");
                }
                else Debug.Log(card.towerName + " is already unlocked!");

                continue;
            }

            if (card.towerName != towerName)
                continue;

            if (!card.islocked)
            {
                Debug.Log(card.towerName + " is already unlocked!");
                return;
            }

            int playerAmount = gold.GetResourceType(oreTypeToSpend);

            if (playerAmount >= customCost)
            {
                gold.SpendingToResourceType(oreTypeToSpend, customCost);

                card.islocked = false;

                ReclaimTower();

                Debug.Log("Unlocked " + card.towerName + " using " + customCost + " " + oreTypeToSpend);
            }
            else
            {
                Debug.Log("Not enough " + oreTypeToSpend + " to unlock " + card.towerName);
            }
            break;
        }
    }

    private void ChangeTexture()
    {
        destroyedState.SetActive(false);
        fixedState.SetActive(true);
    }

    private void ReclaimTower()
    {
        ChangeTexture();
        reclaimText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = true;

            upgradeText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;

        upgradeText.enabled = false;
    }
}
