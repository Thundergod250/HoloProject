using UnityEngine;

public class OnBoardingTrigger : MonoBehaviour
{
    [SerializeField] private TutorialGuidManager _tutorialGuidManager;
    [SerializeField] private tutorialGuides _setSpecificGuide;
    [SerializeField] private bool _retriggerable = false;
    [SerializeField] private bool _hasBeenTriggered = false;

    [SerializeField] private LightingManager _lightingManager;

    public enum tutorialGuides
    {
        walkingMovement,
        shiftMovement,
        miningOres,
        DayNight,
        TabBuild,
        upgradeOre,
        enemyDay,
        workbenchGuide,
        objectiveGuide
    }

    public void TriggerTutorialGuide()
    {
        if (_tutorialGuidManager != null)
        {
            _tutorialGuidManager.SetAndEnableGuide((int)_setSpecificGuide);
            _hasBeenTriggered = true;
        }
        else
        {
            Debug.Log("Tutorial not connected");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (!_hasBeenTriggered || _retriggerable)
            {
                if (_lightingManager != null && _setSpecificGuide == tutorialGuides.DayNight)
                {
                    _lightingManager.StartDayNightTimer();
                    Debug.Log("Tutorial Triggered Light Timer");
                }

                TriggerTutorialGuide();
            }
        }
    }
}
