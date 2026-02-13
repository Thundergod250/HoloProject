using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GoldManager goldManager;
    [SerializeField] private DropResourceManager resourceManager;

    [SerializeField] private GameObject copperImage; 
    [SerializeField] private TextMeshProUGUI copperResourceText;
    [SerializeField] private bool copperEnabled = false;

    [SerializeField] private GameObject ironImage;
    [SerializeField] private TextMeshProUGUI ironResourceText;
    [SerializeField] private bool ironEnabled = false;

    [SerializeField] private GameObject mithrilImage;
    [SerializeField] private TextMeshProUGUI mithrilResourceText;
    [SerializeField] private bool mithrilEnabled = false;

    [SerializeField] private GameObject goldImage;
    [SerializeField] private TextMeshProUGUI goldResourceText;
    [SerializeField] private bool goldEnabled = false;

    // Fading Variables
    [SerializeField] private Image _fadeImage;
    [SerializeField] private TextMeshProUGUI textFade;
    private bool _hasTriggeredFade = false;

    private void Update()
    {
        if (goldManager != null)
        {
            goldText.text = goldManager.PlayerGold.ToString();
            copperResourceText.text = ": " + resourceManager.GetResourceType(upgradeResourceType.Copper).ToString();
            UIOreChecker(upgradeResourceType.Copper);
            ironResourceText.text = ": " + resourceManager.GetResourceType(upgradeResourceType.Iron).ToString();
            UIOreChecker(upgradeResourceType.Iron);
            mithrilResourceText.text = ": " + resourceManager.GetResourceType(upgradeResourceType.Mithril).ToString();
            UIOreChecker(upgradeResourceType.Mithril);
            goldResourceText.text = ": " + resourceManager.GetResourceType(upgradeResourceType.Gold).ToString();
            UIOreChecker(upgradeResourceType.Gold);
        }
        
    }

    private void UIOreChecker(upgradeResourceType targetResource)
    {
        if (resourceManager.GetResourceType(targetResource) >= 1)
        {
            if (targetResource == upgradeResourceType.Copper && !copperEnabled)
            {
                copperEnabled = true;
                copperImage.SetActive(true);
                copperResourceText.gameObject.SetActive(true);
            }
            else if (targetResource == upgradeResourceType.Iron && !ironEnabled)
            {
                ironEnabled = true;
                ironImage.SetActive(true);
                ironResourceText.gameObject.SetActive(true);
            }
            else if (targetResource == upgradeResourceType.Mithril && !mithrilEnabled)
            {
                mithrilEnabled = true;
                mithrilImage.SetActive(true);
                mithrilResourceText.gameObject.SetActive(true);
            }
            else if (targetResource == upgradeResourceType.Gold && !goldEnabled)
            {
                goldEnabled = true;
                goldImage.SetActive(true);
                goldResourceText.gameObject.SetActive(true);
            }
        }
    }

    public void NotEnoughResource(upgradeResourceType resourceType)
    {
        textFade.text = ("Not Enough " + resourceType.ToString() + " Ores");
        StartCoroutine(FadeImageSequence());
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