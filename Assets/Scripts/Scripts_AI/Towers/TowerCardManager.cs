using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCardManager : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cost;
    public Button Button;
    public GameObject TowerPrefab; 
    
    public int GetCostValue()
    {
        return int.TryParse(Cost.text, out int value) ? value : 0;
    }
}