using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("UI Label")]
    public string interactName = "Interactable";

    [Header("Interaction Event")]
    public UnityEvent onInteract;

    public void Interact()
    {
        onInteract?.Invoke();
    }
}