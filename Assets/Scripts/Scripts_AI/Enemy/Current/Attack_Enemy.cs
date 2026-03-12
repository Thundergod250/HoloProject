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
    [SerializeField] private int bulletDamage;
    [SerializeField] private float shootingInterval;
    [SerializeField] private float shootingSpeed;

    [Header("Melee")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private int damage;
    private bool isAttacking = false;

    [Header("Attack Limit")]
    [SerializeField] private int maxAttacks = 3;
    private int attackCounter = 0;

    public Animator animator;
    public Transform target;
    private Transform lastTarget;

    public Targeting archetype => targeting;

    private float timer;

    void Update()
    {
        if (target == null)
        {
            if (animator != null)
                animator.SetBool("isAttacking", false);

            return;
        }

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

        if (target.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            navigation_Enemy.TargetHasDied();
            ResetTarget();
        }
    }

    void AttackType()
    {
        // Stop attacking after max attacks
        if (attackCounter >= maxAttacks)
        {
            ResetTarget();
            return;
        }

        if (targeting == Targeting.Ranged)
        {
            transform.LookAt(target);

            Vector3 direction = (target.position - transform.position).normalized;

            GameObject proj = Instantiate(bullet, gunBarrel.position, Quaternion.identity);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            proj.GetComponent<Projectile_Enemy>().bulletDamage = bulletDamage;

            rb.linearVelocity = direction * 40f;

            Destroy(proj, 2f);

            attackCounter++;
        }
        else if (targeting == Targeting.Melee)
        {
            if (!isAttacking)
            {
                animator.SetBool("isAttacking", true);
                StartCoroutine(MeleeAttackSpeed());
            }
        }
    }

    public IEnumerator MeleeAttackSpeed()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackSpeed);

        if (target != null)
        {
            target.GetComponent<Health>().TakeDamage(damage);
            attackCounter++;
        }

        isAttacking = false;
    }

    void ResetTarget()
    {
        lastTarget = target;
        target = null;
        attackCounter = 0;

        if (animator != null)
            animator.SetBool("isAttacking", false);

        navigation_Enemy.TargetHasDied();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tower"))
            return;

        // Prevent attacking the same tower again
        if (other.transform == lastTarget)
            return;

        if (target == null)
        {
            target = other.transform;
        }
    }
}