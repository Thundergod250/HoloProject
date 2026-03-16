using System.Collections.Generic;
using UnityEngine;

public class Tower_Defense_SlowFieldTower : TowerDefensiveBase
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
                    navEnemy.SlowDownAgent(slowMultiplier);
                }
            }
        }

        // 2. Revert enemies who are no longer in range
        foreach (Navigation_Enemy enemy in enemiesMovement)
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
