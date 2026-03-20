using UnityEngine;
using System.Threading.Tasks;

public class TowerFreeze : TowerOffensiveBase
{
    [Header("Tower Stats")]
    public float range = 10f;
    public float freezeDuration = 2f;
    public float attackInterval = 5f;
    public LayerMask enemyLayer; // Set this to your Enemy layer in the Inspector

    private bool canAttack = true;

    void Update()
    {
        if (canAttack)
        {
            ScanAndFreeze();
        }
    }

    private async void ScanAndFreeze()
    {
        // Find all colliders within range on the Enemy layer
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            canAttack = false;

            foreach (Collider col in enemiesInRange)
            {
                EnemyMovement movement = col.GetComponent<EnemyMovement>();
                if (movement != null)
                {
                    movement.ApplyFreeze(freezeDuration);
                }
            }

            // Tower Cooldown
            await Task.Delay((int)(attackInterval * 1000));
            canAttack = true;
        }
    }

    // Visualizes the range in the Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
