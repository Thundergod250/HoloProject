using UnityEngine;

public class PrismConnection : MonoBehaviour
{
    private LineRenderer line;
    private CapsuleCollider col;
    private Transform startPoint;
    private Transform endPoint;
    private TowerPrism sourceTower;
    private TowerPrism targetTower;
    private float rangeLimit;

    public int instantDamage = 10;

    public void Setup(Transform start, Transform end, TowerPrism source, TowerPrism target, float range)
    {
        line = GetComponent<LineRenderer>();
        col = GetComponent<CapsuleCollider>();

        startPoint = start;
        endPoint = end;
        sourceTower = source;
        targetTower = target;
        rangeLimit = range; // Received from the Tower

        line.useWorldSpace = true;
        line.positionCount = 2;
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

        // Break if we exceed the range passed from the Tower
        if (currentDistance > rangeLimit)
        {
            sourceTower.RemoveConnection(targetTower);
            targetTower.RemoveConnection(sourceTower);
            Destroy(gameObject);
            return;
        }

        // Visual Stretch
        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);

        // Physical Stretch
        transform.position = (startPoint.position + endPoint.position) / 2f;
        transform.LookAt(endPoint);
        col.height = currentDistance;
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
