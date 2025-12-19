using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private Transform target;
    private int damage;
    private float speed;
    private GameObject prefabRef; 
    
    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;
    private float lifeTimer;

    [Header("Explosion VFX")]
    [SerializeField] private GameObject explosionVFX; // pooled explosion prefab

    public void Initialize(Transform target, int damage, float speed, GameObject prefabRef)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.prefabRef = prefabRef;

        lifeTimer = lifetime;
    }

    private void Update()
    {
        // Lifetime countdown
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Explode();
            ReturnToPool();
            return;
        }

        if (target == null)
        {
            Explode();
            ReturnToPool();
            return;
        }

        // Move toward target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * (speed * Time.deltaTime);

        // 👇 Rotate projectile to face its movement direction
        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void ApplyDamage(EnemyBase enemy)
    {
        if (enemy != null) 
            enemy.Health.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemy))
            ApplyDamage(enemy);

        Explode();
        ReturnToPool();
    }

    private void Explode()
    {
        if (explosionVFX == null) return;

        // Spawn explosion via pooling
        GameObject vfx = GameManager.Instance.SpawnObject(
            explosionVFX,
            null,
            transform.position,
            Quaternion.identity
        );

        // If explosion prefab has DelayDisable, it will auto‑return
        var delay = vfx.GetComponent<DelayDisable>();
        if (delay != null)
            delay.SetPrefabReference(explosionVFX);
    }

    private void ReturnToPool()
    {
        if (prefabRef != null)
            GameManager.Instance.ObjectPooling.Return(prefabRef, gameObject);
        else
            gameObject.SetActive(false);
    }
}
