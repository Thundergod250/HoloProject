using TMPro;
using UnityEngine;

public class NPCDIalogue : MonoBehaviour
{
    public TextMeshProUGUI NPCDialogueText;
    
    public void DialogueText()
    {
        NPCDialogueText.text = "Hello There";
    }

    public void DialogueForApproachingDoor()
    {
        NPCDialogueText.text = "XYZ";
    }

    public void DialogueForWhenDoorReached()
    {
        NPCDialogueText.text = "123";
    }
}
