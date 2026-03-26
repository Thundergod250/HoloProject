using UnityEngine;

public class Furnace : Effects_Enemy
{
    [Header("Refs")]
    [SerializeField] Attack_Enemy attack_Enemy;

    private void Start()
    {
        attack_Enemy.EnableOnHit();
    }
}
