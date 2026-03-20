using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Navigation_Enemy : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Attack_Enemy attack_Enemy;
    [SerializeField] private TowerAndEnemy_Archetype target_Arch;
    [SerializeField] private Health helth;

    [Header("Colliders")]
    [SerializeField] private SphereCollider sphereCollider;
    private bool meleeTrigger = false;

    [Header("Navigation Target")]
    [SerializeField] GameObject currentTarget;
    [SerializeField] private int wayPointIndex;
    [SerializeField] private float moveSpeed;
    public List<GameObject> targetsAcquired = new List<GameObject>();
    public List<Transform> wayPoints = new List<Transform>();

    [Header("Vars")]
    [SerializeField] private float distance;
    [SerializeField] private float navMinDistance;
    [SerializeField] private float meleeStopDistance;

    public NavMeshAgent navigation;
    public float defaultMovementSpeed;
    public bool isMoving;

    public Attack_Enemy AttackEnemyRef => attack_Enemy;

    private void Start()
    {
        navigation.speed = defaultMovementSpeed;

        if(meleeStopDistance != 0)
        {
            navigation.stoppingDistance = meleeStopDistance;
        }
        else
        {
            navigation.stoppingDistance = navMinDistance;
        }
    }

    private void Update()
    {
        if(targetsAcquired.Count > 0 && attack_Enemy.archetype != Attack_Enemy.Targeting.Neutral)
        {
            FindNearestTarget();
            StoppingDistanceWithinTarget();
            SetMove(false);
            Debug.Log("Finding Enemy");
        }
        else
        {
            SetMove(true);
            StartMoving();
            Debug.Log("Moving to Waypoint");
        }

    }

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TowerBase>() != null && other.GetComponent<TowerBase>() != other.GetComponent<TowerBigBase>())
        {
            if(other.GetComponent<Health>().GetCurrentHealth() != 0)
            {
                if (other.GetComponent<TowerAndEnemy_Archetype>().material == target_Arch.material || target_Arch.material == TowerAndEnemy_Archetype.TypeAndTarget.All) // if Type is same as enemy or is All
                {
                    if(attack_Enemy.attackedTowers.Contains(other.gameObject))
                    {
                  //      return;
                    }

                  //  Debug.Log("Add Tower to List");
                  //  targetsAcquired.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targetsAcquired.Remove(other.gameObject);

        if (targetsAcquired.Count == 0)
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<TowerBase>() != null)
        {
            meleeTrigger = true;
        }
    }
    #endregion

    #region Navigation&TargetingFuncs
    private void FindNearestTarget()
    {
        if (targetsAcquired.Count == 0)
        {
            currentTarget = null;
            distance = Mathf.Infinity;
            return;
        }

        currentTarget = targetsAcquired[0];
        distance = Vector3.Distance(transform.position, currentTarget.transform.position);

        navigation.destination = currentTarget.transform.position;

        if (currentTarget != null)
        {
            navigation.destination = currentTarget.transform.position;
        }
    }
    private void StoppingDistanceWithinTarget() 
    {
        if (currentTarget == null) return;

        if (distance <= navigation.stoppingDistance && attack_Enemy.archetype == Attack_Enemy.Targeting.Ranged)
        {
            OnReachedTarget();
        }
        else if (meleeTrigger && attack_Enemy.archetype == Attack_Enemy.Targeting.Melee) 
        {
            OnReachedTarget();
        }
    }

    private void OnReachedTarget()
    {
        navigation.isStopped = true;

        if (currentTarget != null)
        {
            Debug.Log(this.name + "has reached the target!");
            this.GetComponent<Attack_Enemy>().target = currentTarget.transform;
        }
    }

    public void TargetHasDied()
    {
        StopCoroutine(attack_Enemy.MeleeAttackSpeed());
        attack_Enemy.animator.SetBool("isAttacking", false);
        targetsAcquired.Remove(currentTarget);
        currentTarget.GetComponent<Health>().Die();
        attack_Enemy.attackedTowers.Remove(currentTarget);

        currentTarget = null;
        attack_Enemy.target = null;

        navigation.isStopped = false;

        navigation.ResetPath();

        if(meleeTrigger) meleeTrigger = false;

        FindNearestWaypoint(); // Optional" May feel better or worse

    }

    public void ChangeTarget()
    {
        StopCoroutine(attack_Enemy.MeleeAttackSpeed());
        attack_Enemy.animator.SetBool("isAttacking", false);
        targetsAcquired.Remove(currentTarget);

        currentTarget = null;
        attack_Enemy.target = null;

        navigation.isStopped = false;

        navigation.ResetPath();

        if (meleeTrigger) meleeTrigger = false;

        FindNearestWaypoint(); // Optional" May feel better or worse
    }
    #endregion

    #region WaypointMovement
    public void SetMove(bool state)
    {
        isMoving = state;
    }

    public void StartMoving()
    {
        if (!isMoving || navigation.pathPending)
            return;

        if (!navigation.hasPath)
        {
            navigation.SetDestination(wayPoints[wayPointIndex].position);
        }

        // Near current waypoint go NEXT
        if (!navigation.pathPending && navigation.remainingDistance <= navigation.stoppingDistance)
        {
            AdvanceWaypoint();
        }
    }

    public void FindNearestWaypoint()
    {
        if (wayPoints.Count == 0) return;

        float nearestDistance = Mathf.Infinity;
        int nearestIndex = 0;

        for (int i = 0; i < wayPoints.Count; i++)
        {
            float d = Vector3.Distance(transform.position, wayPoints[i].position);
            if (d < nearestDistance)
            {
                nearestDistance = d;
                nearestIndex = i;
            }
        }

        wayPointIndex = nearestIndex;

        SetMove(true);
        navigation.isStopped = false;
        navigation.ResetPath();
        navigation.SetDestination(wayPoints[wayPointIndex].position);
    }

    private void AdvanceWaypoint()
    {
        wayPointIndex++;

        if (wayPointIndex >= wayPoints.Count)
        {
           // helth.Die();
            return;
        }

        navigation.SetDestination(wayPoints[wayPointIndex].position);
    }
    #endregion

    #region 
    public void SlowDownAgent(int slowValueTarget)
    {
        navigation.speed = moveSpeed / slowValueTarget;
    }

    public void SpeedUpAgent(int speedValueTarget)
    {
        navigation.speed = moveSpeed * speedValueTarget;
    }

    public void SetSpeedAgent(float defaultTargetSpeed)
    {
        navigation.speed = defaultTargetSpeed;
    }

    public float GetSpeedEnemy()
    {
        return navigation.speed;
    }
    #endregion
}
