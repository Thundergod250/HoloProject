using UnityEngine;

public class TowerNodeManager : MonoBehaviour
{
    public TowerController towerController;   
    public Transform spawnTransform;

    public void DespawnTower()
    {
        if (towerController != null)
            GameManager.Instance.DespawnTower(towerController);
        else
            Debug.Log("Tower Controller not found");
    }
}