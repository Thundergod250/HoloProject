using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text promptText;

    public void Show(string interactName)
    {
        panel.SetActive(true);
        promptText.text = $"Press [E] to {interactName}";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}