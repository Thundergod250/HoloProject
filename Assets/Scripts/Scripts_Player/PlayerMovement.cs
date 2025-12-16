using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // === Core Components ===
    private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerAnimation playerAnimation;

    // === Movement State ===
    private bool canMove = true;
    private bool isJumping = false;
    private Vector2 moveInput;
    private Vector3 velocity;

    // === Movement Settings ===
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;
        
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (!GetCanMove()) return;

        // Ground check
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // If we land after a jump, resume animations
        if (controller.isGrounded && isJumping)
        {
            isJumping = false;
            playerAnimation?.ResumeAfterJump();
        }

        // Camera-relative movement
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;
        controller.Move(move * speed * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Rotate towards movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public bool GetCanMove() => canMove;
    public void SetCanMove(bool value) => canMove = value;

    public void MovementOnMove(InputAction.CallbackContext context)
    {
        if (!GetCanMove()) return;
        moveInput = context.ReadValue<Vector2>();

        // Delegate animation update
        playerAnimation?.UpdateMovementAnimation(moveInput.magnitude, isJumping);
    }
    
    public void MovementOnJump(InputAction.CallbackContext context)
    {
        if (!GetCanMove()) return;
        
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;

            playerAnimation?.TriggerJump();
        }
    }
    
    public void DisableMovement()
    {
        SetCanMove(false); 
        moveInput = Vector2.zero;
        velocity = Vector3.zero;

        playerAnimation?.ResetAnimations();
    }
}
