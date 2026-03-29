using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class TowerBaseHealer : MonoBehaviour
{
    [SerializeField] private DropResourceManager _dropResourceReference;
    [SerializeField] private GameObject _gameTouchUIObject;
    [SerializeField] private GameObject _gameHUDUIObject;
    [SerializeField] public bool _enableTouchUI = false;

    [SerializeField] UI_PromtWarnings _promtWarnings;

    [SerializeField] private TextMeshProUGUI[] _costDisplayTextCop;
    [SerializeField] private TextMeshProUGUI[] _costDisplayTextIron;
    [SerializeField] private TextMeshProUGUI[] _costDisplayTextGold;

    [SerializeField] private Health _baseHealth;

    [SerializeField] protected int _copperHealNeed = 10;
    [SerializeField] protected int _ironHealNeed = 5;
    [SerializeField] protected int _goldHealNeed = 2;

    [SerializeField] private List<ParticleSystem> _smokeVFXObjects;

    private void Start()
    {
        _dropResourceReference = GameManager.Instance.DropManager;

        StopAllVFXSmoke();
    }

    private void Update()
    {
        // If the UI is open, keep the text updated and check for input
        if (_gameTouchUIObject.activeSelf)
        {
            UpdateUI();

            if (Input.GetKeyDown(KeyCode.F))
            {
                ChoiceHealing();
            }
        }

        if (_smokeVFXObjects != null)
        {
            CheckHPForSmokeVFX();
        }

    }

    private void StopAllVFXSmoke()
    {
        for (int i = 0; i< _smokeVFXObjects.Count; i++)
        {
            _smokeVFXObjects[i].Stop();
        }
    }


    // --- COMPUTATION BLOCK (For UI) ---
    private float GetHealthRatio()
    {
        if (_baseHealth == null) return 0;
        float missing = _baseHealth.GetMaxHealth() - _baseHealth.GetCurrentHealth();
        return Mathf.Clamp01(missing / _baseHealth.GetMaxHealth());
    }

    private void CheckHPForSmokeVFX()
    {
        if (_baseHealth != null)
        {
            float currentHP = _baseHealth.GetCurrentHealth();
            float maxHP = _baseHealth.GetMaxHealth(); // Assuming this exists

            // 1. Calculate health percentage (0.0 to 1.0)
            float healthPercent = currentHP / maxHP;

            // 2. Calculate how many smoke objects should be active
            // As health goes DOWN, activeCount goes UP.
            // Example: 50% health (0.5) with 10 objects -> (1 - 0.5) * 10 = 5 objects.
            int activeCount = Mathf.FloorToInt((1f - healthPercent) * _smokeVFXObjects.Count);

            // 3. Update the VFX states
            for (int i = 0; i < _smokeVFXObjects.Count; i++)
            {
                if (i < activeCount)
                {
                    // If it's already playing, VFX Graph handles that gracefully
                    _smokeVFXObjects[i].Play();
                }
                else
                {
                    _smokeVFXObjects[i].Stop();
                }
            }
        }
    }


    private void UpdateUI()
    {
        float ratio = GetHealthRatio();

        // Calculate costs for display
        int copper = Mathf.CeilToInt(ratio * _copperHealNeed);
        int iron = Mathf.CeilToInt(ratio * _ironHealNeed);
        int gold = Mathf.CeilToInt(ratio * _goldHealNeed);

        for (int i = 0; i < 4; i++)
        {
            _costDisplayTextCop[i].text = $"{copper}" + "\n" + " Copper";

            _costDisplayTextIron[i].text = $"{iron}" + "\n" + " Iron";

            _costDisplayTextGold[i].text = $"{gold}" + "\n" + " Gold";
        }
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
            ApplyHeal(currentCopperCost, upgradeResourceType.Copper); // "Copper");
            _promtWarnings.SetPromptTextDisplay("Base Fully Repaired Copper -" + currentCopperCost);
        }
        else if (_dropResourceReference.IronHold >= currentIronCost)
        {
            ApplyHeal(currentIronCost, upgradeResourceType.Iron);//"Iron");
            _promtWarnings.SetPromptTextDisplay("Base Fully Repaired Iron -" + currentIronCost);
        }
        else if (_dropResourceReference.GoldHold >= currentGoldCost)
        {
            ApplyHeal(currentGoldCost, upgradeResourceType.Gold);//"Gold");
            _promtWarnings.SetPromptTextDisplay("Base Fully Repaired Gold -" + currentGoldCost);
        }
        else
        {
            _promtWarnings.SetPromptTextDisplay("Cannot repair Base not enough resources ");
        }
    }

    private void ApplyHeal(int cost, upgradeResourceType resourceType)
    {
        // Since resource variables are read-only, 
        // you should call a "Spend" method on your manager here.
        // Example: _dropResourceReference.Spend(resourceType, cost);
        _dropResourceReference.SpendingToResourceType(resourceType, cost);
        _baseHealth.Heal(_baseHealth.GetMaxHealth());
        Debug.Log($"Healed using {cost} {resourceType}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()) 
        {
            if (_enableTouchUI)
            {
                _gameTouchUIObject.SetActive(true);
                _gameHUDUIObject.SetActive(false);
            }
            else if (!_enableTouchUI)
            {
                _gameHUDUIObject.SetActive(true);
                _gameTouchUIObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _gameTouchUIObject.SetActive(false);
            _gameHUDUIObject.SetActive(false);
        }
    }
}
