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
        // enemyCounterUI.text = "Enemies Left: " + totalEnemies.ToString();
        enemyCounterUI.text =  totalEnemies.ToString();
    }

    public void Update()
    {
        if(ligthingManager._isNight)
        {
            enemyCounterUI.enabled = true;
        }
        else if(!ligthingManager._isNight) // if morning
        {
            enemyCounterUI.enabled = false;
        }
    }
}
