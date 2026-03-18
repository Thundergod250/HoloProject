using UnityEngine;
using System.Threading.Tasks;
public class PlayerHarvesting : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;

    [SerializeField] private GameObject _generalPlayerActions;
    [SerializeField] private GameObject _shovelingAction;
    [SerializeField] private GameObject _drillingAction;
    [SerializeField] private GameObject _miningAction;

    [SerializeField] private int attackDamage = 1;
    [SerializeField] private int attackIntervalMs = 500; // Time between "hits"

    [SerializeField] TrashHeap_ResourceSpawner targetHeap;
    [SerializeField] GarbageObject.GarbageGroup _garbageGroupType;
    [SerializeField] public DropResourceManager _resourceManagerRefererce;
    private bool _isAttacking = false; // Prevents overlapping attack loops


    [SerializeField] private AudioClip _miningAudioClip;

    private void Start()
    {
        //ResetActions();
    }

    private void Update()
    {
        // Check for Mouse0 being HELD DOWN
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_isAttacking && targetHeap != null)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance?.PlaySFXLoop(_miningAudioClip);
            }

            playerAnimation.TriggerMiningStart();
            _ = StartHarvestingLoop();
        }

        // Reset when the button is released
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            AudioManager.Instance?.StopSFXSound();

            playerAnimation.ForceIdleState();
            _isAttacking = false;
            ResetActions();
        }

        //else if (!targetHeap.isActiveAndEnabled)
        //{
        //    targetHeap = null;
        //}
        if(targetHeap != null && targetHeap.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            targetHeap = null;
            _isAttacking = false;

            playerAnimation.ForceIdleState();
            ResetActions(); // Stop animations if we walk away while clicking
        }
    }

    private async Task StartHarvestingLoop()
    {
        _isAttacking = true;

        while(_isAttacking) 
        {
            // While the button is held and we have a target
            // Wait for the interval before the next hit
            await Task.Delay(attackIntervalMs);
            //playerAnimation.TriggerMiningLoop(false);

            // Damage Logic
            if (targetHeap._health != null)
            {
                targetHeap.AttackTriggerAnimation();
                targetHeap._health?.TakeDamage(attackDamage);
            }
        }
        
        //targetHeap.StopTriggerAnimation();
        //_isAttacking = false;

        //playerAnimation.ForceIdleState();
        //playerAnimation.ResetAnimations();
    }


    private void ResetActions()
    {
        //_generalPlayerActions.SetActive(true);
        _shovelingAction.SetActive(false);
        _drillingAction.SetActive(false);
        _miningAction.SetActive(false);
    }

    private void SwitchToAnimationType()
    {
        if (_garbageGroupType == GarbageObject.GarbageGroup.Plastic) { _miningAction.SetActive(true); }
        else if (_garbageGroupType == GarbageObject.GarbageGroup.Organic) { _shovelingAction.SetActive(true); }
        else if (_garbageGroupType == GarbageObject.GarbageGroup.Metal) { _drillingAction.SetActive(true); }
        else if (_garbageGroupType == GarbageObject.GarbageGroup.CopperOre|| _garbageGroupType == GarbageObject.GarbageGroup.IronOre|| _garbageGroupType == GarbageObject.GarbageGroup.GoldOre) { _miningAction.SetActive(true); }
    }

    private void OnTriggerStay(Collider other)
    {
        // if (other.TryGetComponent<TrashHeap_ResourceSpawner>(out var heap))
        if (other.GetComponent<TrashHeap_ResourceSpawner>())
        {
            targetHeap = other.GetComponent<TrashHeap_ResourceSpawner>();
            _garbageGroupType = targetHeap._garbageGroupType;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TrashHeap_ResourceSpawner>())
        {
            targetHeap = null;
            _isAttacking = false;
            ResetActions(); // Stop animations if we walk away while clicking

            playerAnimation.ForceIdleState();
        }
    }
}
