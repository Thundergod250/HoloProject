using UnityEngine;
using System.Threading.Tasks;

public class TowerChainPylon : TowerOffensiveBase
{
    [Header("Tower Settings")]
    public float attackRange = 10f;
    public float dominoRadius = 5f;
    public float fireRate = 2f;
    public float shotDelay = 0.2f;
    public int _damage = 5;
    public int _totalChainHits = 3;
    public LayerMask enemyLayer;

    [Header("Visual Settings")]
    public LineRenderer lineRenderer;
    public Transform firePoint;

    private float _nextFireTime;
    private GameObject[] _hitHistory = new GameObject[3];
    private bool _isDrawing;

    [SerializeField] private AudioSource _chainTowerAudioSource;

    void Update()
    {
        if (Time.time >= _nextFireTime)
        {
            ScanForPrimaryTarget();
        }

        // Update line positions every frame so they follow moving enemies
        if (_isDrawing && lineRenderer && lineRenderer.enabled)
        {
            UpdateLinePositions();
        }
    }

    private void UpdateLinePositions()
    {
        lineRenderer.SetPosition(0, firePoint.position);
        for (int i = 0; i < _hitHistory.Length; i++)
        {
            if (_hitHistory[i] != null)
            {
                lineRenderer.SetPosition(i + 1, _hitHistory[i].transform.position);
            }
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
            _isDrawing = true;
            if (lineRenderer)
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, firePoint.position);
            }

            for (int i = 0; i < _hitHistory.Length; i++) _hitHistory[i] = null;

            GameObject currentTarget = startEnemy;

            if (_chainTowerAudioSource != null)
            {
                _chainTowerAudioSource.Play();
            }


            for (int hitCount = 0; hitCount < _totalChainHits; hitCount++)
            {
                if (currentTarget == null) break;

                _hitHistory[hitCount] = currentTarget;

                if (lineRenderer)
                {
                    // Increment points: Point 0 is Tower, Point 1 is Enemy 1, etc.
                    lineRenderer.positionCount = hitCount + 2;
                }

                if (currentTarget.GetComponent<EnemyBase>())
                {
                    // Accessing health directly as requested
                    currentTarget.GetComponent<EnemyBase>()._healthReference.TakeDamage(_damage);
                }

                GameObject nextTarget = null;
                if (hitCount < _totalChainHits - 1)
                {
                    nextTarget = FindNextClosest(currentTarget.transform.position);
                }

                await Task.Delay((int)(shotDelay * 1000));
                currentTarget = nextTarget;
            }

            // Brief pause so the player sees the full completed chain
            await Task.Delay(100);
        }
        finally
        {
            _isDrawing = false;
            if (lineRenderer)
            {
                lineRenderer.enabled = false;
                lineRenderer.positionCount = 0;
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
