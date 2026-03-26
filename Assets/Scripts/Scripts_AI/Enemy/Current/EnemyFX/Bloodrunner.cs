using UnityEngine;

public class Bloodrunner : Effects_Enemy
{
    [Header("Refs")]
    [SerializeField] Navigation_Enemy nav_enemy;
    [SerializeField] private Health health;

    private void Update()
    {
        if(health.GetCurrentHealth() <= 50)
        {
            nav_enemy.SpeedUpAgent(2);
        }
        else if(health.GetCurrentHealth() >= 51)
        {
            nav_enemy.SetSpeedAgent(nav_enemy.defaultMovementSpeed);
        }
    }
}
