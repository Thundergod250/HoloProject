using NUnit.Framework;
using System.Collections.Generic;
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
    [SerializeField] private ParticleSystem _reclaimParticles;

    [Header("Unlock Tower Vars")]
    [SerializeField] private upgradeResourceType oreToSpendTower;
    [SerializeField] private List<Sprite> oreToSpendTowerImageList = new List<Sprite>();
    [SerializeField] private int oreImageNum;
    [SerializeField] private Image oreToSpendTowerImage;
    [SerializeField] private TextMeshProUGUI oreCostText;
    [SerializeField] private int UnlockCost;
    [SerializeField] private string towerUnlock;
    [SerializeField] private List<GameObject> towerNodes = new List<GameObject>();

    [Header("Reclaim Tower")]
    [SerializeField] private GameObject destroyedState;
    [SerializeField] private GameObject fixedState;
    [SerializeField] private bool isReclaimed = false;
    [SerializeField] private bool playerInside = false;

    private void Start()
    {
        upgradeText.SetActive(false);
        insufficientOreUI.SetActive(false);

        // Reset to Destroyed
        destroyedState.SetActive(true);
        fixedState.SetActive(false);

        LockNodes();
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F) && !isReclaimed)
        {
            // UnlockTowers(towerUnlock, oreToSpendTower, towerUnlockCost);

            UnlockTowerSlots(oreToSpendTower, UnlockCost);
        }
    }

    #region UnlockThings
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

                if (_promptWarnings != null)
                {
                    _promptWarnings.SetPromptTextDisplay("Not enough " + oreTypeToSpend + " to unlock " + card.towerName);
                }

                Debug.Log("Not enough " + oreTypeToSpend + " to unlock " + card.towerName);
            }
            break;
        }
    }

    public void UnlockTowerSlots(upgradeResourceType oreTypeToSpend, int customCost)
    {
        int playerAmount = gold.GetResourceType(oreTypeToSpend);

        if (playerAmount >= customCost)
        {
            gold.SpendingToResourceType(oreTypeToSpend, customCost);

            foreach (GameObject node in towerNodes)
            {
                node.SetActive(true);
            }

            ReclaimTower();

            Debug.Log("Unlocked tower nodes! Using " + customCost + " of " + oreTypeToSpend);

            upgradeText.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowError());

            if (_promptWarnings != null)
            {
                _promptWarnings.SetPromptTextDisplay("Not enough " + oreTypeToSpend + " to unlock tower nodes!");
            }
        }
    }

    public void LockNodes()
    {
        foreach(GameObject node in towerNodes)
        {
            node.SetActive(false);
        }
    }

    #endregion
    private void ChangeTexture()
    {
        destroyedState.SetActive(false);
        fixedState.SetActive(true);
    }

    private void ReclaimTower()
    {
        _reclaimParticles.Play();

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

    public void OreUIImage()
    {
        int i = oreImageNum;

        if (i == 0)
        {
            oreToSpendTowerImage.sprite = oreToSpendTowerImageList[0];
        }
        else if (i == 1)
        {
            oreToSpendTowerImage.sprite = oreToSpendTowerImageList[1];
        }
        else if (i == 2)
        {
            oreToSpendTowerImage.sprite = oreToSpendTowerImageList[2];
        }
        else if (i == 3)
        {
            oreToSpendTowerImage.sprite = oreToSpendTowerImageList[3];
        }

        oreCostText.text = "x " + UnlockCost.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && !isReclaimed)
        {
            playerInside = true;

            OreUIImage();
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
