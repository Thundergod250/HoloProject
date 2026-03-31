using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerDeathNotifier : MonoBehaviour
{
    [Header("Spawners")]
    [SerializeField] private List<GameObject> spawners = new List<GameObject>();
    [SerializeField] private Health health;
    [SerializeField] private GameObject bOSS;

    private void Start()
    {
        spawners.AddRange(GameManager.Instance.spawnerObjects);
        bOSS = ReclamationManager.Instance.bossRef;
    }

    public void RelayDeath()
    {
        if(health.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            Debug.Log("tower Died notified enemes");
            foreach(GameObject s in spawners)
            {
                s.GetComponent<Spawner>().RemoveTowerFromList(this.gameObject);
            }

            if(bOSS != null && bOSS.activeSelf)
            {
                bOSS.GetComponent<Navigation_Enemy>().TargetHasDied(this.gameObject);
            }
        }
        
    }
}
