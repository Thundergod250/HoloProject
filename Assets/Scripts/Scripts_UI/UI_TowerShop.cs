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
    private TowerOffensiveBase _lastSelectedTowerOffsensiveBase;

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
        if (controller == null)
        {
            Debug.LogError("Received a null TowerController!");
            return;
        }

        // Enable the panels
        _LeftPanel.SetActive(true);
        _RightPanel.SetActive(true);
        _statusPanelUI.SetActive(true);

        // Save the base reference for the Refund button
        _lastSelectedTowerOffsensiveBase = controller.GetComponent<TowerOffensiveBase>();

        // Update Health Text
        if (controller.TowerHealth != null)
        {
            healthText.text = controller.TowerHealth.GetCurrentHealth().ToString();
        }
        else
        {
            healthText.text = "Health Script Missing";
            Debug.LogWarning($"Controller {controller.name} has no TowerHealth reference!");
        }
    }

    public void OnClickRefund() // Must be public!
    {
        if (_lastSelectedTowerOffsensiveBase == null) return;

        // Grab the node from the tower we saved when we clicked it
        TowerNodeManager node = _lastSelectedTowerOffsensiveBase.GetComponentInParent<TowerNodeManager>();

        if (node != null)
        {
            node.DespawnTower(); // Runs your existing despawn logic

            // Close the UI panels
            _LeftPanel.SetActive(false);
            _RightPanel.SetActive(false);
            _statusPanelUI.SetActive(false);

            // Clear the reference for safety
            _lastSelectedTowerOffsensiveBase = null;
        }
    }

    // === Card spawning ===
    private void SpawnCards(List<CardInfo> cards)
    {
        foreach (var cardInfo in cards)
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
