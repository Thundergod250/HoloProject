using UnityEngine;

public class Drops_Enemy : MonoBehaviour
{
    [Header("Refs")]
    public Spawner parentSpawner;
    [SerializeField] Transform _spawnPoint;

    [Header("Drop Table")]
    [SerializeField] private GameObject[] DropTable;

    public void DropLoot()
    {

        for (int i = 0; i < DropTable.Length; i++)
        {
            GameObject tempLoot = Instantiate(DropTable[i], _spawnPoint.position, Quaternion.identity, null);

            // Instantiate(DropTable[i], _spawnPoint.position, Quaternion.identity, null);

            // _spawnPoint.SetParent(null); // Spawn it first then reject spawn point
            // tempLoot.transform.position = this.transform.position;
        }

        Destroy(gameObject);
    }

    public void RemoveFromList()
    {
        parentSpawner.UpdateEnemyCounterText();
        parentSpawner.activeEnemies.Remove(this.gameObject);
    }
}
