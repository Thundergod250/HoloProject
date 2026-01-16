using UnityEngine;
using System.Collections.Generic;

public class TowerBigBase : TowerBase
{
    [SerializeField] List<GarbageObject.GarbageGroup> requiredGarbageGroups;
    [SerializeField] List<int> requiredNumberByGroup;
    [SerializeField] List<int> currentNumberByGroup;

    [SerializeField] List<bool> requiredCheckedType;
    [SerializeField] bool requireCompleted = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GarbageObject>())
        {
            Debug.Log("Trash: " + other.GetComponent<GarbageObject>().name);

            for (int i = 0; i < requiredGarbageGroups.Count; i++)
            {
                if (other.GetComponent<GarbageObject.GarbageGroup>() == requiredGarbageGroups[i])
                {
                    currentNumberByGroup[i]++;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!requireCompleted)
        {
            for (int i = 0; i < requiredNumberByGroup.Count; i++) 
            {
                if (requiredNumberByGroup[i] == currentNumberByGroup[i])
                {
                    requiredCheckedType[i] = true;
                }
            }

            foreach(bool fullfilled in requiredCheckedType)
            {
                if (!fullfilled) { requireCompleted = false; }
                else { requireCompleted = true; }
            }
        }
    }

}
