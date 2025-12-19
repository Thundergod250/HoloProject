using UnityEngine;
using UnityEngine.Events;

public class WorldTriggers : MonoBehaviour
{
    public bool DontTriggerAnymore;
    public UnityEvent EvtOnPlayerTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() && DontTriggerAnymore)
        {
            EvtOnPlayerTrigger?.Invoke();
            DontTriggerAnymore = false;
        }
    }
}
