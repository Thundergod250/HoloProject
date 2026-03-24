using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

//[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _lightPreset;


    [Header("Variables")] 
    // Changed to 240 secs == 4 Mins per day 
    [SerializeField] private float _timeOfDay;
    [SerializeField] public bool _isNight = false;
    [SerializeField] public float _maxTimeOfDay = 240;

    [SerializeField] protected Volume _hdriCubeSkyDay;
    [SerializeField] protected Volume _hdriCubeSkyNight;

    [SerializeField] List<Cubemap> _hdriCollection;
    [SerializeField] public bool _isMines = false;
    [SerializeField] public bool _isSnow = false;

    [SerializeField] float _transitionSpeed = 0.5f;

    [SerializeField] private AudioClip[] _dayThemeClip;
    [SerializeField] private AudioClip[] _nightThemeClip;

    public SaveGameManager saveGameManager;

    private int _lastAssignedIndex = -1; // Tracks the last HDRI we triggered
    private bool hasSavedToday = false;

    public int GetTimeOfDay()
    {
        return (int)_timeOfDay;
    }

    public void ForceTimeOfDay(int targetTime)
    {
        _timeOfDay = targetTime;
    }

    private void Start()
    {
        _isNight = false;
    }

    private void Update()
    {
        if (_lightPreset == null) return;

        // Time logic
        if (Application.isPlaying)
        {
            _timeOfDay += Time.deltaTime;
            _timeOfDay %= _maxTimeOfDay;
            UpdateTimeOfDay(_timeOfDay / _maxTimeOfDay);
        }
        else if (_timeOfDay > _maxTimeOfDay)
        {
            _timeOfDay = 0f;
        }
        else
        {
            UpdateTimeOfDay(_timeOfDay / _maxTimeOfDay);
        }

        // --- HDRI TRIGGER LOGIC ---

        // 1. Night to Day Trigger (60 to 65) // 0 - 150 Day
        if (_timeOfDay >= 0 && _timeOfDay < 150) 
        {
            int targetIndex = _isMines ? 1 : (_isSnow ? 3 : 0);

            if (!hasSavedToday)
            {
                if (saveGameManager != null)
                {
                    // saveGameManager.SaveGame(_timeOfDay,GameManager.Instance.DropManager); // Trigger your SaveData logic

                    Debug.Log("Progress saved.");
                    hasSavedToday = true;
                }
                else
                {
                    Debug.Log("Cannot Save! No Save Game Manager");

                    hasSavedToday = true;
                }
            }
            else
            {
                hasSavedToday = true;
                Debug.Log("Sun is up!");
            }


            if (_lastAssignedIndex != targetIndex)
            {
                StartHDRIFade(targetIndex);

                int randomClip = Random.Range(0, _dayThemeClip.Count());

                AudioManager.Instance.PlayMusic(_dayThemeClip[randomClip]);
            }

            _isNight = false;
        }
        // 2. Day to Night Trigger (240 to 245) // 221 - 360 Night
        else if (_timeOfDay >= 151 && _timeOfDay < _maxTimeOfDay) 
        {
            int targetIndex = 1; // Your Moonless/Night HDRI

            hasSavedToday = true;

            Debug.Log("Night is up!");

            if (_lastAssignedIndex != targetIndex)
            {
                StartHDRIFade(targetIndex);

                int randomClip = Random.Range(0, _nightThemeClip.Count());
                AudioManager.Instance.PlayMusic(_nightThemeClip[randomClip]);
            }

            _isNight = true;
        }
    }

    public string GetFormattedTime()
    {
        // 1. Calculate total in-game minutes passed
        // (timeOfDay / maxTimeOfDay) gives us the percentage of the day completed
        float totalMinutes = (_timeOfDay / _maxTimeOfDay) * 1440f;

        // 2. Breakdown into hours and minutes
        int hours = Mathf.FloorToInt(totalMinutes / 60);
        int minutes = Mathf.FloorToInt(totalMinutes % 60);

        // 3. Format as "00:00"
        return string.Format("{0:00}:{1:00}", hours, minutes);
    }
    private void StartHDRIFade(int index)
    {
        _lastAssignedIndex = index;
        StopAllCoroutines();
        StartCoroutine(FadeHDRIRoutine(index));
    }

    private IEnumerator FadeHDRIRoutine(int targetIndex)
    {
        // Determine which volume is currently "on" and which is "off"
        Volume activeVol = _hdriCubeSkyDay.weight > 0.5f ? _hdriCubeSkyDay : _hdriCubeSkyNight;
        Volume targetVol = activeVol == _hdriCubeSkyDay ? _hdriCubeSkyNight : _hdriCubeSkyDay;

        // Load the new HDRI into the volume that is currently hidden (weight 0)
        if (targetVol.profile.TryGet<HDRISky>(out var sky))
        {
            sky.hdriSky.value = _hdriCollection[targetIndex];
        }

        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime * _transitionSpeed;

            targetVol.weight = t;       // Fades the new sky IN
            activeVol.weight = 1f - t;  // Fades the old sky OUT

            yield return null;
        }

        targetVol.weight = 1f;
        activeVol.weight = 0f;
    }

    // Currently 240 Seconds is 1 Day
    // Night time At 300 (Reset Piles) -> 60 Night end

    private void UpdateTimeOfDay(float timePercent)
    {
        RenderSettings.ambientLight = _lightPreset._ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _lightPreset._fogColor.Evaluate(timePercent);

        if (_directionalLight != null)
        {
            _directionalLight.color = _lightPreset._directionalColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if (_directionalLight != null) { return; }

        if (RenderSettings.sun != null) 
        {
            _directionalLight = RenderSettings.sun;
        }
        else
        {
            // Checks for all Lights in Components
            Light[] lights = GetComponentsInChildren<Light>();

            // Finds all Directional Light
            foreach (Light light in lights) 
            {
                if (light.type == LightType.Directional)
                {
                    _directionalLight = light;
                    return;
                }
            }
        }
    }
}
