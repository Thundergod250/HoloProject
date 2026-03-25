using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerDeathNotifier : MonoBehaviour
{
    [Header("Spawners")]
    [SerializeField] private List<GameObject> spawners = new List<GameObject>();
    [SerializeField] private Health health;

    private void Start()
    {
        spawners.AddRange(GameManager.Instance.spawnerObjects);
    }

    public void RelayDeath()
    {
        Debug.Log(health.GetCurrentHealth() + "Tower Health");
        if(health.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            Debug.Log("tower Died notified enemes");
            foreach(GameObject s in spawners)
            {
                s.GetComponent<Spawner>().RemoveTowerFromList(this.gameObject);
            }
        }
        
    }
}
