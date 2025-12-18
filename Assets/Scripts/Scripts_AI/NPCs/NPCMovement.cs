using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject TargetPoint;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoTotargetPoint()
    {
        agent.SetDestination(TargetPoint.transform.position);
    }
}
