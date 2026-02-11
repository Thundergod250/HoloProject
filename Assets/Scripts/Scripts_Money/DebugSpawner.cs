using UnityEngine;

public class DebugSpawner : MonoBehaviour
{
    [SerializeField] private DropResourceManager _dropResourceManager;

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
}
