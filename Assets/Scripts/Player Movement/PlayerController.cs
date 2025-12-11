using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f; // Standard Earth gravity

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity; // Stores vertical velocity (for gravity/jumping)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the CharacterController component attached to this GameObject
        controller = GetComponent<CharacterController>();
    }

    // Called when the Move action is performed, which stores the input values
    public void OnMove(InputAction.CallbackContext context)
    {
        // Read the 2D vector (X and Y axis input) from the action
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {moveInput}");
    }

    // Called when the Jump action is performed
    public void OnJump(InputAction.CallbackContext context)
    {
        // Debug statement to check if the function is being called and if the character is grounded
        Debug.Log($"Jumping - Is Grounded: {controller.isGrounded}");

        // Check if the input button was just pressed AND the character is currently touching the ground
        if (context.performed && controller.isGrounded)
        {
            Debug.Log("We are supposed to jump");
            
            // Calculate the required vertical velocity (y) for the jump height
            // Formula: v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // === 1. Ground Check and Reset Gravity Velocity ===
        // If the character is touching the ground and falling, reset the downward velocity
        // (Use a small negative value to force the isGrounded check to work properly next frame)
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // === 2. Horizontal Movement ===
        // Calculate the direction vector: transform.right (local X) * moveInput.x + transform.forward (local Z) * moveInput.y
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Apply horizontal movement to the controller
        // Time.deltaTime ensures frame-rate independence
        controller.Move(move * speed * Time.deltaTime);

        // === 3. Gravity and Vertical Movement ===
        // Apply gravity acceleration to the vertical velocity (velocity.y)
        velocity.y += gravity * Time.deltaTime;

        // Apply the vertical velocity to the controller
        controller.Move(velocity * Time.deltaTime);
    }
}