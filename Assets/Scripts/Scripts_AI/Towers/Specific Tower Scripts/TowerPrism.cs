using System.Collections.Generic;
using UnityEngine;

public class TowerPrism : MonoBehaviour
{
    [Header("Settings")]
    public float connectionRange = 10f;
    public LayerMask towerLayer;       // The layer your towers are on
    public LayerMask obstructionMask;  // The layer your walls/buildings are on
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
            EstablishConnections();
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

            if (otherTower != null && !connectedTowers.Contains(otherTower))
            {
                // Check if the path is clear before connecting
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
        Vector3 direction = target.firePoint.position - firePoint.position;
        float distance = Vector3.Distance(firePoint.position, target.firePoint.position);

        // Raycast from our firepoint to their firepoint
        // We check against both the obstructionMask AND the towerLayer
        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, distance, obstructionMask | towerLayer))
        {
            // If the first thing we hit is the other tower, the path is clear
            if (hit.collider.gameObject == target.gameObject)
            {
                return true;
            }
        }

        // Something else (like a wall) was in the way
        return false;
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
