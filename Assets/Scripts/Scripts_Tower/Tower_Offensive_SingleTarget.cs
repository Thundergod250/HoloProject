using UnityEngine;

public class Tower_Offensive_SingleTarget : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Tower Stats")]
    public int towerDamageLevel = 1;
    public int towerDamage = 10;
    public int towerFireRateLevel = 1; 
    public float towerFireRate = 1f;
    public float detectionRadius = 10f;

    private float fireCooldown;

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            TryFireAtTarget();
            fireCooldown = towerFireRate;
        }
    }

    private void TryFireAtTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                FireProjectile(enemy.transform);
                break; // Only fire at one target
            }
        }
    }

    private void FireProjectile(Transform target)
    {
        if (projectilePrefab == null || target == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        ProjectileTemp temp = proj.GetComponent<ProjectileTemp>();
        if (temp != null)
        {
            temp.Initialize(target, towerDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}