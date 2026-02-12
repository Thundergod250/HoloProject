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


    private void Update()
    {
        UpdateUIDayNight();
        ChangeDayNightUI();
        CheckForFadeTrigger();
    }

    private void CheckForFadeTrigger()
    {
        float currentTime = lightingManager.GetTimeOfDay();

        // Trigger when time hits 150
        if (Mathf.FloorToInt(currentTime) == 150 && !_hasTriggeredFade)
        {
            StartCoroutine(FadeImageSequence());
            _hasTriggeredFade = true;
            _fadeImage.gameObject.SetActive(true);
        }

        // Reset the flag for the next cycle (assuming day length is > 150)
        if (currentTime < 10)
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
            timeTextUGUI.text = lightingManager.GetTimeOfDay().ToString();
        }
    }

}
