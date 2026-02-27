using UnityEngine;
using UnityEngine.UI;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private Button _playerTeleportMeButton;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerCamManager _playercamManager;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] public bool _isActive = false;

    public void SetTeleporterButton(bool targetActive)
    {
        if (targetActive == true)
        {
            _playerTeleportMeButton.interactable = true;
        }
        else if (targetActive == false)
        {
            _playerTeleportMeButton.interactable = false;
        }
    }

public void TeleportPlayerHere()
    {
        _playercamManager.SwapCameraToRTSCamera(true);
        _playerController.gameObject.transform.position = _spawnPoint.transform.position;
    }
}
