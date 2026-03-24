using UnityEngine;

public class HighlightableToggle : MonoBehaviour
{
    [SerializeField] private GameObject objectToToggle;

    public LightingManager lightingManager;
    public PlayerCamManager playerCamMan;
    public RTSCamera _rtsCamera;

    void Start()
    {
        if (objectToToggle != null) objectToToggle.SetActive(false);

        // if (playerCamMan == null) { playerCamMan = FindAnyObjectByType<PlayerCamManager>(); }

        else if(_rtsCamera == null) { _rtsCamera = FindAnyObjectByType<RTSCamera>(); }
    }

    // This works for the Collider method
    void OnMouseEnter()
    {
        if (_rtsCamera != null)
        {
            if (_rtsCamera.isActiveAndEnabled)
            {
                ToggleObject(true);
            }

            else if (!_rtsCamera.isActiveAndEnabled)
            {
                ToggleObject(false);
            }

            Debug.Log("Found RTS Camera");
        }

        else
        {
            Debug.Log("No RTS Camera");
        }

        //if (lightingManager._isNight)
        //{
        //    Debug.Log("Night");
        //    ToggleObject(true);
        //}
        //else return;
    }
    void OnMouseExit() 
    {
        ToggleObject(false);

        //if (lightingManager._isNight)
        //{
        //    ToggleObject(false);
        //}
        //else return;
    }

    // This is called by the Raycast method
    public void ToggleObject(bool state)
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(state);
        }
    }
}
