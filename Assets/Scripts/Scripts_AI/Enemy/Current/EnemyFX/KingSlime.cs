using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class KingSlime : Effects_Enemy
{
    [Header("Refs")]
    [SerializeField] private Attack_Enemy attack_Enemy;
    [SerializeField] private Navigation_Enemy nav_Enemy;
    [SerializeField] private Health health;
    [SerializeField] private KingSlimeAggroRange ksaggroRange;

    [Header("Vars")]
    [SerializeField] private List<Transform> secondPath = new List<Transform>();
    [SerializeField] private List<Transform> thirdPath = new List<Transform>();
    [SerializeField] private List<Spawner> spawners = new List<Spawner>();
    [SerializeField] private int dsiableTowerDuration;
    [SerializeField] private GameObject invisSmoke;

    [Header("WaveData Override")]
    [SerializeField] public List<WaveData> waveOveridde = new List<WaveData>();

    private bool switchToSecond;
    private bool switchToThird;

    private void Start()
    {
        invisSmoke.SetActive(false);

        switchToSecond = false;
        switchToThird = false;

        ksaggroRange.debuffTimer = dsiableTowerDuration;
    }

    public void HealthThresholdChangeLane()
    {
        float currentHP = health.GetCurrentHealth();

        if (currentHP <= 350 && !switchToSecond) // if at or below 350
        {
            SetWaypoints(secondPath);
            switchToSecond = true;
        }

        if (currentHP <= 175 && switchToSecond && !switchToThird) // if at or below 175
        {
            SetWaypoints(thirdPath);
            switchToThird = true;
        }
    }

    public void InvisHpThreshold()
    {
        float currentHP = health.GetCurrentHealth();

        if (currentHP <= 350 && !switchToSecond) // if at or below 350
        {
            invisSmoke.SetActive(true);
        }
    }

    public void SetWaypoints(List<Transform> newPath)
    {
        nav_Enemy.wayPoints.Clear();
        nav_Enemy.wayPoints.AddRange(newPath);

        nav_Enemy.ChangeTarget();

        var agent = nav_Enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = false;
        transform.position = newPath[0].transform.position; //teleport
        agent.enabled = true;

        agent.Warp(transform.position);
    }

    public void ForwardMyArmy()
    { 
        float currentHP = health.GetCurrentHealth();

        if (currentHP <= 250)
        {
            foreach(Spawner p in spawners)
            {
                p.ForceOverrideWave(waveOveridde);
            }
        }
    }
}
