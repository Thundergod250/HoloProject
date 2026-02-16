using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_PromtWarnings : MonoBehaviour
{
    [SerializeField] LightingManager lightingManager;
    // Fading Variables
    [SerializeField] private Image _fadeImage;
    [SerializeField] private TextMeshProUGUI textFade;
    private bool _hasTriggeredFade = false;

    private void OnEnable()
    {
        // Subscribe: When BuyTower broadcasts, run SetPromptTextDisplay
        BuyTower.OnResourceShortage += SetPromptTextDisplay;
    }

    private void OnDisable()
    {
        // Unsubscribe: Clean up when this UI is hidden/destroyed
        BuyTower.OnResourceShortage -= SetPromptTextDisplay;
    }

    private void Update()
    {
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
        if ((((currentTime <= 153) && (currentTime >= 151)) || (currentTime >= 2) && (currentTime <= 4)) && _hasTriggeredFade)
        {
            _hasTriggeredFade = false;
        }
    }



    public void SetPromptTextDisplay(string targetText)
    {
        StartCoroutine(FadeImageSequence());
        textFade.text = targetText;
        _hasTriggeredFade = true;
        _fadeImage.gameObject.SetActive(true);
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
}
