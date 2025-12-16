using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform[] raycastPoints;
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private UI_Interaction ui_interactionTab;
    [SerializeField] private float enableDelay = 0.1f;   // ⏱ delay before enabling

    private Interactable currentInteractable;
    private WaitForSeconds raycastInterval = new WaitForSeconds(0.1f);
    private Coroutine raycastRoutine;
    private Coroutine enableRoutine;

    private void OnEnable()
    {
        // start delayed enable routine
        if (enableRoutine == null)
            enableRoutine = StartCoroutine(EnableWithDelay());
    }

    private void OnDisable()
    {
        if (raycastRoutine != null)
        {
            StopCoroutine(raycastRoutine);
            raycastRoutine = null;
        }

        if (enableRoutine != null)
        {
            StopCoroutine(enableRoutine);
            enableRoutine = null;
        }
    }

    private IEnumerator EnableWithDelay()
    {
        yield return new WaitForSeconds(enableDelay);

        if (raycastRoutine == null)
            raycastRoutine = StartCoroutine(RaycastRoutine());

        enableRoutine = null; // clear reference
    }

    private IEnumerator RaycastRoutine()
    {
        while (true)
        {
            Interactable closest = null;
            float closestDistance = Mathf.Infinity;

            foreach (Transform point in raycastPoints)
            {
                if (Physics.Raycast(point.position, point.forward, out RaycastHit hit, rayLength, interactableMask))
                {
                    var interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        float dist = Vector3.Distance(transform.position, hit.point);
                        if (dist < closestDistance)
                        {
                            closest = interactable;
                            closestDistance = dist;
                        }
                    }
                }
            }

            if (closest == null)
            {
                Collider[] overlaps = Physics.OverlapSphere(transform.position, 1f, interactableMask);
                foreach (var col in overlaps)
                {
                    var interactable = col.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        float dist = Vector3.Distance(transform.position, col.transform.position);
                        if (dist < closestDistance)
                        {
                            closest = interactable;
                            closestDistance = dist;
                        }
                    }
                }
            }

            if (closest != currentInteractable)
            {
                if (currentInteractable != null)
                {
                    currentInteractable.FocusExit();
                    ui_interactionTab.Hide();
                }

                currentInteractable = closest;
                if (currentInteractable != null)
                {
                    currentInteractable.Focus();
                    ui_interactionTab.Show(currentInteractable.interactName);
                }
            }
            else if (currentInteractable != null)
            {
                currentInteractable.Focus();
            }

            yield return raycastInterval;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!enabled || !ctx.performed || currentInteractable == null)
            return;

        currentInteractable.Interact();
        currentInteractable = null;
        ui_interactionTab.Hide();
    }

    private void OnDrawGizmosSelected()
    {
        if (raycastPoints == null) return;

        Gizmos.color = Color.cyan;
        foreach (Transform point in raycastPoints)
        {
            if (point != null)
                Gizmos.DrawRay(point.position, point.forward * rayLength);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
