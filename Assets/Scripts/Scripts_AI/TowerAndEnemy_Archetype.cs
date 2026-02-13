using UnityEngine;

public class TowerAndEnemy_Archetype : MonoBehaviour
{
    public enum TypeAndTarget //ForBothTowersAndEnemies 
    {
        Wood,
        Plastic,
        Metal,
        All,
        Base,
        None
    }

    [Header("TowerMaterial")]
    [SerializeField] private TypeAndTarget TargetOrFoundation;

    public TypeAndTarget material => TargetOrFoundation;
}
