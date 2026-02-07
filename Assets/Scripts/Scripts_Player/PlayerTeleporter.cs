using UnityEngine;
using UnityEngine.UI;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private Button _playerTeleportMeButton;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerCamManager _playercamManager;
    [SerializeField] private Transform _spawnPoint;

    private void Update()
    {
        if (_playercamManager._freeCam)
        {
            _playerTeleportMeButton.gameObject.SetActive(false);
        }
        else if (!_playercamManager._freeCam)
        {
            _playerTeleportMeButton.gameObject.SetActive(true);
        }
    }

    public void TeleportPlayerHere()
    {
        _playercamManager.SwapCameraToRTSCamera(false);
        _playerController.gameObject.transform.position = _spawnPoint.transform.position;
    }
}
