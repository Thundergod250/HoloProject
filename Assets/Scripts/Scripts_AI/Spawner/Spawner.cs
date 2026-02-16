using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private LightingManager lightingManager;
    [SerializeField] private UI_Caution cautionUI;
    [SerializeField] private UI_EnemyCounter enemyCounterUI;

    [Header("Spawner")]
    [SerializeField] private WaveData[] wave;

    [Header("Variables DO NOT TOUCH")]
    [SerializeField] private int waveVar;
    [SerializeField] private bool spawningWave = true;
    [SerializeField] private bool isNighttime;
    [SerializeField] private WaveData activeWave;
    private bool waveInProgress;

    [Header("Var Safe to Adjust")]
    [SerializeField] private float timeAfterWave = 10f;
    [SerializeField] private bool testingMode;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();

    [Header("Public Vars")]
    public int enemyCounter;

    private Coroutine spawnRoutine;
    private bool wasNightLastFrame;

    private void Start()
    {
        waveVar = 0;
        activeWave = wave[waveVar];

        if (testingMode)
        {
            spawnRoutine = StartCoroutine(spawnEnemy(activeWave.timeBetweenSpawn));
        }
    }

    private void Update()
    {
        bool isNight = lightingManager._isNight;

        // Night just started → start wave if possible
        if (isNight && !wasNightLastFrame)
        {
            TryStartWave();
        }

        // Night just ended → turn off caution UI
        if (!isNight && wasNightLastFrame)
        {
            if (cautionUI.hasWaveStart)
            {
                cautionUI.hasWaveStart = false; // hide UI
                Debug.Log("Night ended: Caution image turned off");
            }
        }

        wasNightLastFrame = isNight;
    }
    private IEnumerator spawnEnemy(float interval)
    {
        for (int i = 0; i < activeWave.EnemiesInWaves.Length; i++)
        {
            GameObject newEnemy = Instantiate(activeWave.EnemiesInWaves[i], transform.position, Quaternion.identity);

            newEnemy.GetComponent<Navigation_Enemy>().wayPoints = wayPoints;
            newEnemy.GetComponent<Drops_Enemy>().parentSpawner = this;

            enemyCounter++;

            enemyCounterUI.totalEnemies ++;
            enemyCounterUI.UpdateEnemyCounter();

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(timeAfterWave);

        OnWaveFinishedSpawning();
    }

    void OnWaveFinishedSpawning()
    {
        waveInProgress = false;

        if (cautionUI.hasWaveStart)
            cautionUI.hasWaveStart = false; // turn off UI when wave finishes

        GoToNextWave();
    }

    private void GoToNextWave()
    {
        if (waveVar == wave.Length - 1) // last wave
        {
            spawningWave = false;
            return;
        }

        waveVar++;
        activeWave = wave[waveVar];

        // Only start next wave if it's night
        if (lightingManager._isNight)
            TryStartWave();
    }

    public void TryStartWave()
    {
        if (!spawningWave || waveInProgress)
            return;

        if (!lightingManager._isNight) // safety check
            return;

        if (!cautionUI.hasWaveStart)
            cautionUI.hasWaveStart = true; // turn on UI
        else
            return; // already showing

        waveInProgress = true;
        StartCoroutine(spawnEnemy(activeWave.timeBetweenSpawn));
    }

    public void SpawnEnemy(GameObject DebugEnemies)
    {
        GameObject newEnemy = Instantiate(DebugEnemies, transform.position, Quaternion.identity);

        newEnemy.GetComponent<Navigation_Enemy>().wayPoints = wayPoints;
    }


    public void UpdateEnemyCounterText()
    {
        enemyCounterUI.totalEnemies--;
        enemyCounterUI.UpdateEnemyCounter();
    }
}
