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
    public bool _buildMenu = false;

    private void Update()
    {
        if (_canSwitchCameras)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwapCameraToRTSCamera(_freeCam);
            }

            if (Input.GetKeyDown(KeyCode.B) && _freeCam) 
            {
                SwapBuildMenu(_buildMenu);
            }

        }
    }

    public void SwapCameraToRTSCamera(bool changedToRTS)
    {
        Debug.Log("Tab Triggered: " + changedToRTS);

        if (changedToRTS)
        {
            _camSwitcher.SetCameraChange();
            //_camSwitcher.movement = null;//_playerMovement;

            _freeCam = false;
        }
        else if (!changedToRTS)
        {
            _camSwitcher.SetCameraChange();
            //_camSwitcher.movement = _playerMovement;

            _freeCam = true;
        }
    }

    public void SwapBuildMenu(bool rtsCamEnabled)
    {
        if (rtsCamEnabled)
        {
            _playerUITarget.SetActive(true);
            _buildMenu = true;
        }
        else if (!rtsCamEnabled)
        {
            _playerUITarget.SetActive(false);
            _buildMenu = false;
        }
    }

}
