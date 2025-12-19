using TMPro;
using UnityEngine;

public class ControllerAndButtonTextUI : MonoBehaviour
{
    public TextMeshProUGUI CABText;
    public GameObject Canvas;

    public void _SetDialogueText(string text)
    {
        Canvas.SetActive(true);
        CABText.text = text;
    }

    public void _DisableDialogue()
    {
        Canvas.SetActive(false);
        CABText.text = " ";
    }
}
