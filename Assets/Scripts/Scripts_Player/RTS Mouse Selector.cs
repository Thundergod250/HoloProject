using UnityEngine;
using UnityEngine.EventSystems;

public class RTSMouseSelector : MonoBehaviour
{
    public LayerMask interactableLayer;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 1. Check if the mouse is over a UI element first
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // If it is, we STOP here and don't fire the raycast
                return;
            }

            if (Camera.main == null)
            {
                Debug.LogError("No Camera tagged as 'MainCamera' found!");
                return;
            }

            // 2. Perform the raycast only if we aren't clicking UI
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Added the interactableLayer here to make your raycast more efficient
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    interactable.InteractWithTarget();
                }
            }
        }
    }
}
