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
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator; // Animator reference

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    private bool canMove = true;   
    private bool isJumping = false; //  track jump state

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (GameManager.Instance != null)
            GameManager.Instance.PlayerController = this;
    }

    private void Update()
    {
        if (!canMove) return;

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

    // === Input System Callbacks ===
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        moveInput = context.ReadValue<Vector2>();

        // Only update animator speed if not frozen mid-jump
        if (animator != null && !isJumping)
            animator.SetFloat("Speed", moveInput.magnitude);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;

            if (animator != null)
            {
                Debug.LogWarning("PLAYER JUMPED");
                animator.speed = 0f;           // freeze animations
                animator.SetTrigger("Jump");   // optional jump animation
            }
        }
    }

    // === Movement Control Methods ===
    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
        moveInput = Vector2.zero;
        velocity = Vector3.zero;

        if (animator != null)
            animator.SetFloat("Speed", 0f);
    }
}
