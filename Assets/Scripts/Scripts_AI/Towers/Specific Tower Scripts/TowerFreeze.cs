using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TowerFreeze : TowerOffensiveBase
{
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
        // 1. Always Scan
        PerformScan();

        // 2. Try to Attack if we have targets and are not on cooldown
        if (!_onCooldown && _currentTargets.Count > 0)
        {
            _ = ExecuteFreezeAttack();
        }
        else if (_currentTargets[0] == null) { _currentTargets.Remove(_currentTargets[0]); }
    }

    private void PerformScan()
    {
        // _currentTargets.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayer);

        foreach (var hit in hits)
        {
            // Use GetComponentInParent in case the collider is on a child object
            Navigation_Enemy em = hit.GetComponentInChildren<Navigation_Enemy>();
            if (em != null && !_currentTargets.Contains(em))
            {
                _currentTargets.Add(em);
            }
        }
    }

    private async Task ExecuteFreezeAttack()
    {
        _onCooldown = true;

        Debug.Log($"[Tower] Freezing {_currentTargets.Count} targets!");

        _attackFreezeParticle.Play();

        // Apply freeze to everyone currently in the list
        foreach (var enemy in _currentTargets)
        {
            // if (enemy != null) enemy.ApplyFreeze(freezeDuration);
            // if (enemy != null) enemy.isMoving = false;

            // SET Freez Here

        }

        // Wait for the 5-second reload
        await Task.Delay((int)(attackInterval * 1000));
        _onCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
