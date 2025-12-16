using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private bool canMove = true;
    private CharacterController controller;
    [SerializeField] private Animator animator; // Animator reference
        private bool isJumping = false; //  track jump state

        private Vector2 moveInput;

        private Vector3 velocity;
        
        [Header("Movement Settings")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.8f;
        
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        
        
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
            if (animator != null)
                animator.speed = 1f; // resume animation playback
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

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void MovementOnMove(InputAction.CallbackContext context)
    {
        if (!GetCanMove()) return;
        moveInput = context.ReadValue<Vector2>();

        // Only update animator speed if not frozen mid-jump
        if (animator != null && !isJumping)
            animator.SetFloat("Speed", moveInput.magnitude);
    }
    
    public void MovementOnJump(InputAction.CallbackContext context)
    {
        if (!GetCanMove()) return;
        
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;

            if (animator != null)
            {
                animator.speed = 0f;           // freeze animations
                animator.SetTrigger("Jump");   // optional jump animation
            }
        }
    }
    
    public void DisableMovement()
    {
        SetCanMove(false); 
        moveInput = Vector2.zero;
        velocity = Vector3.zero;

        if (animator != null)
            animator.SetFloat("Speed", 0f);
    }
}
