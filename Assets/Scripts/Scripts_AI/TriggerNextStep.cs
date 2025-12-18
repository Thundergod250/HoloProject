using UnityEngine;

public class TriggerNextStep : MonoBehaviour
{
    public NPCDialogue NPCDialogueVar;

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.GetComponent<PlayerMovement>())
            NPCDialogueVar.DialogueForApproachingDoor();*/
    }
}
