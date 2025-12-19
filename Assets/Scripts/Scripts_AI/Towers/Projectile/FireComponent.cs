using System.Threading.Tasks;
using UnityEngine;

public class FireComponent : MonoBehaviour
{
    [Header("Settings")]
    public int damagePerTick = 10;
    public float intervalDelay = 1f;
    public int totalTicks = 5;

    [SerializeField] protected EnemyBase enemyBase;

    private int _currentTickCount;

    private async void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyBase>())
        {
            EnemyBase enemyBase = other.GetComponent<EnemyBase>();

            if (enemyBase != null)
            {
                await ApplyDamageOverTime(enemyBase);
            }
        }

        else if (other.GetComponent<FireComponent>())
        {
            FireComponent fire = other.GetComponent<FireComponent>();
            _currentTickCount = 0;
            damagePerTick = fire.damagePerTick;
            intervalDelay = fire.intervalDelay;
            totalTicks = fire.totalTicks;

            await ApplyDamageOverTime(enemyBase);
        }
    }

    private async Task ApplyDamageOverTime(EnemyBase enemy)
    {
        _currentTickCount = 0;

        while (_currentTickCount < totalTicks)
        {
            if (enemy == null) break; // Stop if enemy is destroyed

            enemy.Health.TakeDamage(damagePerTick); // Assuming EnemyBase has TakeDamage(int amount)
            _currentTickCount++;

            await Task.Delay((int)(intervalDelay * 1000)); // Convert seconds to milliseconds
        }
    }
}

