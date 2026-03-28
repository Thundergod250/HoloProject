using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerPrism : TowerOffensiveBase
{
    public float connectionRange = 10f;
    public int maxConnections = 3;
    public LayerMask towerLayer;
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
            if (connectedTowers.Count < maxConnections)
            {
                EstablishConnections();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void EstablishConnections()
    {
        // 1. Get all nearby colliders
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, connectionRange, towerLayer);

        // 2. Sort them by distance (Nearest to Furthest)
        var sortedTowers = hitColliders
            .Where(hit => hit.gameObject != this.gameObject) // Skip self
            .OrderBy(hit => Vector3.Distance(transform.position, hit.transform.position)) // Sort by distance
            .ToList();

        foreach (var hit in sortedTowers)
        {
            // If we filled our 3 slots during this loop, stop looking
            if (connectedTowers.Count >= maxConnections) break;

            TowerPrism otherTower = hit.GetComponent<TowerPrism>();

            // Check if we aren't already linked and if they have room
            if (otherTower != null && !connectedTowers.Contains(otherTower))
            {
                if (otherTower.connectedTowers.Count < otherTower.maxConnections)
                {
                    CreateBeam(otherTower);
                    connectedTowers.Add(otherTower);
                    otherTower.RegisterExistingConnection(this);
                }
            }
        }
    }

    void CreateBeam(TowerPrism target)
    {
        GameObject beamObj = Instantiate(connectionPrefab, firePoint.position, Quaternion.identity);
        PrismConnection connection = beamObj.GetComponent<PrismConnection>();
        connection.Setup(this.firePoint, target.firePoint, this, target, connectionRange);
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
