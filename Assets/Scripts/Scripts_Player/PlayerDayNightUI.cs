using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    // Fading Variables
    [SerializeField] private Image _fadeImage;
    [SerializeField] private TextMeshProUGUI textFade;
    private bool _hasTriggeredFade = false;

    [SerializeField] private GameObject clockHandUIImage;
    float targetZ = 0; 


    private void Update()
    {
        UpdateUIDayNight();
        ChangeDayNightUI();
        CheckForFadeTrigger();
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
        float startAngle = -90f; // Starting point (e.g., 9 o'clock)
        float endAngle = 90f;    // Ending point (e.g., 3 o'clock)

        float targetZ = Mathf.Lerp(startAngle, endAngle, timePercent);

        // 4. Apply rotation
        clockHandUIImage.transform.localRotation = Quaternion.Euler(0, 0, targetZ);
    }

    private void CheckForFadeTrigger()
    {
        float currentTime = lightingManager.GetTimeOfDay();

        // Trigger when time hits 150
        if (Mathf.FloorToInt(currentTime) == 150 && !_hasTriggeredFade)
        {
            StartCoroutine(FadeImageSequence());
            _hasTriggeredFade = true;
            textFade.text = "The Night is here defend the Base";
            _fadeImage.gameObject.SetActive(true);
        }

        else if (Mathf.FloorToInt(currentTime) == 0 && !_hasTriggeredFade)
        {
            StartCoroutine(FadeImageSequence());
            _hasTriggeredFade = true;
            textFade.text = "The Day of gathering resources";
            _fadeImage.gameObject.SetActive(true);
        }

        // Reset the flag for the next cycle (assuming day length is > 150)
        if ( ( ((currentTime <= 153) && (currentTime >= 151)) || (currentTime >= 2) && (currentTime <= 4)) && _hasTriggeredFade)
        {
            _hasTriggeredFade = false;
        }
    }

    private IEnumerator FadeImageSequence()
    {
        // 1. Fade In over 1 second
        yield return StartCoroutine(FadeAlpha(0, 1, 1f));

        // 2. Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // 3. Fade Out over 2 seconds
        yield return StartCoroutine(FadeAlpha(1, 0, 2f));
        _fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeAlpha(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color tempColor = _fadeImage.color;
        tempColor = textFade.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Linearly interpolate the alpha value
            tempColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            _fadeImage.color = tempColor;
            textFade.color = tempColor;
            yield return null;
        }

        // Ensure we hit the exact target at the end
        tempColor.a = endAlpha;
        _fadeImage.color = tempColor;
        textFade.color = tempColor;
    }
    private void ChangeDayNightUI()
    {
        if (!lightingManager._isNight)
        {
            _dayUI?.gameObject.SetActive(true);
            _nightUI?.gameObject.SetActive(false);

            daySlider.value = lightingManager.GetTimeOfDay();

            nightSlider.value = 0;

            dayStatusTextUGUI.text = "Day Time";

        }
        else if (lightingManager._isNight)
        {
            _dayUI?.gameObject.SetActive(false);
            _nightUI?.gameObject.SetActive(true);

            nightSlider.value = lightingManager.GetTimeOfDay();

            daySlider.value = 0;

            dayStatusTextUGUI.text = "Night Time";
        }
    }

    private void UpdateUIDayNight()
    {
        if (lightingManager != null)
        {
            dayNightSlider.value = lightingManager.GetTimeOfDay();
            // timeTextUGUI.text = lightingManager.GetTimeOfDay().ToString();
            timeTextUGUI.text = lightingManager.GetFormattedTime();
        }
    }

}
