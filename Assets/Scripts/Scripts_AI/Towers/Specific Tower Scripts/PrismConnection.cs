using UnityEngine;

public class PrismConnection : MonoBehaviour
{
    private LineRenderer line;
    private BoxCollider col;
    private Transform startPoint;
    private Transform endPoint;
    private TowerPrism sourceTower;
    private TowerPrism targetTower;
    private int damageBeam;
    private float rangeLimit;

    public void Setup(Transform start, Transform end, TowerPrism source, TowerPrism target, float range, int damage)
    {
        line = GetComponent<LineRenderer>();
        col = GetComponent<BoxCollider>();

        startPoint = start;
        endPoint = end;
        sourceTower = source;
        targetTower = target;
        rangeLimit = range;
        damageBeam = damage;

        line.positionCount = 2;
        line.useWorldSpace = true;
        col.isTrigger = true;
        col.direction = 2; // Z-Axis
    }

    void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        float currentDistance = Vector3.Distance(startPoint.position, endPoint.position);

        // The beam only snaps if the player moves the tower out of range
        if (currentDistance > rangeLimit)
        {
            sourceTower.RemoveConnection(targetTower);
            targetTower.RemoveConnection(sourceTower);
            Destroy(gameObject);
            return;
        }

        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);

        transform.position = (startPoint.position + endPoint.position) / 2f;
        transform.LookAt(endPoint);
        col.height = currentDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<EnemyBase>())
        {
            Health healthTarget = other.GetComponentInChildren<EnemyBase>()._healthReference;

            healthTarget.TakeDamage(damageBeam);
        }

        Debug.Log("Prism Beam hit: " + other.name);
    }
}
