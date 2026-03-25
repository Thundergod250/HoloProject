using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Events")]
    public UnityEvent<int> OnDamaged;   // passes remaining health
    public UnityEvent OnDeath;          // triggered when health <= 0
    
    private bool isDead = false;

    [SerializeField] public int startSetHealth; //[FOR TESTING] 
    [SerializeField] protected bool _toBeDeleted = false;

    [SerializeField] private AudioClip _damageClip;
    [SerializeField] private HitFlash _hitFlash; 
    private void Awake()
    {
        currentHealth = maxHealth;
        currentHealth = startSetHealth;
    }
    
    public void TakeDamage(int amount)
    {
        Debug.Log(GetCurrentHealth() + " Tower Health");


        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (_hitFlash != null)
        {
            _hitFlash.PlayHitEffect();
        }

        else if (_damageClip != null && AudioManager.Instance != null )
        {
            AudioManager.Instance.PlaySFXOnce(_damageClip);
        }

        // Trigger damage event
        OnDamaged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        if (isDead) return;

        else if (_toBeDeleted)
        {
            Destroy(gameObject);
            return;
        }

        // _hitFlash.DieVFXEffect();
            isDead = true;

        OnDeath?.Invoke();


        Debug.Log($"{gameObject.name} has died.");
        // Optional: Destroy(gameObject); or disable
        
    }
    
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void ReviveHealth()
    {
        currentHealth = startSetHealth;
        isDead = false;
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}