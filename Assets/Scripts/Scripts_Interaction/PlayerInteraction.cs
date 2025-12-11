using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private UI_Interaction ui_interactionTab;

    private Interactable currentInteractable;

    private void OnTriggerEnter(Collider other)
    {
        Interactable found = other.GetComponent<Interactable>();
        if (found != null)
        {
            currentInteractable = found;
            ui_interactionTab.Show(found.interactName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable found = other.GetComponent<Interactable>();
        if (found == currentInteractable)
        {
            currentInteractable = null;
            ui_interactionTab.Hide();
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || currentInteractable == null)
            return;

        currentInteractable.Interact();
        currentInteractable = null;
        ui_interactionTab.Hide();
    }
}