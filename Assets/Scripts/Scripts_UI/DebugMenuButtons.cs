using UnityEngine;

public class DebugMenuButtons : MonoBehaviour
{
    [SerializeField] private bool _EnableObject = false;

    [SerializeField] private DropResourceManager _dropResourceManager;
    [SerializeField] private LightingManager _lightingManager;
    public void DebugAddOres()
    {
        if (_dropResourceManager != null)
        {
            _dropResourceManager?.AddingToResourceType(upgradeResourceType.Copper, 20);
            _dropResourceManager?.AddingToResourceType(upgradeResourceType.Iron, 20);
            _dropResourceManager?.AddingToResourceType(upgradeResourceType.Mithril, 20);
            _dropResourceManager?.AddingToResourceType(upgradeResourceType.Gold, 20);

            GameManager.Instance?.GoldManager.AddGold(20);
        }
    }

    public void EnableDisableGameObject(GameObject targetGameObject)
    {
        if (!_EnableObject)
        {
            targetGameObject.SetActive(false);
            _EnableObject = true;
        }
        else if (_EnableObject)
        {
            targetGameObject.SetActive(true);
            _EnableObject = false;
        }
    }

    public void ForceTimeNight()
    {
        _lightingManager.ForceTimeOfDay(149);
    }
    public void ForceTimeDay()
    {
        _lightingManager.ForceTimeOfDay(240);
    }
}
