using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraManager _camSwitcher;
    [SerializeField] private PlayerMovement _playerMovement;

    [Header("Checkers")]
    [SerializeField] public bool _canSwitchCameras = true;
    public bool _freeCam = true;

    private void LateUpdate()
    {
        if (_canSwitchCameras)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwapCameraToRTSCamera(_freeCam);
            }
        }
    }

    public void SwapCameraToRTSCamera(bool changedToRTS)
    {
        Debug.Log("Tab Triggered: " + changedToRTS);

        if (changedToRTS)
        {
            _camSwitcher.SetCameraChange();
            _camSwitcher.movement = _playerMovement;

            _freeCam = false;
        }
        else if (!changedToRTS)
        {
            _camSwitcher.SetCameraChange();
            _camSwitcher.movement = null;

            _freeCam = true;
        }
    }
}
