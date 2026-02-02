using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject freeLookCamera;
    [SerializeField] private GameObject rtsCamera;
    [SerializeField] private RTSMouseSelector rtsMouseSelector;
    [SerializeField] PlayerMovement movement;
    bool playerCamActive = true;

    private void Start()
    {
        freeLookCamera.SetActive(true);
        rtsCamera.SetActive(false); 
    }

    public void EnableRTSCamera()
    {
        if (freeLookCamera != null)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            freeLookCamera.gameObject.SetActive(false);
            rtsCamera.gameObject.SetActive(true);
            rtsMouseSelector.enabled = true;
            movement.enabled = false;
        }
    }

    public void DisableRTSCamera()
    {
        if (freeLookCamera != null)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            freeLookCamera.gameObject.SetActive(true);
            rtsCamera.gameObject.SetActive(false);
            rtsMouseSelector.enabled = false;
            movement.enabled = true;
        }
    }
    
    public void BillboardToCamera(GameObject caller)
    {
        if (freeLookCamera == null || caller == null) return;

        // Get camera transform
        Transform camTransform = freeLookCamera.transform;

        // Rotate caller to face the camera
        Vector3 direction = camTransform.position - caller.transform.position;
        direction.y = 0f; // optional: keep billboard upright (ignore vertical tilt)

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            caller.transform.rotation = lookRotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            DisableRTSCamera();

            movement = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() && Input.GetKeyDown(KeyCode.E))
        {
            movement = other.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                if (playerCamActive)
                {
                    DisableRTSCamera();
                    playerCamActive = false;
                    movement.SetCanMove(true);
                }
                else if (!playerCamActive)
                {
                    EnableRTSCamera();
                    playerCamActive = true;
                    movement.SetCanMove(false);
                }
            }
        }
    }


}