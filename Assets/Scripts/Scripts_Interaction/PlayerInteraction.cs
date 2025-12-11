using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private UI_Interaction ui_interactionTab;

    private List<Interactable> nearbyInteractables = new List<Interactable>();
    private int selectedIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        var found = other.GetComponent<Interactable>();
        if (found != null && !nearbyInteractables.Contains(found))
        {
            nearbyInteractables.Add(found);
            UpdateUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var found = other.GetComponent<Interactable>();
        if (found != null && nearbyInteractables.Contains(found))
        {
            nearbyInteractables.Remove(found);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, nearbyInteractables.Count - 1);
            UpdateUI();
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || nearbyInteractables.Count == 0)
            return;

        nearbyInteractables[selectedIndex].Interact();
        nearbyInteractables.RemoveAt(selectedIndex);
        selectedIndex = Mathf.Clamp(selectedIndex, 0, nearbyInteractables.Count - 1);
        UpdateUI();
    }

    public void OnScroll(InputAction.CallbackContext ctx)
    {
        if (nearbyInteractables.Count <= 1)
            return;

        float scrollValue = ctx.ReadValue<float>();

        if (scrollValue > 0)
            selectedIndex = (selectedIndex + 1) % nearbyInteractables.Count;
        else if (scrollValue < 0)
            selectedIndex = (selectedIndex - 1 + nearbyInteractables.Count) % nearbyInteractables.Count;
        
        selectedIndex = Mathf.Clamp(selectedIndex, 0, nearbyInteractables.Count - 1);
        ui_interactionTab.ReorderTabs(selectedIndex);
    }

    private void UpdateUI()
    {
        ui_interactionTab.UpdateTabs(nearbyInteractables, selectedIndex);
    }
}