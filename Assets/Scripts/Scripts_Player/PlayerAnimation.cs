using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void UpdateMovementAnimation(float speed, bool isJumping)
    {
        if (animator == null) return;
        if (!isJumping)
            animator.SetFloat("Speed", speed);
    }

    public void TriggerJump()
    {
        if (animator == null) return;
        animator.speed = 0f;           // freeze animations
        animator.SetTrigger("Jump");   // play jump animation
    }

    public void ResumeAfterJump()
    {
        if (animator == null) return;
        animator.speed = 1f;           // resume animation playback
    }

    public void ResetAnimations()
    {
        if (animator == null) return;
        animator.SetFloat("Speed", 0f);
    }
}