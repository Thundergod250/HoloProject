using UnityEngine;

public class GeneralRemnant : Effects_Enemy
{
    [Header("Rally Effect")]
    [SerializeField] private int rallyHealth;

    public void ApplyRally(EnemyBase enemy)
    {
        enemy.Health.Heal(20);
    }

    public void RemoveRally(EnemyBase enemy)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemy))
        {
            ApplyRally(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemy))
        {
            RemoveRally(enemy);
        }
    }
}
