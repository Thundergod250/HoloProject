using UnityEngine;
using System.Threading.Tasks;

public class TowerOffensiveBase : TowerBase
{
    [SerializeField] private UI_TowerShop towerShop;

    private void Start()
    {
        towerShop = Object.FindAnyObjectByType<UI_TowerShop>();
    }

    // This is the function you call to trigger the "stun" or "recharge"
    public async void DisableForSeconds(float seconds)
    {
        // 1. Disable the script component
        this.enabled = false;
        Debug.Log($"{gameObject.name} disabled for {seconds} seconds.");

        // 2. Wait for the specified time (multiply by 1000 for milliseconds)
        await Task.Delay((int)(seconds * 1000));

        // 3. Re-enable the script component
        // Note: Check if 'this' still exists to avoid errors if the tower was sold/destroyed during the wait
        if (this != null)
        {
            this.enabled = true;
            Debug.Log($"{gameObject.name} is now re-enabled.");
        }
    }

    private void OnMouseDown()
    {
        // Your existing logic...
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        if (towerShop == null)
        {
            towerShop = Object.FindAnyObjectByType<UI_TowerShop>();
        }

        TowerNodeManager node = GetComponentInParent<TowerNodeManager>();

        if (node.towerController != null)
        {
            if (towerShop != null)
            {
                node.towerController.ShowRadiusGuide();
                towerShop.OpenStatusPanel(node.towerController);
            }
        }
        else if (node.towerController == null)
        {
            towerShop.OpenOffensiveTowers();
            Debug.LogError("TowerNodeManager or TowerController is missing on this prefab!");
        }
    }
}
