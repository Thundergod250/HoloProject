using UnityEngine;

public class MineralObject : MonoBehaviour
{
    public upgradeResourceType _resourceType;
    [SerializeField] public DropResourceManager _resourceManager; // will be injected when spawned


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHarvesting>())
        {
            PlayerHarvesting targetPlayer = other.GetComponent<PlayerHarvesting>();

            _resourceManager = targetPlayer._resourceManagerRefererce;
            _resourceManager.AddingToResourceType(_resourceType, 1);

            Destroy(this.gameObject);
        }
    }
}
