using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private LightingManager lightingManager;

    [Header("Spawner")]
    [SerializeField] private WaveData[] wave;

    [Header("Variables")]
    [SerializeField] private int waveVar;
    [SerializeField] private bool spawningWave = true;
    [SerializeField] private bool isNighttime;
    [SerializeField] private WaveData activeWave;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();

    private void Start()
    {
        waveVar = 0;
        activeWave = wave[waveVar];
        if (lightingManager._isNight)
        {
            StartWave();
        }
        else
        {
            // Optional: start a coroutine that waits until night begins
            StartCoroutine(WaitForNight());
        }
    }

    private IEnumerator spawnEnemy(float interval)
    {
        for (int i = 0; i < activeWave.EnemiesInWaves.Length; i++)
        {
            GameObject newEnemy = Instantiate(activeWave.EnemiesInWaves[i], transform.position, Quaternion.identity);

            newEnemy.GetComponent<Navigation_Enemy>().wayPoints = wayPoints;

            yield return new WaitForSeconds(interval);
        }

        OnWaveFinishedSpawning();
    }

    private IEnumerator WaitForNight()
    {
        while (!lightingManager._isNight)
        {
            yield return null; // wait a frame
        }

        StartWave(); // fire exactly once
    }

    void OnWaveFinishedSpawning()
    {
        GoToNextWave();
    }

    private void GoToNextWave()
    {
        if (waveVar == wave.Length - 1) // if at last wave
        {
            spawningWave = false;
            return;
        }

        waveVar++;
        activeWave = wave[waveVar];
        StartWave();
    }

    public void StartWave()
    {
        if (!spawningWave)
            return;

        if(lightingManager._isNight)
        {
            StartCoroutine(spawnEnemy(activeWave.timeBetweenSpawn));
        }
        else
        {
            return;
        }
    }
}
