using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private List<PlayerTeleporter> _teleporters;
    [SerializeField] private GameObject _teleportUI;
    [SerializeField] private GameObject _teleportTextUI;
    private bool _tpUIEnabled = false;

    private void Start()
    {
        _teleportTextUI.SetActive(false);
        _teleportUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _teleportTextUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _teleportTextUI.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController playerRef = other.GetComponent<PlayerController>();

            _teleportTextUI.transform.LookAt(playerRef.transform.position);

            if (Input.GetKeyDown(KeyCode.Tab) && playerRef != null)
            {
                if (!_tpUIEnabled)
                {
                    _teleportUI.SetActive(true);
                }
                else if (_tpUIEnabled)
                {
                    _teleportUI.SetActive(false);
                }
            }
        }
    }

    public void TeleportPlayerToTeleporterRef(int targetTPNumber)
    {
        _teleporters[targetTPNumber].TeleportPlayerHere();
    }
}
