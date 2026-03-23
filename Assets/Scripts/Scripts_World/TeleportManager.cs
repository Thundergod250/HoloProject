using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private List<PlayerTeleporter> _teleporters;
    [SerializeField] private GameObject _teleportUIFollowText;
    [SerializeField] private GameObject _teleportHUDUI;

    PlayerController playerRef;

    private bool _tpUIEnabled = false;

    private void Start()
    {
        _teleportHUDUI.SetActive(false);
        _teleportUIFollowText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
           playerRef = other.GetComponent<PlayerController>();

            ForceMouseEnable();

            _teleportHUDUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerRef = other.GetComponent<PlayerController>();

            ForceMouseDisable();

            _teleportHUDUI.SetActive(false);

            playerRef = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController playerRef = other.GetComponent<PlayerController>();

            _teleportHUDUI.transform.LookAt(playerRef.transform.position);

            if (Input.GetKeyDown(KeyCode.F) && playerRef != null)
            {
                if (!_tpUIEnabled)
                {
                    _teleportUIFollowText.SetActive(true);
                    _tpUIEnabled = true;
                }
                else if (_tpUIEnabled)
                {
                    _teleportUIFollowText.SetActive(false);
                    _tpUIEnabled = false;
                }
            }
        }
        else if (playerRef == null) 
        {
            _tpUIEnabled = false;
            _teleportUIFollowText.SetActive(false);
        }
    }

    public void ForceMouseDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ForceMouseEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void TeleportPlayerToTeleporterRef(int targetTPNumber)
    {
        _teleporters[targetTPNumber].TeleportPlayerHere();

        _tpUIEnabled = false;
        _teleportUIFollowText.SetActive(false);
        playerRef = null;

    }
}
