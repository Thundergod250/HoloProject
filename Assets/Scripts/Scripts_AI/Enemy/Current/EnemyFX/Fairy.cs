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
    [SerializeField] private SphereCollider enemyBaseCollider;
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
    }

    public void ApplyInvisState()
    {
        invisEffect.SetActive(isInvis);
        enemyBaseCollider.enabled = !isInvis;
    }

    public void CannotBeTargeted()
    {
        if(isInvis)
        {
            enemyBaseCollider.enabled = false;
        }
        else if (!isInvis)
        {
            enemyBaseCollider.enabled = true;
        }
    }
}
