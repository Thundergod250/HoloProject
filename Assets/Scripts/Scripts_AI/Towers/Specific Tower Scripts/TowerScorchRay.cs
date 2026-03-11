using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TowerScorchRay : MonoBehaviour
{
    [Header("References")]
    public Transform turretHead;
    public TowerScorchSensor sensorObject;
    public LayerMask enemyLayer;

    [Header("Settings")]
    public int damagePerTick = 10;
    public float attackInterval = 0.5f; // Delay for next shot
    public float rotationSpeed = 5f;

    // List handles "No Limit" enemies
    private List<EnemyBase> _enemiesInRange = new List<EnemyBase>();

    private void Start()
    {
        // Link the sensor to this script
        if (sensorObject != null)
        {
            sensorObject.rayController = this;
        }

        // Start the infinite damage loop
        _ = DamageLoop();
    }

    private void Update()
    {
        // Rotate towards the 1st enemy in the list
        if (_enemiesInRange.Count > 0)
        {
            EnemyBase target = _enemiesInRange[0];

            if (target != null)
            {
                RotateTowards(target.transform.position);
            }
            else
            {
                // Clean up if the enemy was destroyed by something else
                _enemiesInRange.RemoveAt(0);
            }
        }
    }

    private void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - turretHead.position).normalized;
        direction.y = 0; // Keep the tower level

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private async Task DamageLoop()
    {
        // Standard loop using Application.isPlaying instead of tokens
        while (Application.isPlaying)
        {
            if (this == null) return;

            // Damage everyone in the list (No Limit)
            // Loop backwards for safe removal
            for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
            {
                EnemyBase enemy = _enemiesInRange[i];

                if (enemy != null)
                {
                    // Accessing health directly as requested
                    //enemy.health -= damagePerTick;

                    enemy.Health.TakeDamage(damagePerTick);
                }
                else
                {
                    _enemiesInRange.RemoveAt(i);
                }
            }

            // Await Task for the delay between "ticks"
            int delayMs = Mathf.RoundToInt(attackInterval * 1000);
            await Task.Delay(delayMs);
        }
    }

    public void OnEnemyEnter(Collider other)
    {
        // Layer check
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                if (!_enemiesInRange.Contains(enemy))
                {
                    _enemiesInRange.Add(enemy);
                }
            }
        }
    }

    public void OnEnemyExit(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            _enemiesInRange.Remove(enemy);
        }
    }
}
