using System.Collections.Generic;
using UnityEngine;

public class TowerPrism : TowerOffensiveBase
{
    public float connectionRange = 10f;
    public LayerMask towerLayer;
    public GameObject connectionPrefab;
    public Transform firePoint;

    private List<TowerPrism> connectedTowers = new List<TowerPrism>();

    void Start()
    {
        // Start the repeating scan instead of using Update
        StartCoroutine(ScanForTowers());
    }

    System.Collections.IEnumerator ScanForTowers()
    {
        while (true)
        {
            EstablishConnections();
            // Wait for 0.2 seconds (5 scans per second)
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void EstablishConnections()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, connectionRange, towerLayer);

        foreach (var hit in hitColliders)
        {
            if (hit.gameObject == this.gameObject) continue;

            TowerPrism otherTower = hit.GetComponent<TowerPrism>();

            // The list check here ensures we only spawn ONE beam per neighbor
            if (otherTower != null && !connectedTowers.Contains(otherTower))
            {
                CreateBeam(otherTower);
                connectedTowers.Add(otherTower);
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
