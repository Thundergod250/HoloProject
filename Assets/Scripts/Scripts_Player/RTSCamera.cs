using UnityEngine;
using Unity.Cinemachine;
using TMPro;

public class RTSCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 20f;
    public float scrollSpeed = 10f;
    public Vector2 screenEdgeBuffer = new Vector2(10, 10);
    [SerializeField] private bool _allowPanning = false;

    [Header("Boundary Settings")]
    // Defines the rectangle (Min X, Min Z) to (Max X, Max Z)
    public Vector2 minBounds = new Vector2(-50, -50);
    public Vector2 maxBounds = new Vector2(50, 50);

    [Header("Camera References")]
    [SerializeField] public CinemachineCamera cmCamera;
    [SerializeField] public TextMeshProUGUI fovCMText;
    [SerializeField] private Transform _playerGameObject;

    [Header("Zoom Settings")]
    public float minSize = 5f;
    public float maxSize = 20f;
    public float fovMinSize = 35f;
    public float fovMaxSize = 60f;

    [SerializeField] private bool _enableZoom = false;

    private void OnEnable()
    {
        // Safety check: Ensure the player exists before trying to access position
        if (_playerGameObject != null)
        {
            transform.position = new Vector3(_playerGameObject.position.x, transform.position.y, _playerGameObject.position.z);
        }
    }

    private void Update()
    {
        Move();
        BruteForceShowMouse();
        HandleZoom();
        UpdateUI();
    }

    private void BruteForceShowMouse()
    {
        if (this.gameObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void Move()
    {
        if (_allowPanning)
        {
            // Get standard Input
            float xInput = Input.GetAxis("Horizontal");
            float zInput = Input.GetAxis("Vertical");

            // Add Edge Panning
            if (Input.mousePosition.x >= Screen.width - screenEdgeBuffer.x) xInput += 1;
            if (Input.mousePosition.x <= screenEdgeBuffer.x) xInput -= 1;
            if (Input.mousePosition.y >= Screen.height - screenEdgeBuffer.y) zInput += 1;
            if (Input.mousePosition.y <= screenEdgeBuffer.y) zInput -= 1;

            // Calculate movement
            Vector3 moveDir = new Vector3(xInput, 0, zInput).normalized;
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

            // STAY IN THE BOX
            ClampPosition();
        }
    }

    private void ClampPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedZ = Mathf.Clamp(transform.position.z, minBounds.y, maxBounds.y);

        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    public void EnablePanAndZoom()
    {
        _enableZoom = true;
        _allowPanning = true;
    }


    private void HandleZoom()
    {
        if (_enableZoom)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll == 0) return;

            if (cmCamera.Lens.ModeOverride == LensSettings.OverrideModes.Orthographic)
            {
                float newSize = cmCamera.Lens.OrthographicSize - (scroll * scrollSpeed);
                cmCamera.Lens.OrthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
            }
            else
            {
                float newPOV = cmCamera.Lens.FieldOfView - (scroll * scrollSpeed);
                cmCamera.Lens.FieldOfView = Mathf.Clamp(newPOV, fovMinSize, fovMaxSize);
            }
        }
    }

    private void UpdateUI()
    {
        if (fovCMText != null)
        {
            // Show either Ortho Size or FOV depending on mode
            float currentVal = (cmCamera.Lens.ModeOverride == LensSettings.OverrideModes.Orthographic)
                ? cmCamera.Lens.OrthographicSize
                : cmCamera.Lens.FieldOfView;

            fovCMText.text = currentVal.ToString("F1"); // "F1" rounds to 1 decimal place
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((minBounds.x + maxBounds.x) / 2, transform.position.y, (minBounds.y + maxBounds.y) / 2);
        Vector3 size = new Vector3(maxBounds.x - minBounds.x, 1, maxBounds.y - minBounds.y);
        Gizmos.DrawWireCube(center, size);
    }
}
