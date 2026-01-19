using UnityEngine;

public class PlayerAnimAttackChooser : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false;
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator.SetBool("Setter", _isPlayer);
    }
}
