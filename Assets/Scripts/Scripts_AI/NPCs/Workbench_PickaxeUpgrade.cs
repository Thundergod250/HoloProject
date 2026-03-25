using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Workbench_PickaxeUpgrade : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Renderer pickaxeRend;
    [SerializeField] private Renderer pickaxeRendTable;
    [SerializeField] private DropResourceManager gold;
    [SerializeField] private PickaxeLevel picklevel;

    [Header("Vars")]
    [SerializeField] private Texture2D copperPick;
    [SerializeField] private Texture2D ironPick;
    [SerializeField] private Texture2D goldPick;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private UI_PromtWarnings _promptWarnings;
    private Texture2D currentTexture;
    private Texture2D initialTexture;
    private bool playerInside = false;

    [Header("Enemies To Add")]
    [SerializeField] private List<Spawner> spawners = new List<Spawner>();
    [SerializeField] private WaveData ectoplasmWave;

    private DropResourceManager DropResourceManager;
    public static event Action<string> OnResourceShortage;

    private void Start()
    {
        upgradeText.enabled = false;
        initialTexture = copperPick;
        pickaxeRend.material.mainTexture = initialTexture;
        currentTexture = copperPick;
        pickaxeRendTable.enabled = false;
        DropResourceManager = GameManager.Instance.DropManager;
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
            
            _promptWarnings.SetPromptTextDisplay(message);

            Debug.Log("Missing Mithril");
        }
    }

    public void ChangePickTexture()
    {
        if(currentTexture == copperPick && picklevel.copperPick)
        {
            currentTexture = ironPick;
            picklevel.copperPick = false;
            picklevel.ironPick = true;
            picklevel.goldPick = false;
            addEnemies();
        }
        else if (currentTexture == ironPick && picklevel.ironPick)
        {
            currentTexture = goldPick;
            picklevel.copperPick = false;
            picklevel.ironPick = false;
            picklevel.goldPick = true;
        }
        else return;

        pickaxeRend.material.mainTexture = currentTexture;
        Debug.Log(pickaxeRend.material.mainTexture);
    }

    public void addEnemies()
    {
        foreach(Spawner spawnRef in spawners)
        {
            spawnRef.wave.Add(ectoplasmWave);
        }
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
        else if (other.GetComponentInChildren<PickaxeLevel>() != null)
        {
            picklevel = other.GetComponent<PickaxeLevel>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;

        upgradeText.enabled = false;
        pickaxeRendTable.enabled = false;
    }
}
