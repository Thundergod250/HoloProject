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
    
    public void SetTowerManager(TowerNodeManager nodeManager)
    {
        GameManager.Instance.towerNodeManager = nodeManager;
        SetTowerSpawnTransform(nodeManager.spawnTransform); 
    }
}
