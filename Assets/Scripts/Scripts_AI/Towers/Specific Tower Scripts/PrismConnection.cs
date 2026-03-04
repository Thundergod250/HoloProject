using UnityEngine;

public class PrismConnection : MonoBehaviour
{
    [Header("Damage Settings")]
    public int instantDamage = 5;

    private LineRenderer line;
    private CapsuleCollider col;
    private Transform startPoint;
    private Transform endPoint;

    public void Setup(Transform start, Transform end)
    {
        line = GetComponent<LineRenderer>();
        col = GetComponent<CapsuleCollider>();

        startPoint = start;
        endPoint = end;

        // Essential: Make sure LineRenderer uses World Space
        line.useWorldSpace = true;
        line.positionCount = 2;

        col.isTrigger = true;
        // Direction 2 is the Z-axis. This is vital for the 'LookAt' logic.
        col.direction = 2;
    }

    void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        // 1. STRETCH THE VISUAL (LineRenderer)
        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);

        // 2. POSITION THE BEAM OBJECT
        // We move the beam's center to the middle point between towers
        Vector3 midpoint = (startPoint.position + endPoint.position) / 2f;
        transform.position = midpoint;

        // 3. ROTATE THE BEAM
        // We point the Z-axis of this object toward the target tower
        transform.LookAt(endPoint);

        // 4. STRETCH THE COLLIDER
        // Calculate the actual distance between towers
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // Set the height of the capsule to that distance
        col.height = distance;

        // Ensure the radius is thick enough to hit enemies (e.g., 0.2)
        col.radius = 0.2f;
    }

    // Triggered only once when the enemy touches the beam
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyBase>())
        {
            EnemyBase enemyTarget = other.GetComponent<EnemyBase>();

            enemyTarget.Health.TakeDamage(instantDamage);

            Debug.Log("Enemy target Prismed : " + enemyTarget.Health.GetCurrentHealth());
        }
    }
}
