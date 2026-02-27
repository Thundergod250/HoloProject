using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraManager _camSwitcher;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GameObject _playerUITarget;

    [Header("Checkers")]
    [SerializeField] public bool _canSwitchCameras = true;

    public bool _freeCam = false;

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
            _playerUITarget.SetActive(false);
            _camSwitcher.SetCameraChange();
            _camSwitcher.movement = _playerMovement;

            _freeCam = false;
        }
        else if (!changedToRTS)
        {
            _playerUITarget.SetActive(true);
            _camSwitcher.SetCameraChange();
            _camSwitcher.movement = null;

            _freeCam = true;
        }
    }
}
