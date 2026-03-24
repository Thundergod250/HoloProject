using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompendiumManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
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
        if (_currentIndex < allPages.Count - 1)
        {
            _currentIndex++;
            DisplayPage();
        }
    }

    public void PreviousPage()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            DisplayPage();
        }
    }

    void DisplayPage()
    {
        if (allPages.Count == 0) return;

        PageData currentPage = allPages[_currentIndex];

        titleText.text = currentPage.pageTitle;
        descriptionText.text = currentPage.pageDescription;
        displayImage.sprite = currentPage.pageImage;
    }
}
