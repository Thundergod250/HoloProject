using UnityEngine;

public class ProjectileTemp : MonoBehaviour
{
    private Transform target;
    private int damage;
    public float speed = 10f;

    public void Initialize(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * (speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            ApplyDamage();
            Destroy(gameObject);
        }
    }

    private void ApplyDamage()
    {
        EnemyBase enemy = target.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.Health.TakeDamage(damage);
        }
    }
}