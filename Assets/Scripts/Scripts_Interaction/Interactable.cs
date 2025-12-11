using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Name")]
    public string interactName = "Interact";

    [Header("Interaction Event")]
    public UnityEvent onInteract;

    public void Interact()
    {
        onInteract?.Invoke();
    }
}