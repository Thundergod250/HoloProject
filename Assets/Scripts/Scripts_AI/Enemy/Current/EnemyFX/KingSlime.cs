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
    [SerializeField] private AggroRange_Enemy genaggroRange;
    [SerializeField] private BoxCollider ksCollider;

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
    public bool isInvis;
    public bool isWarded;
    public bool triggeredStealth;
    private bool triggeredWard = false;

    private void Start()
    {
        invisSmoke.SetActive(false);
        isInvis = false;
        isWarded = false;
        triggeredStealth = false;

        switchToSecond = false;
        switchToThird = false;

        ksaggroRange.debuffTimer = dsiableTowerDuration;
    }

    private void Update()
    {
        if(!isWarded)
        {
            OuNoKage();
        }
        else if (isWarded && !triggeredWard)
        {
            StartCoroutine(ShineKage());
        }
    }

    public void OuNoKage()
    {
        float currentHP = health.GetCurrentHealth();

        if (currentHP <= 500 && !isWarded) // if at or below 500 hp and if there is no ward nearby
        {
            KageNoChikara();
        }
    }

    public void KageNoChikara()
    {
        invisSmoke.SetActive(true);
        ksCollider.enabled = false;
        ksaggroRange.enabled = false;
        genaggroRange.enabled = false;
    }

    public IEnumerator ShineKage()
    {
        yield return new WaitForSeconds(2.5f);

        triggeredWard = true;

        invisSmoke.SetActive(false);
        ksCollider.enabled = true;
        ksaggroRange.enabled = true;
        genaggroRange.enabled = true;

        // KageNoChikara();
    }

    public void HalfHpGoInvis() // GoInvisOnceAtHalfHP
    {
        float currentHP = health.GetCurrentHealth();

        if (currentHP <= 500 && !triggeredStealth)
        {
            FocusBase();
            triggeredStealth = true;
        }
    }

    public void FocusBase()
    {
        nav_Enemy.targetsAcquired.Clear();
        nav_Enemy.ResetCurrentTarget();

        if (nav_Enemy.AttackEnemyRef != null)
        {
            nav_Enemy.AttackEnemyRef.target = null; // remove enemy ref, back to straight to base
        }

        nav_Enemy.navigation.isStopped = false;
        nav_Enemy.FindNearestWaypoint();
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
    } // USED

    public void CannotBeTargeted()
    {
        if (isInvis)
        {
            ksCollider.enabled = false;
            ksaggroRange.enabled = false;

        }
        else if (!isInvis)
        {
            ksCollider.enabled = true;
            ksaggroRange.enabled = true;
        }
    }

}
