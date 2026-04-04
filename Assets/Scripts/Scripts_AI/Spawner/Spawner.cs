using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private LightingManager lightingManager;
    [SerializeField] private UI_Caution cautionUI;
    [SerializeField] private UI_EnemyCounter enemyCounterUI;
    [SerializeField] private DropResourceManager dropResourceManager;

    [Header("Spawner")]
    public List<WaveData> wave = new List<WaveData>();
    public List<WaveData> waveSet2 = new List<WaveData>();
    public List<WaveData> waveSet3 = new List<WaveData>();
    public List<WaveData> waveSet4 = new List<WaveData>();

    [Header("Variables DO NOT TOUCH")]
    [SerializeField] private int waveVar;
    [SerializeField] private bool isNighttime;
    [SerializeField] private WaveData activeWave;
    public List<GameObject> activeEnemies = new List<GameObject>();
    private bool waveInProgress;

    [Header("Var Safe to Adjust")]
    [SerializeField] private bool testingMode;
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private bool activeSpawner;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();

    private Coroutine spawnRoutine;
    private bool wasNightLastFrame;

    private void Start()
    {
        waveVar = 0;
        activeWave = wave[waveVar];

        if (testingMode)
        {
            StartCoroutine(spawnEnemy(activeWave.timeBetweenSpawn));
        }
    }

    private void Update()
    {
        if(activeSpawner)
        {
            cautionUI.parentSpawnerReady = true;
        }
        else
        {
            cautionUI.parentSpawnerReady = false;
        }

        bool isNight = lightingManager._isNight;

        // Night just started → start wave if possible
        if (isNight && !wasNightLastFrame && activeSpawner)
        {
            TryStartWave();
        }

        // Night just ended → cleanup
        if (!isNight && wasNightLastFrame)
        {
            if (cautionUI.hasWaveStart)
            {
                cautionUI.hasWaveStart = false;
                Debug.Log("Night ended: Caution image turned off");
            }
        }

        wasNightLastFrame = isNight;
    }
    private IEnumerator spawnEnemy(float interval)
    {
        for (int i = 0; i < activeWave.EnemiesInWaves.Length; i++)
        {
            GameObject newEnemy = Instantiate(activeWave.EnemiesInWaves[i], spawnpoint.transform.position, Quaternion.identity);

            newEnemy.GetComponent<Navigation_Enemy>().wayPoints = wayPoints;
            newEnemy.GetComponent<Drops_Enemy>().parentSpawner = this;
            newEnemy.GetComponent<Navigation_Enemy>().lightingManager = lightingManager;
            newEnemy.GetComponent<Drops_Enemy>()._dropResourceManager = dropResourceManager;

            enemyCounterUI.totalEnemies++;
            enemyCounterUI.UpdateEnemyCounter();

            activeEnemies.Add(newEnemy);

            yield return new WaitForSeconds(interval);
        }

       if(!testingMode) OnWaveFinishedSpawning();
    }

    public void ForceOverrideWave(List<WaveData> newWave)
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        // Replace wave data
        wave.Clear();
        wave.AddRange(newWave);

        waveVar = 0;
        activeWave = wave[waveVar];

        // Reset state properly
        waveInProgress = true;

        if (!cautionUI.hasWaveStart)
            cautionUI.hasWaveStart = true;

        // Start new spawning and TRACK it
        spawnRoutine = StartCoroutine(spawnEnemy(activeWave.timeBetweenSpawn));
    }

    void OnWaveFinishedSpawning()
    {
        waveInProgress = false;

        if (cautionUI.hasWaveStart)
            cautionUI.hasWaveStart = false; // turn off UI when wave finishes

        SetupNextWave();
    }

    private void SetupNextWave()
    {
        if (waveVar < wave.Count - 1)
        {
            waveVar++;
        }

        // Always set active wave (even if last)
        activeWave = wave[waveVar];
    }

    public void TryStartWave()
    {
        Debug.Log(this.name + " spawning enemies");

        if (waveInProgress)
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

    public void SpawnEnemy(GameObject DebugEnemies) //DebugMode Probably Buggy
    {
        GameObject newEnemy = Instantiate(DebugEnemies, spawnpoint.transform.position, Quaternion.identity);

        newEnemy.GetComponent<Navigation_Enemy>().wayPoints = wayPoints;
    }

    public void UpdateEnemyCounterText()
    {
        enemyCounterUI.totalEnemies--;
        enemyCounterUI.UpdateEnemyCounter();
    }

    public void enableSpawner(bool state)
    {
        activeSpawner = state;
    }

    public void copySpawnerWaveInfo(Spawner spawnerRef)
    {
        waveVar = spawnerRef.waveVar;
    }

    public void GetNewSetOfWaves(int waveSet)
    {
        wave.Clear();
        if (waveSet == 1)
        {
            wave.AddRange(waveSet2);
        }
        else if (waveSet == 2)
        {
            wave.AddRange(waveSet3);
        }
        else if (waveSet == 3)
        {
            wave.AddRange(waveSet4);
        }

        waveVar = 0;
    }

    public void RemoveTowerFromList(GameObject tower)
    {
        foreach(GameObject e in activeEnemies)
        {
            e.GetComponent<Navigation_Enemy>().TargetHasDied(tower);
        }
    }
}
