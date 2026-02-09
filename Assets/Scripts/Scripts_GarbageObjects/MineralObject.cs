using UnityEngine;

public class MineralObject : MonoBehaviour
{
    public upgradeResourceType _resourceType;
    [SerializeField] public DropResourceManager _resourceManager; // will be injected when spawned
    [SerializeField] public bool isPickedUp = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHarvesting>() && isPickedUp)
        {
            PlayerHarvesting targetPlayer = other.GetComponent<PlayerHarvesting>();

            _resourceManager = targetPlayer._resourceManagerRefererce;
            _resourceManager.AddingToResourceType(_resourceType, 1);

            Destroy(this.gameObject);
        }
    }

    public void PickedUp()
    {
        isPickedUp = true;
    }
}
