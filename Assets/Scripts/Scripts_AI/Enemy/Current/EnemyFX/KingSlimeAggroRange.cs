using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class KingSlimeAggroRange : MonoBehaviour
{
    public int debuffTimer;
    private List<GameObject> towersInRange = new List<GameObject>();


    private void Start()
    {
        StartCoroutine(DisableTowerInRange(10, debuffTimer));
    }
    private IEnumerator DisableTowerInRange(int duration, int disablePower)
    {
        while (true)
        {
            Debug.Log("running disable");

            yield return new WaitForSeconds(duration);

            if (towersInRange.Count == 0)
                continue;

            int randomIndex = Random.Range(0, towersInRange.Count);

            GameObject towerToDisable = towersInRange[randomIndex];

            if (towerToDisable != null)
            {
                towerToDisable.GetComponent<Tower_Offensive_SingleTarget>().DisableForSeconds(disablePower);
                Debug.Log("Disabled tower: " + towerToDisable.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Tower_Offensive_SingleTarget>() != null)
        {
            if (!towersInRange.Contains(other.gameObject))
            {
                towersInRange.Add(other.gameObject);
                Debug.Log("Tower added: " + other.gameObject.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove tower if it leaves the trigger
        if (other.GetComponent<Tower_Offensive_SingleTarget>() != null)
        {
            towersInRange.Remove(other.gameObject);
            Debug.Log("Tower removed: " + other.gameObject.name);
        }
    }
}
