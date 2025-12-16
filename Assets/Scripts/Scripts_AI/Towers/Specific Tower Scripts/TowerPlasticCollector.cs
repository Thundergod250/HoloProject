using System;
using UnityEngine;
using UnityEngine.Events;

public class TowerPlasticCollector : TowerUtilityFunction
{
    public UnityEvent EvtOnGarbageObjectCollide;
    public UnityEvent<GameObject> EvtPassObjectReference;

    private int playerMoney = 5; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GarbageObject>())
        {
            EvtOnGarbageObjectCollide?.Invoke();
            EvtPassObjectReference?.Invoke(other.gameObject);
        }
    }

    public void _DisableGarbageObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
