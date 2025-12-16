using UnityEngine;
using UnityEngine.Events;

public class PlayerGrab : MonoBehaviour
{
    public Transform PlayerGrabPoint;
    public bool IsPlayerCarryingObject;

    // === Events ===
    public UnityEvent<GameObject> EvtOnGrab;
    public UnityEvent<GameObject> EvtOnReleaseGrabObj;
    public UnityEvent<GameObject> EvtOnRemovedGrabbedObject;

    [SerializeField] private float tossForce = 5f; 
    private GameObject currentGrabbedObj;

    public void GrabObject(GameObject obj)
    {
        if (IsPlayerCarryingObject || obj == null) return;

        // Parent to grab point
        currentGrabbedObj = obj;
        obj.transform.SetParent(PlayerGrabPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // Disable physics while carried
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        IsPlayerCarryingObject = true;

        // Trigger event
        EvtOnGrab?.Invoke(obj);
    }

    public void ReleaseGrabbedObject(GameObject obj)
    {
        if (!IsPlayerCarryingObject || obj == null) return;

        // Unparent
        obj.transform.SetParent(null);

        // Re-enable physics and toss forward
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;

            Vector3 releaseForce = transform.forward * tossForce; // tweak force
            rb.AddForce(releaseForce, ForceMode.Impulse);
        }

        IsPlayerCarryingObject = false;
        currentGrabbedObj = null;

        // Trigger event
        EvtOnReleaseGrabObj?.Invoke(obj);
    }

    public void RemoveGrabObject(GameObject obj)
    {
        if (!IsPlayerCarryingObject || obj == null) return;

        // Just unparent, no force
        obj.transform.SetParent(null);

        // Re-enable physics but no toss
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        IsPlayerCarryingObject = false;
        currentGrabbedObj = null;

        // Trigger event
        EvtOnRemovedGrabbedObject?.Invoke(obj);
    }
}
