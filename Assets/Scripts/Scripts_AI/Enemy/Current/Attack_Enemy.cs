using System.Collections;
using UnityEngine;

public class Attack_Enemy : MonoBehaviour
{
    public enum Targeting
    {
        Ranged,
        Melee,
        Neutral
    }

    [Header("Refs")]
    [SerializeField] private Navigation_Enemy navigation_Enemy;
    [SerializeField] private Effects_Enemy effects_Enemy;

    [Header("Targeting Style")]
    [SerializeField] private Targeting targeting;

    [Header("Ranged")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private float shootingInterval;
    [SerializeField] private float shootingSpeed;

    [Header("Melee")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private int damage;
    private bool isAttacking = false;


    public Animator animator;
    public Transform target;
    public Targeting archetype => targeting;

    private float timer;

    void Update()
    {
        if(target != null) // if has target
        {
            timer += Time.deltaTime;

            if (timer >= shootingInterval && targeting == Targeting.Ranged)
            {
                AttackType();
                timer = 0f;
            }
            else if (targeting == Targeting.Melee)
            {
                AttackType();
            }
            else if (targeting == Targeting.Neutral)
            {
                AttackType();
            }

            if (target.GetComponent<Health>().GetCurrentHealth() == 0)
            {
                Debug.Log("changing target");
                navigation_Enemy.TargetHasDied();
            }
        }
    }

    void AttackType()
    {
        if(targeting == Targeting.Ranged)
        {
            transform.LookAt(target.transform);
            Vector3 direction = (target.position - transform.position).normalized;
            GameObject proj = Instantiate(bullet, gunBarrel.transform.position, Quaternion.identity);
            Rigidbody rb = proj.GetComponent<Rigidbody>();

            rb.linearVelocity = direction * 40f;

            Destroy(proj, 2f);
        }
        else if(targeting == Targeting.Melee)
        {
            if (!isAttacking)
            {
                animator.SetBool("isAttacking", true);
                StartCoroutine(MeleeAttackSpeed());
            }
        }
        else if (targeting == Targeting.Neutral)
        {
            //*insert attack scipt
        }
    }

    public IEnumerator MeleeAttackSpeed()
    {
        isAttacking = true; // prevent multiple coroutines
        Debug.Log("DANEG");

        // Wait for attack cooldown
        yield return new WaitForSeconds(attackSpeed);

        // Deal damage
        target.GetComponent<Health>().TakeDamage(damage);

        isAttacking = false; // allow next attack
    }
}
