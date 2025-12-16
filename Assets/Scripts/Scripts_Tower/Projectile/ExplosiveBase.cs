using UnityEngine;

public class ExplosiveBase : MonoBehaviour
{
    // For the sake of VFX, spawn here
    [SerializeField] float _damage = 0f;

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy)
        {
            enemy.TakeDamage((int)_damage);

            Destroy(this.gameObject);
        }
    }
}
