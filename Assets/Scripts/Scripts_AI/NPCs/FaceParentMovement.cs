using UnityEngine;

public class FaceParentMovement : MonoBehaviour
{
    public Transform parent;          // The moving object
    public float rotationSpeed = 10f;

    Vector3 lastParentPosition;

    void Start()
    {
        if (parent == null)
            parent = transform.parent;

        lastParentPosition = parent.position;
    }

    void Update()
    {
        Vector3 movement = parent.position - lastParentPosition;
        movement.y = 0f;

        if (movement.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        lastParentPosition = parent.position;
    }
}
