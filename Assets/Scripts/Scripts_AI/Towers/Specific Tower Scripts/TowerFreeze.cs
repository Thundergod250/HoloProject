using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TowerFreeze : TowerOffensiveBase
{
    [Header("Tower Mode")]
    [Tooltip("Check this to freeze everyone in range. Uncheck for single target.")]
    public bool isAOE = false;

    [Header("Detection")]
    public float range = 10f;
    public LayerMask enemyLayer;
    [SerializeField] private List<Navigation_Enemy> _currentTargets = new List<Navigation_Enemy>();

    [Header("Attack Settings")]
    public float freezeDuration = 2f;
    public float attackInterval = 5f;
    private bool _onCooldown = false;

    [SerializeField] private ParticleSystem _attackFreezeParticle;

    void Update()
    {
        // 1. Maintain the list of enemies currently in range
        UpdateTargetList();

        // 2. Fire if targets exist and we aren't reloading
        if (!_onCooldown && _currentTargets.Count > 0)
        {
            _ = ExecuteAttack();
        }
    }

    private void UpdateTargetList()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayer);
        List<Navigation_Enemy> enemiesThisFrame = new List<Navigation_Enemy>();

        foreach (var hit in hits)
        {
            // Use InParent if the script is on the root but collider is on a child
            Navigation_Enemy navEnemy = hit.GetComponentInParent<Navigation_Enemy>();
            if (navEnemy != null)
            {
                enemiesThisFrame.Add(navEnemy);
                if (!_currentTargets.Contains(navEnemy))
                {
                    _currentTargets.Add(navEnemy);
                }
            }
        }

        _currentTargets.RemoveAll(enemy => enemy == null || !enemiesThisFrame.Contains(enemy));
    }

    private async Task ExecuteAttack()
    {
        _onCooldown = true;

        if (_attackFreezeParticle != null) _attackFreezeParticle.Play();

        if (isAOE)
        {
            // Freeze EVERYONE in the list
            Debug.Log($"[Tower] AOE Freeze on {_currentTargets.Count} targets.");
            foreach (var enemy in _currentTargets)
            {
                if (enemy != null) enemy.EnemyFrozen(freezeDuration);
            }
        }
        else
        {
            if (_currentTargets.Count > 0 && _currentTargets[0] != null)
            {
                Debug.Log($"[Tower] Single Target Freeze on: {_currentTargets[0].name}");
                _currentTargets[0].EnemyFrozen(freezeDuration);
            }
        }

        await Task.Delay((int)(attackInterval * 1000));
        _onCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isAOE ? Color.cyan : Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
