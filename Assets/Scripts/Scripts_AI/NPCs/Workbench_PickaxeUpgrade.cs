using TMPro;
using UnityEngine;

public class Workbench_PickaxeUpgrade : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Renderer pickaxeRend;
    [SerializeField] private Renderer pickaxeRendTable;
    [SerializeField] private DropResourceManager gold;
    [SerializeField] private PlayerInteraction playerInteract;

    [Header("Vars")]
    [SerializeField] private Texture2D copperPick;
    [SerializeField] private Texture2D ironPick;
    [SerializeField] private Texture2D goldPick;
    [SerializeField] private TextMeshProUGUI upgradeText;
    private Texture2D currentTexture;
    private Texture2D initialTexture;

    private void Start()
    {
        upgradeText.enabled = false;
        initialTexture = copperPick;
        pickaxeRend.material.mainTexture = initialTexture;
        currentTexture = copperPick;

    }

    private void Update()
    {
        if(playerInteract.interactable == this.gameObject.GetComponent<Interactable>())
        {
            upgradeText.enabled = true;
            pickaxeRendTable.enabled = true;
            pickaxeRendTable.material.mainTexture = pickaxeRend.material.mainTexture;
            if (Input.GetKeyDown(KeyCode.F))
            {
                UpgradePick();
            }
        }
        else if (playerInteract.interactable == null)
        {
            upgradeText.enabled = false;
            pickaxeRendTable.enabled = false;

        }
    }

    public void UpgradePick()
    {
        if (gold.MythrilHold >= 1)
        {
            gold.SpendingToResourceType(upgradeResourceType.Mithril, 1);
            ChangePickTexture();
        }
        else
        {
            Debug.Log("Missing Mithril");
        }
    }

    public void ChangePickTexture()
    {
        if(currentTexture == copperPick)
        {
            currentTexture = ironPick;
        }
        else if (currentTexture == ironPick)
        {
            currentTexture = goldPick;
        }
        else return;

        pickaxeRend.material.mainTexture = currentTexture;
        Debug.Log(pickaxeRend.material.mainTexture);
    }
}
