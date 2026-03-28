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

    // This list tracks who WE have initiated a beam to
    private List<TowerPrism> connectedTowers = new List<TowerPrism>();

    void Start()
    {
        StartCoroutine(ScanForTowers());
    }

    System.Collections.IEnumerator ScanForTowers()
    {
        while (true)
        {
            // Only search if WE have room for more beams
            if (connectedTowers.Count < maxConnections)
            {
                EstablishConnections();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void EstablishConnections()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, connectionRange, towerLayer);

        // Sort by distance so we grab the 3 closest neighbors first
        var sortedTowers = hitColliders
            .Select(hit => hit.GetComponent<TowerPrism>())
            .Where(t => t != null && t != this)
            .OrderBy(t => Vector3.Distance(transform.position, t.transform.position));

        foreach (var otherTower in sortedTowers)
        {
            // STOP if this tower has reached its personal limit of 3
            if (connectedTowers.Count >= maxConnections) break;

            // Only connect if we haven't already sent a beam to this specific neighbor
            if (!connectedTowers.Contains(otherTower))
            {
                CreateBeam(otherTower);
                connectedTowers.Add(otherTower);

                // We still notify them so they know we are linked, 
                // but we don't check THEIR limit anymore.
                otherTower.RegisterExistingConnection(this);
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
