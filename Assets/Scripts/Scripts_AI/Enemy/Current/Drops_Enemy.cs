using UnityEngine;

public class Drops_Enemy : MonoBehaviour
{
    [Header("Refs")]
    public Spawner parentSpawner;
    [SerializeField] Transform _spawnPoint;
    public DropResourceManager  _dropResourceManager;

    [Header("Drop Table")]
    [SerializeField] private GameObject[] DropTable;

    public void DropLoot()
    {
        if(DropTable.Length != 0)
        {
            for (int i = 0; i < DropTable.Length; i++)
            {
                GameObject tempLoot = Instantiate(DropTable[i], _spawnPoint.position, Quaternion.identity, null);

                _dropResourceManager.AddingToResourceType(tempLoot.GetComponent<MineralObject>()._resourceType, tempLoot.GetComponent<MineralObject>().amountToAddInResource);

                Destroy(tempLoot);
            }
        }

        Destroy(gameObject);
    }

    public void RemoveFromList()
    {
        if (parentSpawner!= null)
        {
            parentSpawner.UpdateEnemyCounterText();
            parentSpawner.activeEnemies.Remove(this.gameObject);
        }
    }
}
