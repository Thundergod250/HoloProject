using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInfo
{
    public string title;
    public string description;
    public int cost;
    public Sprite icon;
    public GameObject towerPrefab;
}

[CreateAssetMenu(fileName = "TowerCategoryData", menuName = "TowerShop/Tower Category")]
public class TowerCategoryData_SO : ScriptableObject
{
    public string categoryName;
    public GameObject towerCardPrefab;
    public List<CardInfo> cards;
}