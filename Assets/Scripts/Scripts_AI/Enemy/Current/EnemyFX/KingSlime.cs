using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KingSlime : Effects_Enemy
{
    [Header("Refs")]
    [SerializeField] private Attack_Enemy attack_Enemy;
    [SerializeField] private Navigation_Enemy nav_Enemy;
    [SerializeField] private Health health;

    [Header("Vars")]
    [SerializeField] private List<Transform> secondPath = new List<Transform>();
    [SerializeField] private List<Transform> thirdPath = new List<Transform>();
    [SerializeField] private List<Spawner> spawners = new List<Spawner>();

    [Header("WaveData Override")]
    [SerializeField] public List<WaveData> waveOveridde = new List<WaveData>();


    private bool switchToSecond;
    private bool switchToThird;

    private void Start()
    {
        switchToSecond = false;
        switchToThird = false;
    }

    public void HealthThresholdChangeLane()
    {
        float currentHP = health.GetCurrentHealth();

        if (currentHP <= 65 && !switchToSecond) // if at below 66, so 65
        {
            SetWaypoints(secondPath);
            switchToSecond = true;
        }

        if (currentHP <= 33 && switchToSecond && !switchToThird) // if at below 31, so 30
        {
            SetWaypoints(thirdPath);
            switchToThird = true;
        }
    }

    public void SetWaypoints(List<Transform> newPath)
    {
        nav_Enemy.wayPoints.Clear();
        nav_Enemy.wayPoints.AddRange(newPath);

        var agent = nav_Enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = false;
        transform.position = newPath[0].transform.position; //teleport
        agent.enabled = true;

        agent.Warp(transform.position);
    }

    public void ForwardMyArmy()
    { 
        float currentHP = health.GetCurrentHealth();

        if (currentHP <= 50)
        {
            foreach(Spawner p in spawners)
            {
                p.ForceOverrideWave(waveOveridde);
            }
        }
    }
}
