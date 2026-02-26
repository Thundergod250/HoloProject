using UnityEngine;

public class Projectile_Enemy : MonoBehaviour
{
    [Header("Damage")]
    public int bulletDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<TowerBase>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        }

        Destroy(this.gameObject);
    }
}
