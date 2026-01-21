using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject freeLookCamera;
    [SerializeField] private GameObject RTSCamera;
    [SerializeField] PlayerController playerController;
    bool playerCamActive = true;

    private void Start()
    {
        freeLookCamera.SetActive(true);
        RTSCamera.SetActive(false); 
    }

    public void EnableCamera()
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.gameObject.SetActive(true);
            RTSCamera.gameObject.SetActive(false);
        }
    }

    public void DisableCamera()
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.gameObject.SetActive(false);
            RTSCamera.gameObject.SetActive(true);
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
        if (other.GetComponent<PlayerController>())
        {
            playerController = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerController = other.GetComponent<PlayerController>();

            if (playerController != null && Input.GetKeyDown(KeyCode.E))
            {
                if (playerCamActive)
                {
                    DisableCamera();
                    playerCamActive = false;
                }
                else if (!playerCamActive)
                {
                    EnableCamera();
                    playerCamActive = true;
                }
            }
        }
    }


}