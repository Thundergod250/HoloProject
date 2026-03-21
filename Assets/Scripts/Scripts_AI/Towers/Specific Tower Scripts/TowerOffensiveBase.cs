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

        if (towerShop != null)
        {
            // 2. Get the controller from the node manager
            TowerNodeManager node = GetComponentInParent<TowerNodeManager>();

            if (node != null && node.towerController != null)
            {
                // 3. Directly call the UI function and pass the controller
                towerShop.OpenStatusPanel(node.towerController);
            }
        }
    }

}
