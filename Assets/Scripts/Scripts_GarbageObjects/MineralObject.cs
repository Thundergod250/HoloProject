using TMPro;
using UnityEngine;

public class MineralObject : MonoBehaviour
{
    public upgradeResourceType _resourceType;
    [SerializeField] public DropResourceManager _resourceManager; // will be injected when spawned
    [SerializeField] public bool isPickedUp = false;
    [SerializeField] private bool hasBeenCollected = false;
    [SerializeField] public int amountToAddInResource = 0;

    [SerializeField] private AudioClip _pickupSoundClip;

    public TextMeshProUGUI addedOreTextEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenCollected)
            return;

        //if (other.TryGetComponent(out PlayerHarvesting targetPlayer) && !isPickedUp)
        if (other.GetComponent<PlayerHarvesting>() && !isPickedUp)
        {
            PlayerHarvesting playerHarvest = other.GetComponent<PlayerHarvesting>();
            hasBeenCollected = true;

            if (_pickupSoundClip!= null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFXOnce(_pickupSoundClip);
            }

            _resourceManager = playerHarvest._resourceManagerRefererce;
            _resourceManager.AddingToResourceType(_resourceType, amountToAddInResource);
            Debug.Log("Adding To Ores");

            Destroy(this.gameObject);
        }
    }

    public void PickedUp()
    {
        isPickedUp = true;
    }
}
