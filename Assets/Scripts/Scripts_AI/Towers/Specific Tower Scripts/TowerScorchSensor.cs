using UnityEngine;

public class TowerScorchSensor : MonoBehaviour
{
    // Reference to the main tower script
    public TowerScorchRay rayController;
    public BoxCollider myCollider;

    private void Start()
    {
        // Auto-assign if you forgot to drag it in the inspector
        if (myCollider == null)
        {
            myCollider = GetComponent<BoxCollider>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (rayController != null)
        {
            rayController.OnEnemyEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (rayController != null)
        {
            rayController.OnEnemyExit(other);
        }
    }
}
