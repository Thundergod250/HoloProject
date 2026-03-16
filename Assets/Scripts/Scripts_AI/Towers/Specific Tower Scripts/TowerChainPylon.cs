using UnityEngine;
using System.Threading.Tasks;

public class TowerChainPylon : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 10f;
    public float dominoRadius = 5f;
    public float fireRate = 2f;
    public float shotDelay = 0.2f;
    public int _damage = 5;
    public LayerMask enemyLayer;

    [Header("Visual Settings")]
    public LineRenderer lineRenderer;
    public Transform firePoint;

    private float _nextFireTime;
    private GameObject[] _hitHistory = new GameObject[3];

    void Update()
    {
        if (Time.time >= _nextFireTime)
        {
            ScanForPrimaryTarget();
        }
    }

    private void ScanForPrimaryTarget()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            _nextFireTime = Time.time + fireRate;
            _ = ExecuteDominoChainAsync(enemiesInRange[0].gameObject);
        }
    }

    private async Task ExecuteDominoChainAsync(GameObject startEnemy)
    {
        try
        {
            if (lineRenderer)
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, firePoint.position);
            }

            for (int i = 0; i < _hitHistory.Length; i++) _hitHistory[i] = null;

            GameObject currentTarget = startEnemy;

            for (int hitCount = 0; hitCount < 3; hitCount++)
            {
                if (currentTarget == null) break;

                _hitHistory[hitCount] = currentTarget;

                if (lineRenderer)
                {
                    lineRenderer.positionCount = hitCount + 2;
                    lineRenderer.SetPosition(hitCount + 1, currentTarget.transform.position);
                }

                if (currentTarget.GetComponent<EnemyBase>().Health)
                {
                    currentTarget.GetComponent<EnemyBase>().Health.TakeDamage(_damage);
                }

                GameObject nextTarget = null;
                if (hitCount < 2)
                {
                    nextTarget = FindNextClosest(currentTarget.transform.position);
                }

                await Task.Delay((int)(shotDelay * 1000));
                currentTarget = nextTarget;
            }
        }
        finally
        {
            // This block runs no matter what, ensuring the line always resets
            if (lineRenderer)
            {
                lineRenderer.positionCount = 0;
                lineRenderer.enabled = false;
            }
        }
    }

    private GameObject FindNextClosest(Vector3 currentPos)
    {
        Collider[] candidates = Physics.OverlapSphere(currentPos, dominoRadius, enemyLayer);

        GameObject bestTarget = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < candidates.Length; i++)
        {
            GameObject candidateObj = candidates[i].gameObject;

            if (AlreadyInChain(candidateObj)) continue;

            float dist = (currentPos - candidateObj.transform.position).sqrMagnitude;
            if (dist < closestDistance)
            {
                closestDistance = dist;
                bestTarget = candidateObj;
            }
        }
        return bestTarget;
    }

    private bool AlreadyInChain(GameObject obj)
    {
        for (int i = 0; i < _hitHistory.Length; i++)
        {
            if (_hitHistory[i] == obj) return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
