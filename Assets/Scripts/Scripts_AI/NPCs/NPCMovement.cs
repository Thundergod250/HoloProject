using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Target")]
    public GameObject TargetPoint;

    public GameObject NPCBody;

    [Header("Rotation")]
    public float rotationSpeed = 8f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
    }

    public void GoTotargetPoint()
    {
        if (TargetPoint == null)
            return;

        agent.SetDestination(TargetPoint.transform.position);

        //if (agent.velocity.sqrMagnitude > 0.01f)
        //{
        //    RotateTowards(agent.velocity);
        //}
    }

    void RotateTowards(Vector3 movement)
    {
        movement.y = 0f;
        if (movement == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
