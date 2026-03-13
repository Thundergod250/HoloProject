using System.Collections;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Workbench_Towers : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private GameObject upgradeText;
    [SerializeField] private GameObject insufficientOreUI;
    [SerializeField] private TowerCategoryData_SO offensiveTowerData;
    [SerializeField] private DropResourceManager gold;

    [SerializeField] private UI_PromtWarnings _promptWarnings;

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
        upgradeText.SetActive(false);
        insufficientOreUI.SetActive(false);

        // Reset to Destroyed
        destroyedState.SetActive(true);
        fixedState.SetActive(false);
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F) && !isReclaimed)
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

                upgradeText.SetActive(false);
            }
            else
            {
                StartCoroutine(ShowError());
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

        if (_promptWarnings != null)
        {
            _promptWarnings.SetPromptTextDisplay("You have Reclaimed a Tower");
        }

        isReclaimed = true;
    }

    public IEnumerator ShowError()
    {
        insufficientOreUI.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        insufficientOreUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && !isReclaimed)
        {
            playerInside = true;

            upgradeText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isReclaimed)
        {
            playerInside = false;

            upgradeText.SetActive(false);
        }
    }
}
