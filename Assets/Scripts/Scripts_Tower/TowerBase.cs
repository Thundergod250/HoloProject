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
    [SerializeField] protected int damageBase = 5; // Kinda will just pass down to Projectile
    [SerializeField] protected float _projectileSpeed = 1f;
    
    [SerializeField] protected int damageIncreasePerLevel = 5;

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
        _isFiring = true;

        // 2. --- DELAY CHARGE UP ---
        if (_delayChargeUp >= 0)
        {
            int delayMs = Mathf.RoundToInt(_delayChargeUp * 1000);
            await Task.Delay(delayMs);
        }

        // 3. --- FIRE PROJECTILE (SHOOT) ---
        if (_enemyTargets.Count > 0 && _enemyTargets[0] != null)
        {
            EnemyBase target = _enemyTargets[0];

            // Instantiate the projectile
            GameObject projectileGO = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity);

            // --- CORE CHANGE: Get Rigidbody and apply force ---
            Rigidbody rb = projectileGO.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Calculate the direction vector from the tower to the target
                // It's crucial to normalize the direction vector
                Vector3 directionToTarget = (target.transform.position - _projectileSpawnPoint.position).normalized;

                // Define the speed (assuming you added the _projectileSpeed field)
                float projectileSpeed = _projectileSpeed; // Use your defined speed

                rb.AddForce(directionToTarget * projectileSpeed, ForceMode.VelocityChange);
            }
            else
            {
                Debug.LogError("Projectile prefab is missing a Rigidbody component! Cannot use AddForce.");
            }

            Debug.Log("Tower: " + this.name + " is Shooting");
        }
        else
        {
            Debug.Log("Tower: " + this.name + " - Target disappeared during charge-up.");
        }

        // 4. --- DELAY COOLDOWN ---
        int cooldownMs = Mathf.RoundToInt(_firingSpeed * 1000);
        await Task.Delay(cooldownMs);

        // 5. Reset the flag
        _isFiring = false;
    }

    public void _IncreaseDamage()
    {
        damageBase += damageIncreasePerLevel;
    }
}
