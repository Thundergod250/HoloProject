using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerGrab : MonoBehaviour
{
    public Transform PlayerGrabPoint;
    public bool IsPlayerCarryingObject;

    // === Events ===
    public UnityEvent<GameObject> EvtOnGrab;
    public UnityEvent<GameObject> EvtOnReleaseGrabObj;
    public UnityEvent<GameObject> EvtOnRemovedGrabbedObject;

    [SerializeField] private float tossForce = 5f;
    [SerializeField] private float grabDelay = 2f;

    private GameObject currentGrabbedObj;
    private bool isOnCooldown = false;

    public void GrabObject(GameObject obj)
    {
        if (isOnCooldown || IsPlayerCarryingObject || obj == null) return;

        // Parent to grab point immediately
        currentGrabbedObj = obj;
        obj.transform.SetParent(PlayerGrabPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // Disable physics + colliders while carried
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        Collider[] colliders = obj.GetComponents<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        IsPlayerCarryingObject = true;

        // Trigger event
        EvtOnGrab?.Invoke(obj);

        // Start cooldown
        StartCoroutine(ActionCooldown());
    }

    public void ReleaseGrabbedObject()
    {
        if (isOnCooldown || !IsPlayerCarryingObject || currentGrabbedObj == null) return;

        // Unparent immediately
        currentGrabbedObj.transform.SetParent(null);

        // Re-enable physics + colliders and toss forward
        Rigidbody rb = currentGrabbedObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;

            Vector3 releaseForce = transform.forward * tossForce;
            rb.AddForce(releaseForce, ForceMode.Impulse);
        }

        Collider[] colliders = currentGrabbedObj.GetComponents<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = true;
        }

        IsPlayerCarryingObject = false;

        // Trigger event
        EvtOnReleaseGrabObj?.Invoke(currentGrabbedObj);

        currentGrabbedObj = null;

        // Start cooldown
        StartCoroutine(ActionCooldown());
    }

    public void RemoveGrabObject()
    {
        if (!IsPlayerCarryingObject || currentGrabbedObj == null) return;

        // Just unparent immediately
        currentGrabbedObj.transform.SetParent(null);

        // Re-enable physics + colliders but no toss
        Rigidbody rb = currentGrabbedObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        Collider[] colliders = currentGrabbedObj.GetComponents<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = true;
        }

        IsPlayerCarryingObject = false;

        // Trigger event
        EvtOnRemovedGrabbedObject?.Invoke(currentGrabbedObj);

        currentGrabbedObj = null;

        // ❌ No cooldown here — always available
    }

    private IEnumerator ActionCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(grabDelay);
        isOnCooldown = false;
    }
}
