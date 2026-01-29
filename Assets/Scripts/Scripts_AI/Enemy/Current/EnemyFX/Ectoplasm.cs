using UnityEngine;

public class Ectoplasm : Effects_Enemy
{
    [Header("Deathrattle Spawn")]
    [SerializeField] private GameObject slime;
    [SerializeField] private int spawnCount = 2;

    public void OnDeath()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject drattleSlime =  Instantiate(slime, transform.position, Quaternion.identity);
            drattleSlime.GetComponent<Navigation_Enemy>().waypoints


        }
    }
}
