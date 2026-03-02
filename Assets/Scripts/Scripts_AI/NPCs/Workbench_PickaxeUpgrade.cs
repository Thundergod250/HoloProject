using System;
using TMPro;
using UnityEngine;

public class Workbench_PickaxeUpgrade : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Renderer pickaxeRend;
    [SerializeField] private Renderer pickaxeRendTable;
    [SerializeField] private DropResourceManager gold;
    [SerializeField] private PlayerInteraction playerInteract; //NOT USED

    [Header("Vars")]
    [SerializeField] private Texture2D copperPick;
    [SerializeField] private Texture2D ironPick;
    [SerializeField] private Texture2D goldPick;
    [SerializeField] private TextMeshProUGUI upgradeText;
    private Texture2D currentTexture;
    private Texture2D initialTexture;
    private bool playerInside = false;

    public static event Action<string> OnResourceShortage;

    private void Start()
    {
        upgradeText.enabled = false;
        initialTexture = copperPick;
        pickaxeRend.material.mainTexture = initialTexture;
        currentTexture = copperPick;
        pickaxeRendTable.enabled = false;

    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            UpgradePick();
            pickaxeRendTable.material.mainTexture = pickaxeRend.material.mainTexture;
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
            string message = "Not Enough ores:   Mithril";
            OnResourceShortage?.Invoke(message);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = true;

            upgradeText.enabled = true;
            pickaxeRendTable.enabled = true;

          //  pickaxeRendTable.material.mainTexture = pickaxeRend.material.mainTexture;
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;

        upgradeText.enabled = false;
        pickaxeRendTable.enabled = false;
    }
}
