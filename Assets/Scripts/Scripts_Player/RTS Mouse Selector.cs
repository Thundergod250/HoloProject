using UnityEngine;

public class RTSMouseSelector : MonoBehaviour
{
    public LayerMask interactableLayer;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main == null)
            {
                Debug.LogError("No Camera tagged as 'MainCamera' found!");
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<Interactable>())
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    interactable.Interact();
                }
            }
        }
    }
}
