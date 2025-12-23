using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Navigation_Enemy : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Attack_Enemy attack_Enemy;

    [Header("Colliders")]
    [SerializeField] private SphereCollider sphereCollider;

    [Header("Navigation Target")]
    [SerializeField] private List<GameObject> targetsAcquired = new List<GameObject>();
    [SerializeField] GameObject currentTarget;

    [Header("Vars")]
    [SerializeField] private float distance;
    [SerializeField] private float aggroRange;
    [SerializeField] private int navMinDistance;

    public NavMeshAgent navigation;

    private void Start()
    {
        navigation.stoppingDistance = navMinDistance;
    }

    private void Update()
    {
        if(targetsAcquired.Count > 0)
        {
            FindNearestTarget();
            StoppingDistanceWithinTarget();
        }
    }

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TowerBaseFunction>() != null)
        {
            if(other.GetComponent<Health>().GetCurrentHealth() != 0)
            {
                targetsAcquired.Add(other.gameObject);
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
        float nearestDistance = currentTarget != null
         ? Vector3.Distance(transform.position, currentTarget.transform.position)
         : Mathf.Infinity;

        GameObject nearestTarget = currentTarget;

        for (int i = 0; i < targetsAcquired.Count; i++)
        {
            float d = Vector3.Distance(transform.position, targetsAcquired[i].transform.position);

            if (d < nearestDistance)
            {
                nearestDistance = d;
                nearestTarget = targetsAcquired[i];
            }
        }

        currentTarget = nearestTarget;
        distance = nearestDistance;

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
        else if (distance <= 4.5f && attack_Enemy.archetype == Attack_Enemy.Targeting.Melee)
        {
            OnReachedTarget();
        }
    }

    private void OnReachedTarget()
    {
        navigation.isStopped = true;

        if (currentTarget != null)
        {
            this.GetComponent<Attack_Enemy>().target = currentTarget.transform;

        }
    }

    public void TargetHasDied()
    {
        targetsAcquired.Remove(currentTarget);
        currentTarget = null;
        aggroRange = 21;
        attack_Enemy.target = null;
        navigation.isStopped = false;
    }
    #endregion
}
