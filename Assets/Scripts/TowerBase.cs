using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TowerBase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected List<EnemyBase> _enemyTargets; // Need to change <GameObject> to Enemy Script

    [SerializeField] protected GameObject _projectilePrefab; // spawn this and should also have target as reference

    [SerializeField] protected Transform _projectileSpawnPoint;

    [Header("Parameters")]
    [SerializeField] protected float _firingSpeed = 1.0f;
    [SerializeField] protected float _delayChargeUp = 0.0f;
    [SerializeField] protected float _damageBase = 1.0f; // Kinda will just pass down to Projectile

    private bool _isFiring = false;

    private void Update()
    {
        CheckAndFire();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null && !_enemyTargets.Contains(enemy))
        {
            _enemyTargets.Add(enemy);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            _enemyTargets.Remove(enemy);
        }
    }

    private void CheckAndFire()
    {
        // 1. Clean up dead/destroyed targets
        _enemyTargets.RemoveAll(target => target == null);

        // 2. Check if we have targets AND are not currently firing
        if (_enemyTargets.Count > 0 && !_isFiring)
        {
            // Start the async firing sequence
            FireProjectileAsync();
        }
    }

    // async void is used for fire-and-forget methods like this
    protected virtual async void FireProjectileAsync()
    {
        // 1. Set the flag to true (MUST be on main thread)
        _isFiring = true;

        // 2. --- DELAY CHARGE UP ---
        if (_delayChargeUp >= 0)
        {
            // Calculate delay in milliseconds
            int delayMs = Mathf.RoundToInt(_delayChargeUp * 1000);
            // Wait without blocking the main thread
            await Task.Delay(delayMs);
        }

        // 3. --- FIRE PROJECTILE (SHOOT) ---

        // **SAFETY CHECK:** Crucial check after the delay to prevent missing
        if (_enemyTargets.Count > 0 && _enemyTargets[0] != null)
        {
            // All code here is guaranteed to be running on the main thread 
            // because there were no 'ConfigureAwait(false)' calls after the Task.Delay.

            EnemyBase target = _enemyTargets[0];

            // Instantiate the projectile
            GameObject projectileGO = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity);

            // Set projectile properties
            // Projectile projectileScript = projectileGO.GetComponent<Projectile>();
            //if (projectileScript != null)
            //{
            //    projectileScript.SetTarget(target.transform);
            //    projectileScript.SetDamage(_damageBase);
            //}

            Debug.Log("Tower: " + this.name + " is Shooting");
        }
        else
        {
            // If the target disappeared during charge-up, we exit early
            Debug.Log("Tower: " + this.name + " - Target disappeared during charge-up.");
        }

        // 4. --- DELAY COOLDOWN ---
        int cooldownMs = Mathf.RoundToInt(_firingSpeed * 1000);
        await Task.Delay(cooldownMs);

        // 5. Reset the flag (MUST be on main thread)
        _isFiring = false;
    }
}
