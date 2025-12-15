using UnityEngine;
using System.Collections.Generic;

public class TowerBase : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _enemyTargets; // Need to change by Enemy Script

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GameObject>())
        {
            _enemyTargets.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GameObject>())
        {
            _enemyTargets.Remove(other.gameObject);
        }
    }
}
