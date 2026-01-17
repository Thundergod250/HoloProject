using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private WaveData[] wave;

    [Header("Variables")]
    [SerializeField] private int spawnerInterval;
    [SerializeField] private int waveVar;
    [SerializeField] private bool spawningWave = true;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private WaveData currentWave;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();

    private void Start()
    {
        waveVar = 0;
        currentWave = wave[waveVar];
        StartWave();
    }

    private IEnumerator spawnEnemy(float interval)
    {
        for (int i = 0; i < currentWave.EnemiesInWaves.Length; i++)
        {
            GameObject newEnemy = Instantiate(currentWave.EnemiesInWaves[i], transform.position, Quaternion.identity);

            newEnemy.GetComponent<Navigation_Enemy>().wayPoints = wayPoints;

            yield return new WaitForSeconds(interval);
        }

        OnWaveFinishedSpawning();
    }

    void OnWaveFinishedSpawning()
    {
        GoToNextWave();
    }

    private void GoToNextWave()
    {
        if (waveVar == wave.Length - 1)
        {
            spawningWave = false;
            return;
        }

        waveVar++;
        currentWave = wave[waveVar];
        StartWave();
    }

    public void StartWave()
    {
        if (!spawningWave)
            return;

        StartCoroutine(spawnEnemy(spawnerInterval));
    }
}
