using UnityEngine;
using System.Collections;
public class HitFlash : MonoBehaviour
{
    [Header("Hit Flash Objects")]
    [SerializeField] private GameObject flashModel; // The red version of your model
    [SerializeField] private GameObject flashLight; // The Red Point Light
    [SerializeField] private float flashDuration = 0.5f;

    private Coroutine hitCoroutine;

    void Start()
    {
        // Ensure both start OFF
        if (flashModel != null) flashModel.SetActive(false);
        if (flashLight != null) flashLight.SetActive(false);
    }

    // --- CALL THIS ON TAKE DAMAGE ---
    public void PlayHitEffect()
    {
        // Safety check to ensure we don't error if object is already being destroyed
        if (!gameObject.activeInHierarchy) return;

        else if (hitCoroutine != null) StopCoroutine(hitCoroutine);
        hitCoroutine = StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        // Turn BOTH on
        flashModel.SetActive(true);
        flashLight.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        // Turn BOTH off
        flashModel.SetActive(false);
        flashLight.SetActive(false);
    }

    // --- CALL THIS ON DEATH ---
    public void DieVFXEffect()
    {
        StopAllCoroutines();

        // Final cleanup to make sure no red sticks around
        flashModel.SetActive(false);
        flashLight.SetActive(false);

        // Since your other script handles Destroy/Deactivation, 
        // we just stop the visuals here.
    }
}
