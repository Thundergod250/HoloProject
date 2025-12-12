using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerCategoryData", menuName = "TowerShop/Tower Category")]
public class TowerCategoryData_SO : ScriptableObject
{
    public string categoryName;
    public GameObject towerCardPrefab;
    public List<TowerCardInfo> cards;
}

[System.Serializable]
public class TowerCardInfo
{
    public string title;
    public string description;
    public int cost;
    public Sprite icon;
    public GameObject towerPrefab; 
}