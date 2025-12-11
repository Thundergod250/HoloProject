using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetectEntity : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float scanInterval = 1f;

    [SerializeField] private FactionType factionToDetect;

    private HashSet<GameObject> detectedEntities = new HashSet<GameObject>();
    private Coroutine scanCoroutine;

    private void Start()
    {
        scanCoroutine = StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        while (true)
        {
            if (this == null || gameObject == null) yield break;

            ScanForEntities();
            yield return new WaitForSeconds(scanInterval);
        }
    }

    private void ScanForEntities()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        HashSet<GameObject> currentFrameEntities = new HashSet<GameObject>();

        foreach (var hit in hits)
        {
            GameObject obj = hit.gameObject;
            if (IsMatchingFaction(obj))
            {
                currentFrameEntities.Add(obj);

                if (!detectedEntities.Contains(obj))
                {
                    detectedEntities.Add(obj);
                    Debug.Log($"Detected {factionToDetect}: {obj.name}");
                }
            }
        }

        // Remove entities that are no longer detected
        var entitiesToRemove = new List<GameObject>();
        foreach (var entity in detectedEntities)
        {
            if (!currentFrameEntities.Contains(entity))
            {
                entitiesToRemove.Add(entity);
            }
        }

        foreach (var entity in entitiesToRemove)
        {
            detectedEntities.Remove(entity);
            Debug.Log($"Lost {factionToDetect}: {entity.name}");
        }
    }

    private bool IsMatchingFaction(GameObject obj)
    {
        switch (factionToDetect)
        {
            case FactionType.Enemy:
                return obj.GetComponent<Identifier_Enemy>() != null;
            case FactionType.Player:
                return obj.GetComponent<Identifier_Player>() != null;
            case FactionType.Tower:
                return obj.GetComponent<Identifier_Tower>() != null;
            default:
                return false;
        }
    }

    public GameObject GetLatestDetected()
    {
        foreach (var entity in detectedEntities)
            return entity;
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnDestroy()
    {
        if (scanCoroutine != null)
        {
            StopCoroutine(scanCoroutine);
            scanCoroutine = null;
        }

        Debug.Log("DetectEntity destroyed");
    }
}
