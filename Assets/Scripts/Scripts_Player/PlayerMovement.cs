using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerAnimation playerAnimation;

    private bool canMove = true;
    private bool isJumping = false;
    private bool noclip = false;   // 🚀 new flag
    private Vector2 moveInput;
    private Vector3 velocity;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;

    [Header("Noclip Settings")]
    [SerializeField] private float noclipSpeed = 15f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (!GetCanMove()) return;

        if (noclip)
        {
            HandleNoclip();
        }
        else
        {
            HandleNormalMovement();
        }
    }

    private void HandleNormalMovement()
    {
        // Ground check
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (controller.isGrounded && isJumping)
        {
            isJumping = false;
            playerAnimation?.ResumeAfterJump();
        }

        // Camera-relative movement
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f; camRight.y = 0f;
        camForward.Normalize(); camRight.Normalize();

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;
        controller.Move(move * speed * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void HandleNoclip()
    {
        // WASD relative to camera, but allow vertical too
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.Normalize(); camRight.Normalize();

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;

        // 🚀 Move directly with transform
        transform.position += move * noclipSpeed * Time.deltaTime;

        // Optional: rotate to face move direction
        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }
    }

    public void SetNoclip(bool value)
    {
        noclip = value;
        controller.enabled = !noclip; // disable collider when noclip
        velocity = Vector3.zero;
    }

    public bool GetCanMove() => canMove;
    public void SetCanMove(bool value) => canMove = value;

    public void MovementOnMove(InputAction.CallbackContext context)
    {
        if (!GetCanMove()) return;
        moveInput = context.ReadValue<Vector2>();

        if (!noclip)
            playerAnimation?.UpdateMovementAnimation(moveInput.magnitude, isJumping);
    }

    public void MovementOnJump(InputAction.CallbackContext context)
    {
        if (!GetCanMove() || noclip) return;

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

    public Vector3 GetVelocity() => velocity;
}
