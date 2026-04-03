using UnityEngine;

public class Fairy : Effects_Enemy
{
    [Header("Fairy InField FX")]
    public bool isInvis;
    public GameObject invisEffect;
    private bool lastInvisState;

    [Header("Refs")]
    [SerializeField] private Navigation_Enemy navigation_Enemy;
    [SerializeField] private SphereCollider aggroRangeCollider;
    [SerializeField] private BoxCollider fairyCollider;

    private int lastHealth;

    private void Start()
    {
        isInvis = true; 
        ApplyInvisState();
        lastInvisState = isInvis;
    }

    private void Update()
    {
        if (isInvis != lastInvisState)
        {
            ApplyInvisState();
            lastInvisState = isInvis;
        }

        CannotBeTargeted();
    }

    public void ApplyInvisState()
    {
        invisEffect.SetActive(isInvis);
    }

    public void CannotBeTargeted()
    {
        if(isInvis)
        {
            fairyCollider.enabled = false;
        }
        else if (!isInvis)
        {
            fairyCollider.enabled = true;
        }
    }
}
