using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable interactable;

    private void OnTriggerEnter(Collider other)
    {
        var found = other.GetComponent<Interactable>();
        if (found != null)
        {
            interactable = found;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>() == interactable)
        {
            interactable = null;
        }
    }
    
    // This gets called by the Player Input component when the Interact action is triggered
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        Debug.Log("F Button Pressed");
        
        if (interactable != null)
        {
            Debug.Log("Interacted");
            interactable.Interact();
        }
    }
}