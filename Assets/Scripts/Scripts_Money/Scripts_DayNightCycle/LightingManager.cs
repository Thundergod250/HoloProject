using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _lightPreset;

    [Header("Variables")] // Change 2nd number (24 total secs  * 15  = (360 secs) == 6 mins per day)
    [SerializeField, Range(0, 360)] private float _timeOfDay;

    private void Update()
    {
        if (_lightPreset == null) return;

        if(Application.isPlaying)
        {
            _timeOfDay += Time.deltaTime;
            _timeOfDay %= 360; // Clamp 0 - 24 // Follow Number above
            UpdateTimeOfDay(_timeOfDay /  360f);
        }
        else
        {
            UpdateTimeOfDay(_timeOfDay /  360f);
        }
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
