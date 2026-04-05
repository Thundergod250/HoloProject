using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Workbench_PickaxeUpgrade : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Renderer pickaxeRend;
    [SerializeField] private Renderer pickaxeRendTable;
    [SerializeField] private DropResourceManager gold;
    [SerializeField] private PickaxeLevel picklevel;
    [SerializeField] private ParticleSystem _pickPoofUpgrade;

    [Header("Vars")]
    [SerializeField] private Texture2D copperPick;
    [SerializeField] private Texture2D ironPick;
    [SerializeField] private Texture2D goldPick;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private UI_PromtWarnings _promptWarnings;
    [SerializeField] private int mithrilTargetUpgrade = 5;

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
        upgradeUI.SetActive(false);
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
        if (gold.MythrilHold >= mithrilTargetUpgrade)
        {
            gold.SpendingToResourceType(upgradeResourceType.Mithril, mithrilTargetUpgrade);
            
            NextTargetPickUpgrade(3); // so 30 Mithril from 10

            upgradeText.text = mithrilTargetUpgrade.ToString();

            _pickPoofUpgrade.Play();
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

    public void NextTargetPickUpgrade(int targetMultiplier)
    {
        mithrilTargetUpgrade = targetMultiplier * mithrilTargetUpgrade;
    }

    public void ChangePickTexture()
    {
        if(currentTexture == copperPick && picklevel.copperPick)
        {
            currentTexture = ironPick;
            picklevel.copperPick = false;
            picklevel.ironPick = true;
            picklevel.goldPick = false;
            ChangeWaveSet(1);
        }
        else if (currentTexture == ironPick && picklevel.ironPick)
        {
            currentTexture = goldPick;
            picklevel.copperPick = false;
            picklevel.ironPick = false;
            picklevel.goldPick = true;
            ChangeWaveSet(2);

        }
        else return;

        pickaxeRend.material.mainTexture = currentTexture;
        Debug.Log(pickaxeRend.material.mainTexture);
    }

    public void ChangeWaveSet(int i)
    {
        foreach(Spawner spawnRef in spawners)
        {
            spawnRef.GetNewSetOfWaves(i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerInside = true;
            upgradeText.text = mithrilTargetUpgrade.ToString();
            upgradeUI.SetActive(true);
            upgradeText.enabled = true;
            pickaxeRendTable.enabled = true;

          //  pickaxeRendTable.material.mainTexture = pickaxeRend.material.mainTexture;
        }
        else if (other.GetComponentInChildren<PickaxeLevel>())
        {
            picklevel = other.GetComponent<PickaxeLevel>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;

        upgradeUI.SetActive(false);
        upgradeText.enabled = false;
        pickaxeRendTable.enabled = false;
    }
}
