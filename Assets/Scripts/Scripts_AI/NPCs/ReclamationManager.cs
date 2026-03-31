using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class ReclamationManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private List<GameObject> unreclaimedBuildings = new List<GameObject>();
    [SerializeField] private int reclaimedBuildings;
    [SerializeField] private LightingManager lightManager;

    [Header("Boss")]
    [SerializeField] private GameObject bOSS;
    private bool bossSpawnReady;

    public static ReclamationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        bOSS.SetActive(false);
        bossSpawnReady = false;
        lightManager = FindAnyObjectByType<LightingManager>();
    }

    private void Update()
    {
        if(lightManager._isNight && reclaimedBuildings == 7)
        {
            if(bOSS != null)
            bOSS.SetActive(true);
        }
    }

    public void CheckIfAllReclaimed()
    {
        reclaimedBuildings = 0;

        foreach (GameObject building in unreclaimedBuildings)
        {
            if(building.GetComponent<Workbench_Towers>().isReclaimed)
            {
                reclaimedBuildings += 1;
                Debug.Log(reclaimedBuildings + " building/s are reclaimed!");
            }
        }

        if(reclaimedBuildings == 7)
        {
            ActivateBoss();
        }
    }


    public void ActivateBoss()
    {
        bossSpawnReady = true;
    }


}
