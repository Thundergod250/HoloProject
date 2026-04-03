using System.Collections.Generic;
using UnityEngine;

public class Tower_Defense_SlowFieldTower : TowerOffensiveBase
{
    [SerializeField] protected int slowMultiplier = 4;
    public float detectionRadius = 15f;

    [SerializeField] protected List<Navigation_Enemy> enemiesMovement;

    private void Update()
    {
        SlowDownNearEnemies();
    }

    private void SlowDownNearEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        List<Navigation_Enemy> currentEnemies = new List<Navigation_Enemy>();

        // 1. Slow down new enemies
        foreach (Collider hit in hits)
        {
            //if (hit.TryGetComponent<Navigation_Enemy>(out Navigation_Enemy enemy))
            if (hit.GetComponent<Navigation_Enemy>())
            {
                Navigation_Enemy navEnemy = hit.GetComponent<Navigation_Enemy>();
                currentEnemies.Add(navEnemy);

                if (!enemiesMovement.Contains(navEnemy))
                {
                    navEnemy.PlaySlowVFX();
                    navEnemy.SlowDownAgent(slowMultiplier);
                }
            }
        }

        // 2. Revert enemies who are no longer in range
        foreach (Navigation_Enemy enemy in enemiesMovement)
        {
            if (!currentEnemies.Contains(enemy))
            {
                //enemy.SpeedUpAgent(slowMultiplier);
                enemy.SetSpeedAgent(enemy.defaultMovementSpeed);
                enemy.StopSlowVFX();
            }
        }

        enemiesMovement = currentEnemies;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
