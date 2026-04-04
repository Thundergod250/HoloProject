using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuidManager : MonoBehaviour
{
    [SerializeField] protected List<GameObject> tutorialGuides;
    [SerializeField] protected int indexCounter = 0;
    [SerializeField] protected float timerToHide = 15f;
    [SerializeField] private bool autoHide = true;
    [SerializeField] private bool resetable = false;
    [SerializeField] private bool autoPlayGuides = false;

    private void Start()
    {
        if (autoPlayGuides)
        {
            StartCoroutine(CO_AutoPlayGuides());
        }
    }

    public void ChangeToNextGuide()
    {
        indexCounter++;
    }

    public void DisableAllGuides()
    {
        for (int i = 0; i < tutorialGuides.Count; i++)
        {
            tutorialGuides[i].SetActive(false);
        }
    }


    public void SetAndEnableGuide(int targetNumberGuide)
    {
        DisableAllGuides();

        tutorialGuides[targetNumberGuide].SetActive(true);

        if (autoHide)
        {
            StartCoroutine(CO_HideGuide());
        }
    }

    IEnumerator CO_HideGuide()
    {
        yield return new WaitForSeconds(timerToHide);
        tutorialGuides[indexCounter].SetActive(false);
    }

    IEnumerator CO_AutoPlayGuides()
    {
        SetAndEnableGuide(indexCounter);

        yield return new WaitForSeconds (timerToHide + 5f);

        ChangeToNextGuide();
        if (indexCounter < tutorialGuides.Count)
        {
            StartCoroutine(CO_AutoPlayGuides());
        }
        else if (resetable && indexCounter > tutorialGuides.Count)
        {
            indexCounter = 0;
            StartCoroutine(CO_AutoPlayGuides());
        }
    }

}
