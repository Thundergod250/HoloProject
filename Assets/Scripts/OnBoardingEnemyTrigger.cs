using UnityEngine;
public class OnBoardingEnemyTrigger : MonoBehaviour
{
    [SerializeField] private TutorialGuidManager _tutorialGuidManager;
    [SerializeField] private tutorialEnemyGuides _setSpecificGuide;
    [SerializeField] private bool _retriggerable = false;
    [SerializeField] private bool _hasBeenTriggered = false;

    public enum tutorialEnemyGuides
    {
        slimeWispGuide,
        ectoplasmGuide,
        oreGolemGuide,
        earthCrawlerGuide,
        faeGuide,
        furnaceGuide,
        bloodChaserGuide,
        kingSlimeGuide
    }

    public void TriggerTutorialGuide(int setGuideNumber)
    {
        if (_tutorialGuidManager != null)
        {
            _tutorialGuidManager.SetAndEnableEnemyGuide(setGuideNumber);
            _hasBeenTriggered = true;
        }
        else
        {
            Debug.Log("Tutorial not connected");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int setGuideNumber = 0;
        if (other.GetComponentInChildren<Ectoplasm>())
        {
            Debug.Log("Found ectoplasm");
            _setSpecificGuide = tutorialEnemyGuides.ectoplasmGuide;

            setGuideNumber = 1;
        }
        else if (other.GetComponentInChildren<Fairy>())
        {
            Debug.Log("Found Fae");
            _setSpecificGuide = tutorialEnemyGuides.faeGuide;

            setGuideNumber = 4;
        }
        else if (other.GetComponentInChildren<Burrower>())
        {
            Debug.Log("Found Crawler");
            _setSpecificGuide = tutorialEnemyGuides.earthCrawlerGuide;

            setGuideNumber = 3;
        }
        else if (other.GetComponentInChildren<Furnace>())
        {
            Debug.Log("Found Furnace");
            _setSpecificGuide = tutorialEnemyGuides.furnaceGuide;

            setGuideNumber = 5;
        }
        else if (other.GetComponentInChildren<Bloodrunner>())
        {
            Debug.Log("Found BloodChaser");
            _setSpecificGuide = tutorialEnemyGuides.bloodChaserGuide;

            setGuideNumber = 6;
        }
        else if (other.GetComponentInChildren<Bloodrunner>())
        {
            Debug.Log("Found BloodChaser");
            _setSpecificGuide = tutorialEnemyGuides.kingSlimeGuide;

            setGuideNumber = 7;
        }

        else if (other.GetComponent<Attack_Enemy>() && setGuideNumber == 0)
        {
            Attack_Enemy enemy = other.GetComponent<Attack_Enemy>();

            Debug.Log("Found Slime Enemy Attack");

            if (enemy.gameObject.name == "Ore Golem")
            {
                Debug.Log("Found Ore Golem");
                _setSpecificGuide = tutorialEnemyGuides.oreGolemGuide;

                setGuideNumber = 2;
            }
        }

        if (!_hasBeenTriggered || _retriggerable)
        {
            TriggerTutorialGuide(setGuideNumber);
        }
    }
}
