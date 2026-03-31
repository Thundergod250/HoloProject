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

        // Ensure collider is a trigger
        col.isTrigger = true;

        // Reset the center of the box to 0 to ensure it scales outward from the middle
        col.center = Vector3.zero;
    }

    void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        float currentDistance = Vector3.Distance(startPoint.position, endPoint.position);

        if (currentDistance > rangeLimit)
        {
            sourceTower.RemoveConnection(targetTower);
            targetTower.RemoveConnection(sourceTower);
            Destroy(gameObject);
            return;
        }

        // 1. Update Line Visuals
        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);

        // 2. Position the object in the exact center of the two towers
        transform.position = (startPoint.position + endPoint.position) / 2f;

        // 3. Rotate to look at the target tower (Z-axis points forward)
        transform.LookAt(endPoint);

        // 4. Update the Box Collider Size
        // We keep X and Y small (the thickness of the beam) and scale Z to the distance
        col.size = new Vector3(0.5f, 0.5f, currentDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Try to find the Enemy component
        EnemyBase enemy = other.GetComponentInChildren<EnemyBase>();

        if (enemy != null)
        {
            Health healthTarget = enemy._healthReference;
            if (healthTarget != null)
            {
                healthTarget.TakeDamage(damageBeam);
                Debug.Log($"Prism Beam hit {other.name} for {damageBeam} damage!");
            }
        }
    }
}
