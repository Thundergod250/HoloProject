using UnityEngine;

public class CurvedUI_Health : MonoBehaviour
{
    public Renderer meshRenderer;
    [SerializeField] private Health health;

    // This MUST match the "Reference" name in your Shader Graph Blackboard
    private static readonly int FillAmountID = Shader.PropertyToID("_FillAmount");

    private float lastHealthPercent = -1f;

    void Start()
    {
        if (health == null) health = GetComponentInParent<Health>();
        UpdateVisuals();
    }

    void Update()
    {
        float currentPercent = (float)health.GetCurrentHealth() / (float)health.GetMaxHealth();

        // Only update the shader if the health value has actually changed
        if (!Mathf.Approximately(currentPercent, lastHealthPercent))
        {
            UpdateVisuals(currentPercent);
            lastHealthPercent = currentPercent;
        }
    }

    void UpdateVisuals(float percent)
    {
        if (meshRenderer != null)
        {
            // This sends the 0-1 value to the "Step" node in your Shader Graph
            meshRenderer.material.SetFloat(FillAmountID, percent);
        }
    }

    // Overload for initial start
    void UpdateVisuals()
    {
        UpdateVisuals( (float)(health.GetCurrentHealth() / health.GetMaxHealth()) );
    }
}
