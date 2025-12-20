using UnityEngine;

public class SetTowerSpawn : MonoBehaviour
{
    public void SetTowerSpawnTransform(TowerNodeManager node)
    {
        GameManager.Instance.CurrentTowerNode = node;
        if (node.towerController == null) return;
        node.towerController.SetTowerNodeManager(node);
    }

    public void SpawnTower(GameObject obj) => GameManager.Instance.SpawnTower(obj);
}
