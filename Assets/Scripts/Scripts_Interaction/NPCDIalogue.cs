using TMPro;
using UnityEngine;

public class NPCDIalogue : MonoBehaviour
{
    public TextMeshProUGUI NPCDialogueText;
    
    public void DialogueText()
    {
        NPCDialogueText.text = "Hello There";
    }
}
