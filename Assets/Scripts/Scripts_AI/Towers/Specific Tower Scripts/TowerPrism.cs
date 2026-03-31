using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerPrism : TowerOffensiveBase
{
    [Header("Settings")]
    public float connectionRange = 10f;
    public bool useMaxLimit = true; // Toggle this for the behavior you want
    public int maxConnections = 3;
    public int damageOnTouch = 15;
    public LayerMask towerLayer;
    public LayerMask obstructionMask;
    public GameObject connectionPrefab;
    public Transform firePoint;

    private List<TowerPrism> connectedTowers = new List<TowerPrism>();

    void Start()
    {
        StartCoroutine(ScanForTowers());
    }

    System.Collections.IEnumerator ScanForTowers()
    {
        while (true)
        {
            // If limit is OFF, we always scan. If ON, only scan if we have room.
            if (!useMaxLimit || connectedTowers.Count < maxConnections)
            {
                EstablishConnections();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void EstablishConnections()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, connectionRange, towerLayer);

        var sortedTowers = hitColliders
            .Select(hit => hit.GetComponent<TowerPrism>())
            .Where(t => t != null && t != this)
            .OrderBy(t => Vector3.Distance(transform.position, t.transform.position));

        foreach (var otherTower in sortedTowers)
        {
            // The Logic Split:
            if (useMaxLimit)
            {
                // If we are using the limit and we hit it, stop looking for more neighbors.
                if (connectedTowers.Count >= maxConnections) break;
            }

            if (!connectedTowers.Contains(otherTower))
            {
                if (HasLineOfSight(otherTower))
                {
                    CreateBeam(otherTower);
                    connectedTowers.Add(otherTower);
                    otherTower.RegisterExistingConnection(this);
                }
            }
        }
    }

    private bool HasLineOfSight(TowerPrism target)
    {
        Vector3 start = firePoint.position;
        Vector3 end = target.firePoint.position;
        Vector3 direction = end - start;
        float distance = Vector3.Distance(start, end);

        // Raycast logic that allows the "target" tower to be hit, 
        // but blocks if a different tower or wall is in between.
        if (Physics.Raycast(start, direction, out RaycastHit hit, distance, obstructionMask))
        {
            if (hit.collider.gameObject == target.gameObject)
            {
                return true; // Path is clear to the target
            }
            return false; // Path is blocked by something else
        }

        return true; // No obstructions hit at all
    }

    void CreateBeam(TowerPrism target)
    {
        GameObject beamObj = Instantiate(connectionPrefab, firePoint.position, Quaternion.identity);
        PrismConnection connection = beamObj.GetComponent<PrismConnection>();
        connection.Setup(this.firePoint, target.firePoint, this, target, connectionRange, damageOnTouch);
    }

    public void RegisterExistingConnection(TowerPrism tower)
    {
        if (!connectedTowers.Contains(tower)) connectedTowers.Add(tower);
    }

    public void RemoveConnection(TowerPrism tower)
    {
        if (connectedTowers.Contains(tower)) connectedTowers.Remove(tower);
    }
}
