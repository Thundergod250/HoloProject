using UnityEngine;

public class DisableMovement : MonoBehaviour
{
    // === Player Movement Control ===
    public void DisablePlayerMovement() => GameManager.Instance.PlayerController?.DisableMovement();

    public void EnablePlayerMovement() => GameManager.Instance.PlayerController?.EnableMovement();

    // === Camera Control ===
    public void DisableCameraMovement() => GameManager.Instance.CameraManager?.DisableCamera();

    public void EnableCameraMovement() => GameManager.Instance.CameraManager?.EnableCamera();
}