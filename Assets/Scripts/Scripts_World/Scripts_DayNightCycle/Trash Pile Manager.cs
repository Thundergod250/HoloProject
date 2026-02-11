using UnityEngine;
using System.Collections.Generic;
public class TrashPileManager : MonoBehaviour
{
    [SerializeField] protected List<TrashHeap_ResourceSpawner> _trashHeaps;
    [SerializeField] protected LightingManager _lightingManager;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        CheckTime();
    }

    private void ResetEnableAllHeaps()
    {
        for (int i = 0; i < _trashHeaps.Count; i++)
        {
            _trashHeaps[i]?.gameObject.SetActive(true);
            _trashHeaps[i]?.gameObject.GetComponent<Health>().ReviveHealth();

            _trashHeaps[i]?.gameObject.GetComponent<TrashHeap_ResourceSpawner>().ResetBool();

            if (_trashHeaps[i]?.GetComponent<Health>().GetCurrentHealth() <= 0)
            {
                _trashHeaps[i].GetComponent<Health>().Heal(10);
            }
        }
        Debug.Log("Enabled All Heaps");
    }

    private void CheckTime()
    {
        if (_lightingManager?.GetTimeOfDay() >= 240 && _lightingManager?.GetTimeOfDay() <= 241)
        {
            ResetEnableAllHeaps();
        }
    }
}
