using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDayNightUI : MonoBehaviour
{
    [SerializeField] LightingManager lightingManager;
    [SerializeField] TextMeshProUGUI timeTextUGUI;
    [SerializeField] Slider dayNightslider;

    private void Update()
    {
        UpdateUIDayNight();
    }

    private void UpdateUIDayNight()
    {
        if (lightingManager != null)
        {
            dayNightslider.value = lightingManager.GetTimeOfDay();
            timeTextUGUI.text = lightingManager.GetTimeOfDay().ToString();
        }
    }

}
