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

        // 1. Check Range (Existing logic)
        if (currentDistance > rangeLimit)
        {
            BreakConnection();
            return;
        }

        // 2. Check for new Obstructions
        if (IsObstructed())
        {
            BreakConnection();
            return;
        }

        // ... (Keep existing Stretching/Rotation logic)
    }

    private bool IsObstructed()
    {
        Vector3 direction = endPoint.position - startPoint.position;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // We use a Raycast to see if something on the obstructionMask is now in the way
        // Note: We use a LayerMask here so the beam doesn't "hit" itself!
        // Set your obstructionMask in Setup() or make it public
        LayerMask mask = sourceTower.obstructionMask;

        if (Physics.Raycast(startPoint.position, direction, out RaycastHit hit, distance, mask))
        {
            return true; // Path is blocked
        }
        return false;
    }

    private void BreakConnection()
    {
        sourceTower.RemoveConnection(targetTower);
        targetTower.RemoveConnection(sourceTower);
        Destroy(gameObject);
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
