using UnityEngine;

public class HighlightableToggle : MonoBehaviour
{
    public float interactDistance = 5f; // How far away can you see it?
    private Outline lastOutline;

    private void LateUpdate()
    {
        // 1. Create a ray from the center of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // 2. Check if the ray hits anything within distance
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // 3. Try to find the Outline component on what we hit
            Outline outline = hit.collider.GetComponent<Outline>();

            if (outline != null)
            {
                // If we hit a NEW object, turn off the old one and turn on the new one
                if (lastOutline != outline)
                {
                    ClearHighlight();
                    outline.enabled = true;
                    lastOutline = outline;
                }
            }
            else
            {
                ClearHighlight();
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    private void ClearHighlight()
    {
        if (lastOutline != null)
        {
            lastOutline.enabled = false;
            lastOutline = null;
        }
    }
}
