using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Interaction_Tab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionName;
    [SerializeField] private Image highlightFrame;

    public void Show(string name)
    {
        interactionName.text = $"[F] {name}";
        interactionName.gameObject.SetActive(true);
    }

    public void SetHighlight(bool isSelected)
    {
        if (highlightFrame != null)
            highlightFrame.enabled = isSelected;

        interactionName.fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;
    }
}