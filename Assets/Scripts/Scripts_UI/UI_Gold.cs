using System;
using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GoldManager goldManager;

    //To change later
    private void Update()
    {
        goldText.text = "Gold: " + goldManager.playerGold; 
    }
}

