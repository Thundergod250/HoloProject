using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform[] raycastPoints;
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private UI_Interaction ui_interactionTab;

    private Interactable currentInteractable;
    private WaitForSeconds raycastInterval = new WaitForSeconds(0.1f);

    private void Start()
    {
        StartCoroutine(RaycastRoutine());
    }

    private IEnumerator RaycastRoutine()
    {
        while (true)
        {
            Interactable closest = null;
            float closestDistance = Mathf.Infinity;

            // 🔹 First try raycasts
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

            // 🔹 If no raycast found anything, fallback to OverlapSphere at player position
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

            // 🔹 Handle focus enter/exit
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
        if (!ctx.performed || currentInteractable == null)
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

        // 🔹 Draw overlap sphere for debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
