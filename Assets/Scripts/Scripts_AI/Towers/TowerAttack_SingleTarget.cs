using UnityEngine;

public class TowerAttack_SingleTarget : TowerAttack
{
    [Header("Single Target Settings")]
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int projectileDamage = 20;

    protected override void PerformAttack()
    {
        if (this == null || detector == null || !gameObject.activeInHierarchy) return;

        GameObject target = detector.GetLatestDetected();
        if (target == null) return;

        // Spawn projectile
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Configure projectile
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(target.transform, projectileSpeed, projectileDamage);
        }

        Debug.Log($"Tower fired at {target.name}");
    }
}