using UnityEngine;

public class SetTowerSpawn : MonoBehaviour
{
    public void SetTowerSpawnTransform(Transform location)
    {
        GameManager.Instance.CurrentTowerSpawn = location; 
    }
    
    public void SpawnTower(GameObject obj)
    {
        GameManager.Instance.SpawnTower(obj); 
    }
}
