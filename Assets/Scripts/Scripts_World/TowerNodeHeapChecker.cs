using UnityEngine;

public class TowerNodeHeapChecker : MonoBehaviour
{
    [SerializeField]private TrashHeap_ResourceSpawner _trashHeapOnThisNode;
    [SerializeField] private Interactable _towerInteractable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TrashHeap_ResourceSpawner>())
        {
            _trashHeapOnThisNode = other.GetComponent<TrashHeap_ResourceSpawner>();
            Debug.Log("Found Heap");
            _towerInteractable._DisableInteraction();
        }
        else 
        {
            _towerInteractable._EnableInteraction();
        }
    }
}
