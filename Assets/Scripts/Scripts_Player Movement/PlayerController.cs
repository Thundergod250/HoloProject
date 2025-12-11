using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform; // Drag your Main Camera here

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Safety check: if no camera assigned, auto‑find the main camera
        if (cameraTransform == null && Camera.main != null) 
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // === 1. Ground Check and Reset Gravity Velocity ===
        if (controller.isGrounded && velocity.y < 0) 
            velocity.y = -2f; // small negative to keep grounded state consistent

        // === 2. Camera‑Relative Horizontal Movement ===
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Flatten to horizontal plane
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Combine input with camera orientation
        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;

        // Apply movement
        controller.Move(move * speed * Time.deltaTime);

        // === 3. Gravity and Vertical Movement ===
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // === 4. Rotate Player to Face Movement Direction ===
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    // Input System callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // Debug.Log($"Move Input: {moveInput}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            // Debug.Log("Jump triggered");
        }
    }
}
