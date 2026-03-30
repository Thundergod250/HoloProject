using UnityEngine;

public class TowerOffensiveBase : TowerBase
{
    [SerializeField] private UI_TowerShop towerShop;

    private void Start()
    {
        towerShop = Object.FindAnyObjectByType<UI_TowerShop>();
    }

    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        if (towerShop == null)
        {
            Debug.Log("Did not find tower, tried again");
            towerShop = Object.FindAnyObjectByType<UI_TowerShop>();

            Debug.Log("Found Tower " + towerShop);
        }
            // 1. Get the Node Manager sitting on this tower's parent or self
            TowerNodeManager node = GetComponentInParent<TowerNodeManager>();

        if (node.towerController != null)
        {
            if (towerShop != null)
            {
                // 3. Directly pass the controller to the UI
                node.towerController.ShowRadiusGuide();
                towerShop.OpenStatusPanel(node.towerController);
            }
        }
        else if(node.towerController == null)
        {
            towerShop.OpenOffensiveTowers();
            Debug.LogError("TowerNodeManager or TowerController is missing on this prefab!");
        }
    }

}
