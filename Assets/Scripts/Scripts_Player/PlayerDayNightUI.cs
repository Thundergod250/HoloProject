using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDayNightUI : MonoBehaviour
{
    [SerializeField] LightingManager lightingManager;
    [SerializeField] TextMeshProUGUI timeTextUGUI;
    [SerializeField] TextMeshProUGUI dayStatusTextUGUI;

    [SerializeField] Slider dayNightslider;

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
            //_dayUI?.gameObject.SetActive(true);
            //_nightUI?.gameObject.SetActive(false);

            dayStatusTextUGUI.text = "Day Time";

        }
        else if (lightingManager._isNight)
        {
            //_dayUI?.gameObject.SetActive(false);
            //_nightUI?.gameObject.SetActive(true);

            dayStatusTextUGUI.text = "Night Time";
        }
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
