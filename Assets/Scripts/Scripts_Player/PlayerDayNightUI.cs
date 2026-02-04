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

    private void Update()
    {
        UpdateUIDayNight();
        ChangeDayNightUI();
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
