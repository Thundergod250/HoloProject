using UnityEngine;

public class Drops_Enemy : MonoBehaviour
{
    [Header("Refs")]
    public Spawner parentSpawner;

    [Header("Drop Table")]
    [SerializeField] private GameObject[] DropTable;

    public void DropLoot()
    {
        for (int i = 0; i < DropTable.Length; i++)
        {
            GameObject tempLoot = Instantiate(DropTable[i], transform.position, Quaternion.identity, null);

            tempLoot.transform.position = this.transform.position;
        }

        Destroy(gameObject);
    }

    public void RemoveFromList()
    {
        parentSpawner.UpdateEnemyCounterText();
    }
}
