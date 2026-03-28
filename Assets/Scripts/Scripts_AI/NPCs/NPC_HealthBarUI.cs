using UnityEngine;
using UnityEngine.UI;

public class NPC_HealthBarUI : MonoBehaviour
{
    public Slider slider;
    public Health helth;
    private Camera mainCamera;
    [SerializeField] private bool _forceLookPlayer = true;

    private void Start()
    {
        SetMaxHealth(helth.GetMaxHealth());
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        if (_forceLookPlayer)
        {
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
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
