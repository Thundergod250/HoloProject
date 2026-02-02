using System.Collections.Generic;
using UnityEngine;

public class Tower_Defense_SlowFieldTower : TowerDefensiveBase
{
    [SerializeField] protected int slowMultiplier = 4;
    public float detectionRadius = 15f;

    [SerializeField] protected List<EnemyMovement> enemiesMovement;

    private void Update()
    {
        SlowDownNearEnemies();
    }

    private void SlowDownNearEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        List<EnemyMovement> currentEnemies = new List<EnemyMovement>();

        // 1. Slow down new enemies
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<EnemyMovement>(out EnemyMovement enemy))
            {
                currentEnemies.Add(enemy);
                if (!enemiesMovement.Contains(enemy))
                {
                    enemy.SlowDownAgent(slowMultiplier);
                }
            }
        }

        // 2. Revert enemies who are no longer in range
        foreach (EnemyMovement enemy in enemiesMovement)
        {
            if (!currentEnemies.Contains(enemy))
            {
                enemy.SpeedUpAgent(slowMultiplier);
            }
        }

        enemiesMovement = currentEnemies;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<EnemyMovement>() )
    //    {
    //        EnemyMovement enemyTarget = other.GetComponent<EnemyMovement>();
    //        enemiesMovement.Add(enemyTarget);

    //        enemyTarget.SlowDownAgent(slowMultiplier);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<EnemyMovement>())
    //    {
    //        EnemyMovement enemyTarget = other.GetComponent<EnemyMovement>();
    //        enemiesMovement.Add(enemyTarget);

    //        enemyTarget.SpeedUpAgent(slowMultiplier);
    //    }
    //}
}
