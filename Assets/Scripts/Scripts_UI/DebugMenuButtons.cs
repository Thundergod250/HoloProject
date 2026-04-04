using UnityEngine;

public class DebugMenuButtons : MonoBehaviour
{
    [SerializeField] private bool _EnableObject = false;

    [SerializeField] private DropResourceManager _dropResourceManager;
    [SerializeField] private LightingManager _lightingManager;

    [SerializeField] private int _addResourceAmount = 50;

    public void DebugAddOres()
    {
        if (_dropResourceManager != null)
        {
            _dropResourceManager?.AddingToResourceType(upgradeResourceType.Copper, _addResourceAmount);
            _dropResourceManager?.AddingToResourceType(upgradeResourceType.Iron, _addResourceAmount);
            _dropResourceManager?.AddingToResourceType(upgradeResourceType.Mithril, _addResourceAmount);
            _dropResourceManager?.AddingToResourceType(upgradeResourceType.Gold, _addResourceAmount);

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
        _lightingManager.ForceTimeOfDay(239);
    }
}
