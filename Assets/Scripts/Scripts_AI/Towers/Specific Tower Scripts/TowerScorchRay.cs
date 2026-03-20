using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

public class TowerScorchRay : TowerOffensiveBase
{
    [Header("References")]
    public Transform turretHead;
    public TowerScorchSensor sensorObject;
    public LayerMask enemyLayer;

    [Header("Settings")]
    public float detectionRadius = 15f;
    public int damagePerTick = 10;
    public float attackInterval = 0.5f;
    public float rotationSpeed = 5f;

    
    [SerializeField] protected VisualEffect _fireParticles;

    // List handles "No Limit" enemies for the Scorch Damage
    private List<EnemyBase> _enemiesInDamageZone = new List<EnemyBase>();

    private void Start()
    {
        if (sensorObject != null)
        {
            sensorObject.rayController = this;
        }

        _fireParticles.Stop();

        _ = DamageLoop();
    }

    private void Update()
    {
        // 1. Broad detection for rotation (No Limit)
        Transform target = GetNearestRotationTarget();

        if (target != null)
        {
            RotateTowards(target.position);
        }
    }

    private Transform GetNearestRotationTarget()
    {
        // This returns an array of exactly how many enemies are found
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < hits.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, hits[i].transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = hits[i].transform;
            }
        }
        return closest;
    }

    private void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - turretHead.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private async Task DamageLoop()
    {
        while (Application.isPlaying)
        {
            if (this == null) return;

            // Damage everyone inside the narrow Sensor Trigger
            for (int i = 0; i < _enemiesInDamageZone.Count ; i++)
            {
                EnemyBase enemy = _enemiesInDamageZone[i];

                if (enemy != null)
                {
                    enemy.Health.TakeDamage(damagePerTick);
                }
                else if (enemy == null)
                {
                    _enemiesInDamageZone.RemoveAt(i);
                    ToggleParticles();
                }
            }

            int delayMs = Mathf.RoundToInt(attackInterval * 1000);
            await Task.Delay(delayMs);
        }
    }
    public void OnEnemyEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null && !_enemiesInDamageZone.Contains(enemy))
            {
                _enemiesInDamageZone.Add(enemy);
                ToggleParticles(); // Check if we should start playing
            }
        }
    }

    public void OnEnemyExit(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy != null || enemy == null) // might be a fail safe to removing null?
        {
            _enemiesInDamageZone.Remove(enemy);
            ToggleParticles(); // Check if we should stop playing
        }
    }

    private void ToggleParticles()
    {
        if (_fireParticles == null) return;

        if (_enemiesInDamageZone.Count > 0)
        {
            _fireParticles.Play();
            
        }
        else if(_enemiesInDamageZone.Count<= 0)
        {
            _fireParticles.Stop();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
