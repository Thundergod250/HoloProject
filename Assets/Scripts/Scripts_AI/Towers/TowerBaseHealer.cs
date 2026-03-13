using TMPro;
using UnityEngine;

public class TowerBaseHealer : MonoBehaviour
{
    [SerializeField] private DropResourceManager _dropResourceReference;
    [SerializeField] private GameObject _gameUIObject;

    [SerializeField] UI_PromtWarnings _promtWarnings;

    [SerializeField] private TextMeshProUGUI _costDisplayTextCop;
    [SerializeField] private TextMeshProUGUI _costDisplayTextIron;
    [SerializeField] private TextMeshProUGUI _costDisplayTextGold;

    [SerializeField] private Health _baseHealth;

    [SerializeField] protected int _copperHealNeed = 10;
    [SerializeField] protected int _ironHealNeed = 5;
    [SerializeField] protected int _goldHealNeed = 2;

    private void Start()
    {
        _dropResourceReference = GameManager.Instance.DropManager;
    }

    private void Update()
    {
        // If the UI is open, keep the text updated and check for input
        if (_gameUIObject.activeSelf)
        {
            UpdateUI();

            if (Input.GetKeyDown(KeyCode.F))
            {
                ChoiceHealing();
                
            }
        }
    }

    // --- COMPUTATION BLOCK (For UI) ---
    private float GetHealthRatio()
    {
        if (_baseHealth == null) return 0;
        float missing = _baseHealth.GetMaxHealth() - _baseHealth.GetCurrentHealth();
        return Mathf.Clamp01(missing / _baseHealth.GetMaxHealth());
    }

    private void UpdateUI()
    {
        float ratio = GetHealthRatio();

        //if (ratio <= 0)
        //{
        //    //_costDisplayText.text = "Health is Full";
        //    return;
        //}

        // Calculate costs for display
        int copper = Mathf.CeilToInt(ratio * _copperHealNeed);
        int iron = Mathf.CeilToInt(ratio * _ironHealNeed);
        int gold = Mathf.CeilToInt(ratio * _goldHealNeed);

        // Show the player what the current "Best Option" is
        if (_dropResourceReference.CopperHold >= copper)
            _costDisplayTextCop.text = $"Repair Cost: {copper} Copper";
        else if (_dropResourceReference.IronHold >= iron)
            _costDisplayTextIron.text = $"Repair Cost: {iron} Iron";
        else if (_dropResourceReference.GoldHold >= gold)
            _costDisplayTextGold.text = $"Repair Cost: {gold} Gold";
        //else
            //_costDisplayText.text = "<color=red>Not Enough Resources</color>";
    }

    // --- LOGIC BLOCK (For Action) ---
    private void ChoiceHealing()
    {
        if (_dropResourceReference == null || _baseHealth == null) return;

        float ratio = GetHealthRatio();
        if (ratio <= 0) return;

        int currentCopperCost = Mathf.CeilToInt(ratio * _copperHealNeed);
        int currentIronCost = Mathf.CeilToInt(ratio * _ironHealNeed);
        int currentGoldCost = Mathf.CeilToInt(ratio * _goldHealNeed);

        // Priority Check (Back to your original logic style)
        if (_dropResourceReference.CopperHold >= currentCopperCost)
        {
            ApplyHeal(currentCopperCost, "Copper");
        }
        else if (_dropResourceReference.IronHold >= currentIronCost)
        {
            ApplyHeal(currentIronCost, "Iron");
        }
        else if (_dropResourceReference.GoldHold >= currentGoldCost)
        {
            ApplyHeal(currentGoldCost, "Gold");
        }

        if (_promtWarnings != null)
        {
            _promtWarnings.SetPromptTextDisplay("Base Fully Repaired");
        }
    }

    private void ApplyHeal(int cost, string resourceType)
    {
        // Since resource variables are read-only, 
        // you should call a "Spend" method on your manager here.
        // Example: _dropResourceReference.Spend(resourceType, cost);

        _baseHealth.Heal(_baseHealth.GetMaxHealth());
        Debug.Log($"Healed using {cost} {resourceType}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()) _gameUIObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>()) _gameUIObject.SetActive(false);
    }
}
