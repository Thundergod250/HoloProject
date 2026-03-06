using System.Collections.Generic;
using UnityEngine;

public class TowerPrism : MonoBehaviour
{
    [Header("Settings")]
    public float connectionRange = 50f;
    public Transform firePoint;
    public LayerMask towerLayer; // Set this to the layer your towers are on
    public GameObject connectionPrefab;

    // Keeps track of who we are already linked to
    private List<TowerPrism> connectedTowers = new List<TowerPrism>();

    void Start()
    {
        EstablishConnections();
    }

    void EstablishConnections()
    {
        // Physics.OverlapSphere only detects colliders within 'connectionRange'
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, connectionRange, towerLayer);

        foreach (var hit in hitColliders)
        {
            // 1. If the collider we hit is our own, skip it immediately
            if (hit.gameObject == this.gameObject) continue;

            // 2. Now we check if what we hit actually has the script
            TowerPrism otherTower = hit.GetComponent<TowerPrism>();

            // 3. Check for the script and existing connections
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
        GameObject beam = Instantiate(connectionPrefab, firePoint.position, Quaternion.identity);
        PrismConnection connection = beam.GetComponent<PrismConnection>();

        // Pass the firePoints, not the base of the towers
        connection.Setup(this.firePoint, target.firePoint);
    }

    public void RegisterExistingConnection(TowerPrism tower)
    {
        if (!connectedTowers.Contains(tower))
        {
            connectedTowers.Add(tower);
        }
    }

    // Visual aid in the editor to see the connection range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, connectionRange);
    }
}
