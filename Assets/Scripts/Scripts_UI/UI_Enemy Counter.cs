using TMPro;
using UnityEngine;

public class UI_EnemyCounter : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Spawner[] spawners;

    [Header("Vars")]
    [SerializeField] private TextMeshProUGUI enemyCounterUI;
    public int totalEnemies;

    public void UpdateEnemyCounter()
    {
        enemyCounterUI.text = "Enemies Remaining: " + totalEnemies;
    }
}
