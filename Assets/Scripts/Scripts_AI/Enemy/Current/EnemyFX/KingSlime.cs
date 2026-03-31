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

    [Header("Vars")]
    [SerializeField] private List<Transform> secondPath = new List<Transform>();
    [SerializeField] private List<Transform> thirdPath = new List<Transform>();
    [SerializeField] private List<Spawner> spawners = new List<Spawner>();
    [SerializeField] private int dsiableTowerDuration;

    [Header("WaveData Override")]
    [SerializeField] public List<WaveData> waveOveridde = new List<WaveData>();

    private List<GameObject> towersInRange = new List<GameObject>();
    private bool switchToSecond;
    private bool switchToThird;

    private void Start()
    {
        switchToSecond = false;
        switchToThird = false;

        StartCoroutine(DisableTowerInRange(10, dsiableTowerDuration));
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

        if (currentHP <= 250)
        {
            foreach(Spawner p in spawners)
            {
                p.ForceOverrideWave(waveOveridde);
            }
        }
    }

    private IEnumerator DisableTowerInRange(int duration, int disablePower)
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);

            if (towersInRange.Count == 0)
                continue; 

            int randomIndex = Random.Range(0, towersInRange.Count);

            GameObject towerToDisable = towersInRange[randomIndex];

            if (towerToDisable != null)
            {
                towerToDisable.GetComponent<Tower_Offensive_SingleTarget>().DisableForSeconds(disablePower);
                Debug.Log("Disabled tower: " + towerToDisable.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Tower_Offensive_SingleTarget>() != null)
        {
            if (!towersInRange.Contains(other.gameObject))
            {
                towersInRange.Add(other.gameObject);
                Debug.Log("Tower added: " + other.gameObject.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove tower if it leaves the trigger
        if (other.GetComponent<Tower_Offensive_SingleTarget>() != null)
        {
            towersInRange.Remove(other.gameObject);
            Debug.Log("Tower removed: " + other.gameObject.name);
        }
    }
}
