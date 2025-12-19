using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Base References")]
    [SerializeField] protected float _damage = 0;

    [SerializeField] protected bool _isSelfDestroy = false;
    [SerializeField] protected bool _isImpactDestroy = false;

    [SerializeField] protected float _selfDestroyDelay = 1f;

    [Header("Explosive References")]
    [SerializeField] protected GameObject _explosionPrefab;
    [SerializeField] protected bool _isExplosive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyBase>())
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (_isExplosive) // Spawns Kaboom
            {
                GameObject projectileGO = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            }

            enemy.Health.TakeDamage((int)_damage);
        }

        else if(_isImpactDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (_isSelfDestroy)
        {
            StartCoroutine(CO_SelfDestroy());
        }
    }

    IEnumerator CO_SelfDestroy()
    {
        yield return new WaitForSeconds(_selfDestroyDelay);
        Destroy(this.gameObject);
    }

}
