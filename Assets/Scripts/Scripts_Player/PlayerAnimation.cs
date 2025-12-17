using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Animation Clips")]
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip runClip;
    [SerializeField] private AnimationClip jumpClip;

    [Header("Animator State Names")]
    [SerializeField] private string idleState = "Idle";
    [SerializeField] private string runState = "Walk";
    [SerializeField] private string jumpState = "Jump";

    private string currentState;

    private void PlayState(string stateName, float crossFade = 0.15f)
    {
        if (animator == null) return;
        if (currentState == stateName) return;

        animator.CrossFade(stateName, crossFade, 0);
        currentState = stateName;
    }

    public void UpdateMovementAnimation(float speed, bool isJumping)
    {
        if (isJumping) return;

        if (speed > 0.1f)
            PlayState(runState);
        else
            PlayState(idleState);
    }

    public void TriggerJump()
    {
        PlayState(jumpState, 0.05f);
    }

    public void ResetAnimations()
    {
        PlayState(idleState, 0.1f);
    }
}