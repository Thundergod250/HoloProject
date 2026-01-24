using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class RTSCamera : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float scrollSpeed = 10f;
    public Vector2 screenEdgeBuffer = new Vector2(10, 10); // For edge-panning

    // For Isometric Zoom (Orthographic Size)
    [SerializeField] public CinemachineCamera cmCamera;
    public float minSize = 5f;
    public float maxSize = 20f;

    public float fovMinSize = 35f;
    public float fovMaxSize = 60f;

    private void Update()
    {
        Move();
        HandleZoom();
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Optional: Mouse Edge Panning
        if (Input.mousePosition.x >= Screen.width - screenEdgeBuffer.x) moveDir.x += 1;
        if (Input.mousePosition.x <= screenEdgeBuffer.x) moveDir.x -= 1;
        if (Input.mousePosition.y >= Screen.height - screenEdgeBuffer.y) moveDir.z += 1;
        if (Input.mousePosition.y <= screenEdgeBuffer.y) moveDir.z -= 1;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0) return;

        // Correct Cinemachine 3.x API check
        if (cmCamera.Lens.ModeOverride == LensSettings.OverrideModes.Orthographic)
        {
            float newSize = cmCamera.Lens.OrthographicSize - (scroll * scrollSpeed);
            cmCamera.Lens.OrthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
        }
        else
        {
            // Fallback for Perspective Overview
            float newFOV = cmCamera.Lens.FieldOfView - (scroll * scrollSpeed);
            cmCamera.Lens.FieldOfView = Mathf.Clamp(newFOV, fovMinSize, fovMaxSize);
        }
    }
}
