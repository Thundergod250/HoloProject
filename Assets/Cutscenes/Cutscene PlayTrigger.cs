using UnityEngine;
using UnityEngine.Playables;

public class CutscenePlayTrigger : MonoBehaviour
{

    [SerializeField] CutsceneManager _cutsceneManager;
    [SerializeField] bool _triggerTest = false;
    [SerializeField] int _sceneNumber = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggerTest)
        {
            if (other.GetComponent<PlayerController>())
            {
                _cutsceneManager.PlayCutscene(_sceneNumber);
            }
        }
    }
}
