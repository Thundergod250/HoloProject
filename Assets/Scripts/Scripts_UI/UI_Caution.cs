using UnityEngine;
using UnityEngine.UI;

public class UI_Caution : MonoBehaviour
{
    [Header("Refs")]
    private Camera playerCamera;
    [SerializeField] private RawImage cautionImage;

    [Header("Pulse Settings")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.1f;

    private Vector3 originalScale;

    public bool hasWaveStart;

    private void Start()
    {
        playerCamera = Camera.main;
        originalScale = transform.localScale;
    }

    private void LateUpdate()
    {
        if (hasWaveStart) // Wave has started
        {
            cautionImage.enabled = false;
            Debug.Log("Caution image going off");
        }
        else if(!hasWaveStart) // Wave has not started 
        {
            cautionImage.enabled = true;

            float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            transform.localScale = originalScale + Vector3.one * pulse;

            if (playerCamera == null) return;

            Debug.Log("Caution image active");

            transform.LookAt(transform.position + playerCamera.transform.forward);
        }
    }
}
