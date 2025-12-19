using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
     NavMeshAgent agent;

    [Header("Roaming Settings")]
    [SerializeField] private float roamRadius = 10f;     // how far from start they can roam
    [SerializeField] private float roamInterval = 5f;    // seconds before switching target

    private float roamTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        roamTimer = roamInterval;
        SetNewDestination();
    }

    void Update()
    {
        roamTimer -= Time.deltaTime;

        // If timer expired or agent reached destination, pick a new one
        if (roamTimer <= 0f || (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance))
        {
            SetNewDestination();
            roamTimer = roamInterval;
        }
    }

    public void SlowDownAgent(int slowValueTarget)
    {
        agent.speed = agent.speed / slowValueTarget;
    }

    public void SpeedUpAgent(int speedValueTarget)
    {
        agent.speed = agent.speed * speedValueTarget;
    }

    private void SetNewDestination()
    {
        // Pick a random point inside a sphere
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}