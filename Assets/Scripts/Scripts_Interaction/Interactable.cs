using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactName;

    [Header("Interaction Events")]
    public UnityEvent EvtOnFocus;      // repeatedly invoked while raycast hits this
    public UnityEvent EvtOnFocusExit;  // invoked once when raycast leaves
    public UnityEvent EvtOnInteract;   // invoked when player presses interact key

    // Called by PlayerInteraction when this is the current target
    public void Focus()
    {
        EvtOnFocus?.Invoke();
    }

    // Called when no longer targeted
    public void FocusExit()
    {
        EvtOnFocusExit?.Invoke();
    }

    // Called when player presses interact key
    public void Interact()
    {
        EvtOnInteract?.Invoke();
    }
}