using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject _generalPlayerActions;
    [SerializeField] private GameObject _shovelingAction;
    [SerializeField] private GameObject _drillingAction;
    [SerializeField] private GameObject _miningAction;


    [Header("Normal States")]
    [SerializeField] private string idleState = "Idle";
    [SerializeField] private string runState = "Walk";
    [SerializeField] private string jumpState = "Jump";

    [Header("Grab States")]
    [SerializeField] private string grabIdleState = "Basic Grab";
    [SerializeField] private string grabWalkState = "Grab and Walk";
    [SerializeField] private string grabJumpState = "Grab and Jump";

    [Header("Action States")]
    [SerializeField] private string shovelDigState = "Shovel and Dig";
    [SerializeField] private string miningState = "Mine and Hit";
    [SerializeField] private string drillState = "Drill";

    private PlayerGrab playerGrab;
    private string currentState;

    private void Start()
    {
        playerGrab = GetComponent<PlayerGrab>();

        _generalPlayerActions.SetActive(true);
        _shovelingAction.SetActive(false);
        _drillingAction.SetActive(false);
    }
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

        bool isCarrying = playerGrab != null && playerGrab.IsPlayerCarryingObject;

        if (speed > 0.1f)
        {
            PlayState(isCarrying ? grabWalkState : runState);
        }
        else
        {
            PlayState(isCarrying ? grabIdleState : idleState);
        }
    }

    public void TriggerJump()
    {
        bool isCarrying = playerGrab != null && playerGrab.IsPlayerCarryingObject;
        PlayState(isCarrying ? grabJumpState : jumpState, 0.05f);
    }

    public void ResetAnimations()
    {
        bool isCarrying = playerGrab != null && playerGrab.IsPlayerCarryingObject;
        PlayState(isCarrying ? grabIdleState : idleState, 0.1f);
    }

    public void AttackingState()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _generalPlayerActions.gameObject.SetActive(false);
            _drillingAction.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _generalPlayerActions.gameObject.SetActive(true);
            _drillingAction.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _generalPlayerActions.gameObject.SetActive(false);
            _shovelingAction.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _generalPlayerActions.gameObject.SetActive(true);
            _shovelingAction.gameObject.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _generalPlayerActions.gameObject.SetActive(false);
            _miningAction.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Backspace))
        {
            _generalPlayerActions.gameObject.SetActive(true);
            _miningAction.gameObject.SetActive(false);
        }
    }

}