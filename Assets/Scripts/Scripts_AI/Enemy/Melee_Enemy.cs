using UnityEngine;
using System.Collections;

public class Melee_Enemy : MonoBehaviour
{
    private Coroutine damageCoroutine;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Health health) && collision.gameObject.GetComponent<TowerBaseFunction>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(100);
        }
    }
}
