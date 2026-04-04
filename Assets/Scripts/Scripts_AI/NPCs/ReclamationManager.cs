using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;


public class ReclamationManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private List<GameObject> unreclaimedBuildings = new List<GameObject>();
    [SerializeField] private int reclaimedBuildings;
    [SerializeField] private LightingManager lightManager;
    [SerializeField] private CinemachineOrbitalFollow cam;
    [SerializeField] private UI_PromtWarnings _promptWarnings;

    [Header("Boss")]
    [SerializeField] private GameObject bOSS;
    private bool bossSpawnReady;

    [Header("CameraShake")]
    [SerializeField] private float camShakeDuration;
    [SerializeField] private float camShakeMagnitude;

    public GameObject bossRef => bOSS;

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
        if(lightManager._isNight && reclaimedBuildings == 7 && bossSpawnReady)
        {
            if(bOSS != null)
            {
                bOSS.SetActive(true);
            }
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
            StartCoroutine(BossScreenShake(camShakeDuration, camShakeMagnitude));

            if (_promptWarnings != null)
            {
                _promptWarnings.SetPromptTextDisplay("Something's rising and it ain't the shield hero!");
            }
        }
    }

    public void ActivateBoss()
    {
        bossSpawnReady = true;
    }

    public void DebugBossSpawn()
    {
        bOSS.SetActive(true);
    }

    public IEnumerator BossScreenShake(float duration, float strength)
    {
        float ogX = cam.HorizontalAxis.Value;
        float ogY = cam.VerticalAxis.Value;

        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            cam.HorizontalAxis.Value = ogX + x;
            cam.VerticalAxis.Value = ogY + y;

            elapsed += Time.deltaTime;

            yield return null;
        }
        Debug.Log("CAMERA SHALE");

        cam.HorizontalAxis.Value = ogX;
        cam.VerticalAxis.Value = ogY;
    }
}
