using UnityEngine;

public class Projectile_Enemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<TowerBaseFunction>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(25);
        }

        Destroy(this.gameObject);
    }
}
