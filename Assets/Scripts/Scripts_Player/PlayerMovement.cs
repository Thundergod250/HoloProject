using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool canMove = true;

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    
    /*public void EnableMovement()
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
    }*/
}
