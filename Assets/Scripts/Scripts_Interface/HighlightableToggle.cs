using UnityEngine;

public class HighlightableToggle : MonoBehaviour
{
    private Outline outline;
    private void Start()
    {
        // Get the Outline component we added in Step 2
        outline = GetComponent<Outline>();

        // Ensure it's off when the game starts
        outline.enabled = false;
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}
