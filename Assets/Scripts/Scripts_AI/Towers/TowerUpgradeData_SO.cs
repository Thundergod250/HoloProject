using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeInfo
{
    public string title;
    public string description;
    public int cost;
    public Sprite icon;
    public GameObject towerPrefab;
    public int TowerHealth;
    public int CurrentDamage;
    public int DamageIncrease;
    public int CurrentFireRate;
    public int FireRateIncrease;
    public GameObject towerCardPrefab;
}

public class TowerUpgradeData_SO : MonoBehaviour
{
    public string categoryName;
    public List<UpgradeInfo> cards;
}
