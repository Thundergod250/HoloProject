using UnityEngine;

public class TowerNodeHeapChecker : MonoBehaviour
{
    [SerializeField]private TrashHeap_ResourceSpawner _trashHeapOnThisNode;
    [SerializeField] private Interactable _towerInteractable;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<TrashHeap_ResourceSpawner>())
        {
            _trashHeapOnThisNode = other.GetComponent<TrashHeap_ResourceSpawner>();
            _towerInteractable._DisableInteraction();
        }
        else if(_trashHeapOnThisNode == null)
        {
            _towerInteractable._EnableInteraction();
        }
    }
}
