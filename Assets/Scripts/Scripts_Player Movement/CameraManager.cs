using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject freeLookCamera;

    public void EnableCamera()
    {
        if (freeLookCamera != null)
            freeLookCamera.gameObject.SetActive(true);
    }

    public void DisableCamera()
    {
        if (freeLookCamera != null)
            freeLookCamera.gameObject.SetActive(false);
    }
}