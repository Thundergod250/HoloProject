using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompendiumManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedRangeText;
    public TextMeshProUGUI descriptionText;
    public Image displayImage;

    [Header("Data")]
    public List<PageData> allPages; // Drop your created Page assets here
    private int _currentIndex = 0;

    void Start()
    {
        DisplayPage();
    }

    public void NextPage()
    {
        _currentIndex++;
        if (_currentIndex >= allPages.Count)
        {
            _currentIndex = 0;
        }
        DisplayPage();
    }

    public void PreviousPage()
    {
        // Replace your old PreviousPage with this:
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = allPages.Count - 1;
        }
        DisplayPage();
    }

    void DisplayPage()
    {
        if (allPages.Count == 0) return;

        PageData currentPage = allPages[_currentIndex];

        titleText.text = currentPage.pageTitle;
        healthText.text = currentPage.pageHealth.ToString();
        damageText.text = currentPage.pageDamage.ToString();
        speedRangeText.text = currentPage.pageSpeedRange;

        descriptionText.text = currentPage.pageDescription;
        displayImage.sprite = currentPage.pageImage;
    }
}
