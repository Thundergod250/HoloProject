using UnityEngine;

public class TowerOffensiveBase : TowerBase
{
    [SerializeField] private UI_TowerShop towerShop;

    private void Start()
    {
        towerShop = Object.FindAnyObjectByType<UI_TowerShop>();

        if (towerShop != null)
        {
            TowerNodeManagerManipulator manipulator = GetComponent<TowerNodeManagerManipulator>();
            // Add the listener via code so it works even for spawned prefabs
            manipulator.EvtOnInteractWithTowerController.AddListener(towerShop.OpenStatusPanel);
        }
    }


    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        TowerNodeManagerManipulator manipulator = GetComponent<TowerNodeManagerManipulator>();
        if (manipulator != null)
        {
            // Find the UI_TowerShop in the scene dynamically

            if (towerShop != null)
            {
                // Manually trigger the logic or ensure the manipulator's 
                // UnityEvent is pointing to this shopUI instance.
                manipulator._SendTowerController();
            }
        }
    }

}
