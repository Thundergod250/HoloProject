using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_TowerShop : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform cardParent;
    [SerializeField] private TowerCategoryData_SO offensiveTowersData;
    [SerializeField] private TowerCategoryData_SO defensiveTowersData;
    [SerializeField] private TowerCategoryData_SO utilityTowersData;

    [SerializeField] private GameObject _LeftPanel;
    [SerializeField] private GameObject _RightPanel;


    [Header("Shop Buttons")]
    [SerializeField] private GameObject towerUpgradesButton;
    [SerializeField] private GameObject offensiveButton;
    [SerializeField] private GameObject statusButton;
    [SerializeField] private GameObject defensiveButton;
    [SerializeField] private GameObject utilityButton;

    [Header("Towers To Lock At Start (String)")]
    [SerializeField] private List<string> offensiveStartLocked;

    [Header("Status Panel References")]
    [SerializeField] private GameObject _statusPanelUI; // The specific Status UI
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private TMPro.TextMeshProUGUI costText;
    [SerializeField] private TMPro.TextMeshProUGUI nameText; 
    [SerializeField] private Image towerIcon;
    [SerializeField] private Image oreIcon;
    private TowerOffensiveBase _lastSelectedTowerOffsensiveBase;
    private TowerCategoryData_SO _lastSelectedTowerData;

    private TowerCategoryData_SO towerUpgradesData;
    private Dictionary<string, GameObject> shopButtons;
    private readonly List<GameObject> activeCards = new();
    private TowerCategoryData_SO currentCategory;

    private void Awake()
    {
        shopButtons = new Dictionary<string, GameObject>
        {
            { "Upgrades", towerUpgradesButton },
            { "Offensive", offensiveButton },
            { "Status", statusButton },
            { "Defensive", defensiveButton },
            { "Utility", utilityButton }
        };

        LockTowersFromInspector(offensiveTowersData, offensiveStartLocked);

        OpenOffensiveTowers();
    }

    public void EnableTowerShopUI()
    {
        _LeftPanel.SetActive(true); 
        _RightPanel.SetActive(true);

        TrySpawnCategory(offensiveTowersData);
    }


    public void SetUpgradeCategoryData(TowerCategoryData_SO data) => towerUpgradesData = data;

    // === Category entry points ===
    public void OpenTowerUpgrades() => TrySpawnCategory(towerUpgradesData);
    public void OpenOffensiveTowers() => TrySpawnCategory(offensiveTowersData);
    public void OpenDefensiveTowers() => TrySpawnCategory(defensiveTowersData);
    public void OpenUtilityTowers() => TrySpawnCategory(utilityTowersData);

    // === Category spawning ===
    private void TrySpawnCategory(TowerCategoryData_SO data)
    {
        if (data == null) return;
        if (currentCategory == data) return;

        ClearCards();
        currentCategory = data;
        SpawnCards(data.cards);
    }

    public void OpenStatusPanel(TowerController controller)
    {
        if (controller == null) return;

        // 1. Setup UI & References
        _LeftPanel.SetActive(true);
        _RightPanel.SetActive(true);
        _statusPanelUI.SetActive(true);
        _lastSelectedTowerOffsensiveBase = controller.GetComponent<TowerOffensiveBase>();

        // 2. Health (Direct)
        if (controller.TowerHealth != null)
        {
            healthText.text = controller.TowerHealth.GetCurrentHealth().ToString();
        }

        // 3. The Checker Logic
        _lastSelectedTowerData = controller.GetTowerData();
        string targetName = controller.GetTowerNameID();

        if (_lastSelectedTowerData != null)
        {
            CardInfo matchingCard = null;

            // Loop through the cards in the SO to find the right one
            foreach (var card in _lastSelectedTowerData.cards)
            {
                if (card.towerName == targetName)
                {
                    matchingCard = card;
                    break;
                }
            }

            // 4. Populate UI if found
            if (matchingCard != null)
            {
                nameText.text = matchingCard.towerName;
                costText.text = matchingCard.oreCost.ToString();

                if (matchingCard.towerIcon != null) towerIcon.sprite = matchingCard.towerIcon;
                if (matchingCard.oreIcon != null) oreIcon.sprite = matchingCard.oreIcon;
            }
            else
            {
                Debug.LogError($"Tower ID '{targetName}' not found in {_lastSelectedTowerData.name}!");
            }
        }
    }

    public void OnClickRefund()
    {
        if (_lastSelectedTowerOffsensiveBase == null || _lastSelectedTowerData == null) return;

        // 1. Get the controller to find the ID
        TowerController controller = _lastSelectedTowerOffsensiveBase.GetComponent<TowerController>();
        if (controller == null) return;

        string targetName = controller.GetTowerNameID();

        // 2. Search the SO for the matching Resource Type and Cost
        CardInfo matchingCard = null;
        foreach (var card in _lastSelectedTowerData.cards)
        {
            if (card.towerName == targetName)
            {
                matchingCard = card;
                break;
            }
        }

        // 3. Process the Refund via DropManager
        TowerNodeManager node = _lastSelectedTowerOffsensiveBase.GetComponentInParent<TowerNodeManager>();
        if (node != null)
        {
            if (matchingCard != null)
            {
                GameManager.Instance.DropManager.AddingToResourceType(
                    matchingCard.neededUpgradeResourceType,
                    matchingCard.oreCost
                );

                Debug.Log($"Refunding {matchingCard.oreCost} {matchingCard.neededUpgradeResourceType} for {targetName}");
            }
            else
            {
                Debug.LogError("Refund failed: Card data not found for " + targetName);
            }

            // 4. Despawn and Cleanup
            node.DespawnTower();

            this.gameObject.SetActive(true);

            _LeftPanel.SetActive(true);
            _RightPanel.SetActive(true);
            _statusPanelUI.SetActive(false);

            _lastSelectedTowerOffsensiveBase = null;
            _lastSelectedTowerData = null;
        }
        else
        {
            Debug.LogWarning("Could not find TowerNodeManager for refund.");
        }
    }

    // === Card spawning ===
    private void SpawnCards(List<CardInfo> cards)
    {
        foreach (CardInfo cardInfo in cards)
        {
            if (cardInfo.towerCardPrefab == null)
            {
                Debug.LogError($"Card prefab missing for {cardInfo.towerName}");
                continue;
            }

            GameObject cardGO = ObjectPooling.Instance.Get(cardInfo.towerCardPrefab, cardParent);
            cardGO.SetActive(true);

            TowerCardManager card = cardGO.GetComponent<TowerCardManager>();
            if (card != null)
            {
                card.ResetCard(cardInfo);
                card.SetSourcePrefab(cardInfo.towerCardPrefab);
            }

            BuyTower buyTower = cardGO.GetComponent<BuyTower>();
            if (buyTower != null)
                buyTower.TowerCardManager = card;

            if(cardInfo.islocked == true)
            {
                card.lockFilter.SetActive(true);
            }
            else if(cardInfo.islocked == false)
            {
                card.lockFilter.SetActive(false);
            }

                activeCards.Add(cardGO);
        }
    }

    // === Clear old cards ===
    public void ClearCards()
    {
        foreach (var card in activeCards)
        {
            TowerCardManager manager = card.GetComponent<TowerCardManager>();
            if (manager != null && manager.GetSourcePrefab() != null)
                ObjectPooling.Instance.Return(manager.GetSourcePrefab(), card);
            else
                Destroy(card);
        }
        activeCards.Clear();
        currentCategory = null;
    }

    // === Lock Cards at Start ===
    private void LockTowersFromInspector(TowerCategoryData_SO data, List<string> namesToLock)
    {
        if (data == null || namesToLock == null) return;

        HashSet<string> lockSet = new HashSet<string>(namesToLock);

        foreach (CardInfo card in data.cards)
        {
            card.islocked = lockSet.Contains(card.towerName);
        }
    }

    // === Button visibility ===
    public void ShowShopButtons(bool showUpgrades)
    {
        // shopButtons["Upgrades"].SetActive(showUpgrades);
        shopButtons["Offensive"].SetActive(true);
        shopButtons["Status"].SetActive(true);
        //shopButtons["Defensive"].SetActive(true);
        //shopButtons["Utility"].SetActive(true);
    }
}
