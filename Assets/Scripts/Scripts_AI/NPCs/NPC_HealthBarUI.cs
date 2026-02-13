using UnityEngine;
using UnityEngine.UI;

public class NPC_HealthBarUI : MonoBehaviour
{
    public Slider slider;
    public Health helth;

    private void Start()
    {
        SetMaxHealth(helth.GetMaxHealth());
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth()
    {
        slider.value = helth.GetCurrentHealth();
    }
}
