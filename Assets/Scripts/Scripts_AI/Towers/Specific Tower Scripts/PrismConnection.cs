using System.Collections.Generic;
using UnityEngine;

public class PrismConnection : MonoBehaviour
{
    private LineRenderer line;
    private BoxCollider col;
    private Transform startPoint;
    private Transform endPoint;
    private TowerPrism sourceTower;
    private TowerPrism targetTower;

    [Header("Combat Settings")]
    public float damageInterval = 1f; // Time between damage ticks
    private int damageBeam;
    private float rangeLimit;

    // Initialize the dictionary here so it is never null
    private Dictionary<Health, float> enemyCooldowns = new Dictionary<Health, float>();

    public void Setup(Transform start, Transform end, TowerPrism source, TowerPrism target, float range, int damage)
    {
        line = GetComponent<LineRenderer>();
        col = GetComponent<BoxCollider>();

        startPoint = start;
        endPoint = end;
        sourceTower = source;
        targetTower = target;
        rangeLimit = range;
        damageBeam = damage;

        line.positionCount = 2;
        line.useWorldSpace = true;
        col.isTrigger = true;
        col.center = Vector3.zero;
    }

    void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        float currentDistance = Vector3.Distance(startPoint.position, endPoint.position);

        if (currentDistance > rangeLimit)
        {
            sourceTower.RemoveConnection(targetTower);
            targetTower.RemoveConnection(sourceTower);
            Destroy(gameObject);
            return;
        }

        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);

        transform.position = (startPoint.position + endPoint.position) / 2f;
        transform.LookAt(endPoint);

        // Size the BoxCollider (X and Y are the width/height of the beam)
        col.size = new Vector3(0.5f, 0.5f, currentDistance);
    }

    private void OnTriggerStay(Collider other)
    {
        // Find the enemy component
        EnemyBase enemy = other.GetComponentInChildren<EnemyBase>();

        if (enemy != null && enemy._healthReference != null)
        {
            Health targetHealth = enemy._healthReference;

            // CHECK COOLDOWN: 
            // If the enemy isn't in the dictionary, OR the current time has passed their next tick...
            if (!enemyCooldowns.ContainsKey(targetHealth) || Time.time >= enemyCooldowns[targetHealth])
            {
                // 1. Deal the damage
                targetHealth.TakeDamage(damageBeam);

                // 2. Set the next allowed time for THIS specific enemy
                enemyCooldowns[targetHealth] = Time.time + damageInterval;

                Debug.Log($"{other.name} hit! Next tick in {damageInterval}s");
            }
        }
    }

    private void OnDestroy()
    {
        // Clean up the dictionary when the beam is destroyed
        if (enemyCooldowns != null)
        {
            enemyCooldowns.Clear();
        }
    }

}
