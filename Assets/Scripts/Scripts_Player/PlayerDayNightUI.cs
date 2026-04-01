using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDayNightUI : MonoBehaviour
{
    [SerializeField] LightingManager lightingManager;
    [SerializeField] TextMeshProUGUI timeTextUGUI;
    [SerializeField] TextMeshProUGUI dayStatusTextUGUI;

    [SerializeField] Slider dayNightSlider;
    [SerializeField] Slider daySlider;
    [SerializeField] Slider nightSlider;

    [SerializeField] private GameObject _dayUI;
    [SerializeField] private GameObject _nightUI;
    [SerializeField] private bool _debugUISliders = false;

    [SerializeField] private GameObject clockHandUIImage;
    float targetZ = 0;

    [SerializeField] private bool _useCountdown = true;

    private void Start()
    {
        dayStatusTextUGUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateUIDayNight();
        ChangeDayNightUI();
        UpdateHandUIImage();
    }

    private void UpdateHandUIImage()
    {
        float currentTime = lightingManager.GetTimeOfDay();
        float maxTime = 240f;

        // 1. Clamp the time so it never goes above 240 or below 0
        float clampedTime = Mathf.Clamp(currentTime, 0f, maxTime);

        // 2. Convert to 0.0 - 1.0 range
        float timePercent = clampedTime / maxTime;

        // 3. Map to your vertical half-circle (-90 to 90 or 90 to 270)
        // For a "downward" arc starting from the side:
        //float startAngle = -90f; // Starting point (e.g., 9 o'clock)
        //float endAngle = 90f;

        float startAngle = 180f; // Starting point (e.g., 12 o'clock)
        float endAngle = 540f;    // Ending point (e.g., 12 o'clock)

        float targetZ = Mathf.Lerp(startAngle, endAngle, timePercent);

        // 4. Apply rotation
        clockHandUIImage.transform.localRotation = Quaternion.Euler(0, 0, -targetZ);
    }

    private void DebugSliders(bool targetTime)
    {
        if (_debugUISliders)
        {
            if (targetTime)
            {
                _dayUI?.gameObject.SetActive(true);
                _nightUI?.gameObject.SetActive(false);
            }
            else if (!targetTime)
            {
                _dayUI?.gameObject.SetActive(false);
                _nightUI?.gameObject.SetActive(true);
            }
        }
        else
        {
            dayNightSlider.gameObject.SetActive(false);
            daySlider.gameObject.SetActive(false);
            nightSlider.gameObject.SetActive(false);
        }
    }


    public void SetClockDisplayAsCountdown(bool targetClock)
    {
        _useCountdown = targetClock;
    }

    private void ChangeDayNightUI()
    {
        if (!lightingManager._isNight)
        {
            DebugSliders(!lightingManager._isNight);

            daySlider.value = lightingManager.GetTimeOfDay();

            nightSlider.value = 0;

            dayStatusTextUGUI.gameObject.SetActive(true);

            dayStatusTextUGUI.text = "Day";

        }
        else if (lightingManager._isNight)
        {
            DebugSliders(lightingManager._isNight);

            nightSlider.value = lightingManager.GetTimeOfDay();

            daySlider.value = 0;

            dayStatusTextUGUI.gameObject.SetActive(false);

            dayStatusTextUGUI.text = "Night";
        }
    }

    private void UpdateUIDayNight()
    {
        if (lightingManager != null)
        {
            dayNightSlider.value = lightingManager.GetTimeOfDay();

            if (_useCountdown)
            {
                timeTextUGUI.text = GetCountdownTime();
            }
            else
            {
                // Use your existing logic from LightingManager
                timeTextUGUI.text = lightingManager.GetFormattedTime();
            }
        }
    }

    private string GetCountdownTime()
    {
        float currentTime = lightingManager.GetTimeOfDay();
        float maxTime = 240f;

        // Calculate how much time is left in the day
        float timeRemaining = Mathf.Max(0, maxTime - currentTime);

        // Calculate minutes and seconds
        // Mathf.FloorToInt(65 / 60) = 1 minute
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);

        // 65 % 60 = 5 seconds
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        // Formats it to "0:00" style
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

}
