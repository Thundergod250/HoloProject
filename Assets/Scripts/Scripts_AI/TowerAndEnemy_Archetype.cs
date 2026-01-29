using UnityEngine;

public class TowerAndEnemy_Archetype : MonoBehaviour
{
    public enum TypeAndTarget //ForBothTowersAndEnemies 
    {
        Wood,
        Plastic,
        Metal,
        All,
        None
    }

    [Header("TowerMaterial")]
    [SerializeField] private TypeAndTarget TargetOrFoundation;

    public TypeAndTarget material => TargetOrFoundation;
}
