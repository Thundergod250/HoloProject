using UnityEngine;

public class Burrower : Effects_Enemy
{
    [Header("InField FX")]
    public bool burrowed;
    public GameObject aboveGround;
    public GameObject underGround;
    private bool lastBurrowState;

    [Header("Refs")]
    [SerializeField] private Navigation_Enemy navigation_Enemy;
    [SerializeField] private Attack_Enemy attack_Enemy;
    [SerializeField] private SphereCollider aggroRangeCollider;
    [SerializeField] private BoxCollider enemyBaseCollider;
    private int lastHealth;

    private void Start()
    {
        burrowed = true;
        lastBurrowState = !burrowed;

        aboveGround.SetActive(false);
        underGround.SetActive(true);
    }

    private void Update()
    {
        if (navigation_Enemy.wayPointIndexRef == navigation_Enemy.wayPoints.Count - 2)
        {
            Debug.Log("at Base");
            burrowed = false;
        }

        if (burrowed != lastBurrowState)
        {
            ApplyBurrowState();
            lastBurrowState = burrowed;
        }
    }

    public void ApplyBurrowState()
    {
        if (burrowed)
        {
            aggroRangeCollider.enabled = false;
            enemyBaseCollider.enabled = false;

            aboveGround.SetActive(false);
            underGround.SetActive(true);

            Debug.Log("crawling");

            backToBurrowed(); 
        }
        else 
        {
            aggroRangeCollider.enabled = true;
            enemyBaseCollider.enabled = true;

            Debug.Log("dsee");

            aboveGround.SetActive(true);
            underGround.SetActive(false);
        }
    }


    public void backToBurrowed()
    {
        navigation_Enemy.targetsAcquired.Clear();
        navigation_Enemy.ResetCurrentTarget();

        if (navigation_Enemy.AttackEnemyRef != null)
        {
            navigation_Enemy.AttackEnemyRef.target = null; // remove enemy ref, back to straight to base
        }

        navigation_Enemy.navigation.isStopped = false;
        navigation_Enemy.FindNearestWaypoint();
    }
}
