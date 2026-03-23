using UnityEngine;

public class AggroRange_Enemy : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Navigation_Enemy navigation_enemy;
    [SerializeField] private TowerAndEnemy_Archetype target_Arch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TowerBase>() != null && other.GetComponent<TowerBase>() != other.GetComponent<TowerBigBase>())
        {
            if (other.GetComponent<Health>().GetCurrentHealth() != 0)
            {
                if (other.GetComponent<TowerAndEnemy_Archetype>().material == target_Arch.material || target_Arch.material == TowerAndEnemy_Archetype.TypeAndTarget.All) // if Type is same as enemy or is All
                {
                    if (navigation_enemy.AttackEnemyRef.attackedTowers.Contains(other.gameObject))
                    {
                        return;
                    }

                    Debug.Log("Add Tower to List");
                    navigation_enemy.targetsAcquired.Add(other.gameObject);
                }
            }
        }
    }
}
