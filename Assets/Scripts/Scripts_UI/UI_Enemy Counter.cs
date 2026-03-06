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
        Debug.Log(totalEnemies);
        enemyCounterUI.text = "Enemies Left: " + totalEnemies.ToString();
    }

    public void Update()
    {
        if(ligthingManager._isNight)
        {
            enemyCounterUI.enabled = true;
            //UpdateEnemyCounter();
        }
        else if(!ligthingManager._isNight)
        {
            enemyCounterUI.enabled = false;
        }
    }
}
