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
        if (other.GetComponent<Attack_Enemy>())
        {
            Attack_Enemy enemy = other.GetComponent<Attack_Enemy>();

            int selectedEnemyGuide = 0;
            Debug.Log("Found Attack Enemy");

            if (other.GetComponent<Ectoplasm>())
            {
                selectedEnemyGuide = (int)tutorialEnemyGuides.ectoplasmGuide;
            }
            else if(other.GetComponent<Fairy>())
            {
                selectedEnemyGuide = (int)tutorialEnemyGuides.faeGuide;
            }
            else if(other.GetComponent<Burrower>())
            {
                selectedEnemyGuide = (int)tutorialEnemyGuides.earthCrawlerGuide;
            }
            else if (other.GetComponent<Furnace>())
            {
                selectedEnemyGuide = (int)tutorialEnemyGuides.furnaceGuide;
            }
            else if(other.GetComponent<Bloodrunner>())
            {
                selectedEnemyGuide = (int)tutorialEnemyGuides.bloodChaserGuide;
            }
            

            if (!_hasBeenTriggered || _retriggerable)
            {
                TriggerTutorialGuide(selectedEnemyGuide);
            }
        }
    }
}
