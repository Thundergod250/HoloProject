using TMPro;
using UnityEngine;

public class UI_EnemyCounter : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private LightingManager ligthingManager;

    [Header("Vars")]
    [SerializeField] private TextMeshProUGUI enemyCounterUI;
    public int totalEnemies;

    private void Start()
    {
        enemyCounterUI.enabled = false;
    }

    public void UpdateEnemyCounter()
    {
        enemyCounterUI.text = "Enemies Remaining: " + totalEnemies;
    }

    public void LateUpdate()
    {
        if(ligthingManager._isNight)
        {
            enemyCounterUI.enabled = true;
        }
        else if(!ligthingManager._isNight)
        {
            enemyCounterUI.enabled = false;
        }
    }
}
