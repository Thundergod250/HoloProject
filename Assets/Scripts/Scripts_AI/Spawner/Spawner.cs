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
    public List<WaveData> wave = new List<WaveData>();
    public List<WaveData> waveSet2 = new List<WaveData>();
    public List<WaveData> waveSet3 = new List<WaveData>();
    public List<WaveData> waveSet4 = new List<WaveData>();

    [Header("Variables DO NOT TOUCH")]
    [SerializeField] private int waveVar;
    [SerializeField] private bool spawningWave = true;
    [SerializeField] private bool isNighttime;
    [SerializeField] private WaveData activeWave;
    public List<GameObject> activeEnemies = new List<GameObject>();
    private bool waveInProgress;

    [Header("Var Safe to Adjust")]
    [SerializeField] private bool testingMode;

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
        bool isNight = lightingManager._isNight;

        // Night just started → start wave if possible
        if (isNight && !wasNightLastFrame)
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
            GameObject newEnemy = Instantiate(activeWave.EnemiesInWaves[i], transform.position, Quaternion.identity);

            newEnemy.GetComponent<Navigation_Enemy>().wayPoints = wayPoints;
            newEnemy.GetComponent<Drops_Enemy>().parentSpawner = this;
            newEnemy.GetComponent<Navigation_Enemy>().lightingManager = lightingManager;

            enemyCounterUI.totalEnemies++;
            enemyCounterUI.UpdateEnemyCounter();

            activeEnemies.Add(newEnemy);

            yield return new WaitForSeconds(interval);
        }

       if(!testingMode) OnWaveFinishedSpawning();
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

    public int GetWaveNumber()
    {
        return waveVar;
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
    }

    public void RemoveTowerFromList(GameObject tower)
    {
        foreach(GameObject e in activeEnemies)
        {
            e.GetComponent<Navigation_Enemy>().TargetHasDied(tower);
        }
    }
}
