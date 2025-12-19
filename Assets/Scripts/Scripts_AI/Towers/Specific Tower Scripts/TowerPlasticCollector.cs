using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class IntEvent : UnityEvent<int> { }

public class TowerPlasticCollector : TowerUtilityBase
{
    [Header("Events")]
    public UnityEvent EvtOnGarbageObjectCollide;
    public UnityEvent<GameObject> EvtPassObjectReference;
    public IntEvent EvtOnAddMoney;

    [Header("Damage Settings")]
    public int TowerDamageLevel = 1;
    public int StartingDamageValue = 20;
    public int DamageIncreasePerLevel = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GarbageObject>())
        {
            EvtOnGarbageObjectCollide?.Invoke();
            EvtPassObjectReference?.Invoke(other.gameObject);

            int moneyToAdd = CalculateMoneyValue();
            EvtOnAddMoney?.Invoke(moneyToAdd);
        }
    }

    public void _IncreaseTowerDamageLevel()
    {
        TowerDamageLevel++;
        int newValue = CalculateMoneyValue();
        Debug.Log($"[TowerPlasticCollector] TowerDamageLevel increased to {TowerDamageLevel}. New money value: {newValue}");
    }

    public void _DisableGarbageObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public int CalculateMoneyValue()
    {
        return StartingDamageValue + (TowerDamageLevel - 1) * DamageIncreasePerLevel;
    }
}