using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInfo
{
    public string towerName;
    public string description;
    public Sprite towerIcon;
    public upgradeResourceType neededUpgradeResourceType;
    public int oreCost;
    public Sprite oreIcon;
    public GameObject towerPrefab;
    public GameObject towerCardPrefab;
}

[CreateAssetMenu(fileName = "TowerCategoryData", menuName = "TowerShop/Tower Category")]
public class TowerCategoryData_SO : ScriptableObject
{
    public List<CardInfo> cards;
}