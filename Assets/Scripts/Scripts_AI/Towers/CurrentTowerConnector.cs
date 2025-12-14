using UnityEngine;

public class CurrentTowerConnector : MonoBehaviour
{
    [HideInInspector] public TowerNodeManager CurrentTowerNode;

    private void OnEnable()
    {
        CurrentTowerNode = GameManager.Instance.CurrentTowerNode;
    }

    public void _RepairTower()
    {
        CurrentTowerNode.towerController.TowerHealth.Heal(CurrentTowerNode.towerController.TowerHealth.GetMaxHealth());
    }
}
