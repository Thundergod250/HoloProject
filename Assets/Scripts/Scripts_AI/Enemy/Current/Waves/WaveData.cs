using UnityEngine;
[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 1)]
public class WaveData : ScriptableObject
{
    [field: SerializeField]
    public GameObject[] EnemiesInWaves { get; private set; }

    [field: SerializeField]
    public float timeBetweenSpawn { get; private set; }
}
