using UnityEngine;

public class TowerController : MonoBehaviour
{
    [HideInInspector] public GameObject towerPrefab;   // prefab used to spawn this tower
    [HideInInspector] public GameObject towerInstance; // actual GameObject (this one)

    public Health TowerHealth;
    
    private void Awake()
    {
        towerInstance = gameObject;
    }
}