using System;
using UnityEngine;
using UnityEngine.Events;

public class TowerNodeManagerManipulator : MonoBehaviour
{
    public UnityEvent<TowerController> EvtOnInteractWithTowerController;
    
    private TowerNodeManager towerNodeManager;

    private void OnEnable()
    {
        towerNodeManager = GetComponent<TowerNodeManager>();
    }

    public void _SendTowerController()
    {
        EvtOnInteractWithTowerController?.Invoke(towerNodeManager?.towerController);
    }
}
