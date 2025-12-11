using UnityEngine;

public abstract class TowerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] protected float attackInterval = 1f;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform firePoint;

    protected DetectEntity detector;
    protected float attackTimer;

    protected virtual void Awake()
    {
        detector = GetComponent<DetectEntity>();
    }

    protected virtual void Update()
    {
        if (this == null || !gameObject.activeInHierarchy) return;

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
            PerformAttack();
        }
    }


    /// <summary>
    /// Child classes override this to define attack behavior.
    /// </summary>
    protected abstract void PerformAttack();
}