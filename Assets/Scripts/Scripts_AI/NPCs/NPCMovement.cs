using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [Header("References")]
    public GameObject NPCBody;

    [Header("Rotation")]
    public float rotationSpeed = 8f;
    
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // we’ll handle rotation ourselves
    }
    
    public void _GoTotargetPoint(Transform targetPoint)
    {
        if (agent == null || targetPoint == null) return;
        agent.SetDestination(targetPoint.position);
    }

    private void Update()
    {
        // If agent is moving, rotate NPCBody toward velocity direction
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            NPCBody.transform.rotation = Quaternion.Slerp(
                NPCBody.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}