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

    [Header("Navigation Target")]
    [SerializeField] private List<GameObject> targetsAcquired = new List<GameObject>();
    [SerializeField] GameObject currentTarget;
    [SerializeField] private int wayPointIndex;
    [SerializeField] private float moveSpeed;
    public List<Transform> wayPoints = new List<Transform>();


    [Header("Vars")]
    [SerializeField] private float distance;
    [SerializeField] private float aggroRange;
    [SerializeField] private int navMinDistance;

    public NavMeshAgent navigation;
    public bool isMoving;

    private void Start()
    {
        navigation.stoppingDistance = navMinDistance;
    }

    private void Update()
    {
        if(targetsAcquired.Count > 0 && attack_Enemy.archetype != Attack_Enemy.Targeting.Neutral)
        {
            FindNearestTarget();
            StoppingDistanceWithinTarget();
            SetMove(false);
        }
        else
        {
            SetMove(true);
            StartMoving();
        }

    }

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TowerBase>() != null)
        {
            if(other.GetComponent<Health>().GetCurrentHealth() != 0)
            {
                if (other.GetComponent<TowerAndEnemy_Archetype>().material == target_Arch.material || target_Arch.material == TowerAndEnemy_Archetype.TypeAndTarget.All)
                {
                    targetsAcquired.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targetsAcquired.Remove(other.gameObject);

        if (targetsAcquired.Count == 0)
        {
            aggroRange = 20;
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
        else if (distance <= 3.5f && attack_Enemy.archetype == Attack_Enemy.Targeting.Melee) // revert back to 4.5 if enemies have anims
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
        targetsAcquired.Remove(currentTarget);
        currentTarget.GetComponent<Health>().Die();
        currentTarget = null;
        aggroRange = 21;
        attack_Enemy.target = null;
        navigation.isStopped = false;

        FindNearestWaypoint(); // Optional" May feel better or worse

        navigation.ResetPath();
    }
    #endregion

    #region WaypointMovement
    public void SetMove(bool state)
    {
        isMoving = state;
    }

    public void StartMoving()
    {
        if (!isMoving)
        {
            return;
        }


        if(wayPointIndex < wayPoints.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex].position, Time.deltaTime * moveSpeed);

            var distance = Vector3.Distance(transform.position, wayPoints[wayPointIndex].position);
            if (distance <= 1f)
            {
                wayPointIndex++;

                if (wayPointIndex >= wayPoints.Count)
                {
                    helth.Die();
                }
            }
        }
    }

    public void FindNearestWaypoint()
    {
        if (wayPoints.Count == 0) return;

        float nearestDistance = Mathf.Infinity;
        int nearestIndex = wayPointIndex;

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

        navigation.ResetPath();
        navigation.isStopped = false;
        navigation.SetDestination(wayPoints[wayPointIndex].position);

        SetMove(true);
    }
    #endregion
}
