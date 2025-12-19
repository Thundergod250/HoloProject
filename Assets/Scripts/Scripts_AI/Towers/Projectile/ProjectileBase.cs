using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private Transform target;
    private int damage;
    private float speed;
    private GameObject prefabRef; // 👈 reference for returning to pool

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;
    private float lifeTimer;

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
            ReturnToPool();
            return;
        }

        if (target == null)
        {
            ReturnToPool();
            return;
        }

        // Move toward target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * (speed * Time.deltaTime);

        // Homing Missle Code check
        /*if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            ApplyDamage();
            ReturnToPool();
        }*/
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

        ReturnToPool();
    }


    private void ReturnToPool()
    {
        if (prefabRef != null)
            GameManager.Instance.ObjectPooling.Return(prefabRef, gameObject);
        else
            gameObject.SetActive(false);
    }
}