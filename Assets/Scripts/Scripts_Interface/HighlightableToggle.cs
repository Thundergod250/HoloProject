using UnityEngine;

public class HighlightableToggle : MonoBehaviour
{
    [SerializeField] private GameObject objectToToggle;

    void Start()
    {
        if (objectToToggle != null) objectToToggle.SetActive(false);
    }

    // This works for the Collider method
    void OnMouseEnter() 
    {
        Debug.Log("Mouse Found");
        ToggleObject(true); 
    }
    void OnMouseExit() { ToggleObject(false); }

    // This is called by the Raycast method
    public void ToggleObject(bool state)
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(state);
        }
    }
}
