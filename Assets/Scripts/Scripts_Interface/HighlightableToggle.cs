using UnityEngine;

public class HighlightableToggle : MonoBehaviour
{
    [SerializeField] private GameObject objectToToggle;

    public LightingManager lightingManager;
    public PlayerCamManager playerCamMan;

    void Start()
    {
        if (objectToToggle != null) objectToToggle.SetActive(false);
    }

    // This works for the Collider method
    void OnMouseEnter() 
    {
        if (lightingManager._isNight)
        {
            Debug.Log("Night");
            ToggleObject(true);
        }
        else return;
    }
    void OnMouseExit() 
    {
        if (lightingManager._isNight)
        {
            ToggleObject(false);
        }
        else return;
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
