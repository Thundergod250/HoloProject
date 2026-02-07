using UnityEngine;

public class Effects_Enemy : MonoBehaviour
{
    public enum FieldFX
    {
        Deathrattle,
        InField,
        None
    }

    [Header("EnemyEffects")]
    [SerializeField] private FieldFX enemyFX;

    public FieldFX fieldedEnemyFX => enemyFX;
}
