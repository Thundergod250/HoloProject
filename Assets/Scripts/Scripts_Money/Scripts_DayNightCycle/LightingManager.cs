using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _lightPreset;

    [Header("Variables")] // Change 2nd number (24 total secs  * 15  = (360 secs) == 6 mins per day)
    [SerializeField, Range(0, 360)] private float _timeOfDay;

    [SerializeField] protected Volume _hdriCubeSkyDay;
    [SerializeField] protected Volume _hdriCubeSkyNight;

    [SerializeField] List<Cubemap> _hdriCollection;
    [SerializeField] public bool _isMines = false;
    [SerializeField] public bool _isSnow = false;

    [SerializeField] float _transitionSpeed = 0.5f;

    private int _lastAssignedIndex = -1; // Tracks the last HDRI we triggered

    private void Update()
    {
        if (_lightPreset == null) return;

        // Time logic
        if (Application.isPlaying)
        {
            _timeOfDay += Time.deltaTime;
            _timeOfDay %= 360;
            UpdateTimeOfDay(_timeOfDay / 360f);
        }
        else
        {
            UpdateTimeOfDay(_timeOfDay / 360f);
        }

        // --- HDRI TRIGGER LOGIC ---

        // 1. Night to Day Trigger (60 to 65)
        if (_timeOfDay >= 60 && _timeOfDay < 65)
        {
            int targetIndex = _isMines ? 1 : (_isSnow ? 3 : 0);

            if (_lastAssignedIndex != targetIndex)
            {
                StartHDRIFade(targetIndex);
            }
        }
        // 2. Day to Night Trigger (200 to 210)
        else if (_timeOfDay >= 240 && _timeOfDay < 245)
        {
            int targetIndex = 1; // Your Moonless/Night HDRI

            if (_lastAssignedIndex != targetIndex)
            {
                StartHDRIFade(targetIndex);
            }
        }
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

    // Currently 360 Seconds is 1 Day
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
