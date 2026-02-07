using UnityEngine;

public class PlayerTPTrigger : MonoBehaviour
{
    [SerializeField] Transform _spawnPoint;
    [SerializeField] PlayerController _playerController;
    [SerializeField] GarbageObject _garbageObject;

    [SerializeField] GameObject _hideUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (_hideUI != null)
            {
                _hideUI?.SetActive(true);
            }
            _playerController = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>()) 
        {
            if (_hideUI != null)
            {
                _hideUI?.SetActive(true);
                _playerController.gameObject.transform.position = _spawnPoint.position;
                _hideUI?.SetActive(false);
            }
            else
            {
                _playerController.gameObject.transform.position = _spawnPoint.position;
            }
        }
        else if (other.GetComponent<GarbageObject>())
        {
            _garbageObject = other.GetComponent<GarbageObject>();
            //_garbageObject.transform.position = _playerController.gameObject.transform.position;
             _garbageObject.transform.position = _spawnPoint.position;
        }
        
    }
}
