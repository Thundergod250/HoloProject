using UnityEngine;
using System.Threading.Tasks;
public class PlayerHarvesting : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PickaxeLevel pickaxeLevel;

    [SerializeField] private GameObject _generalPlayerActions;
    [SerializeField] private GameObject _shovelingAction;
    [SerializeField] private GameObject _drillingAction;
    [SerializeField] private GameObject _miningAction;

    [SerializeField] private int attackDamage = 1;
    [SerializeField] private int attackIntervalMs = 500; // Time between "hits"
    [SerializeField] private UI_PromtWarnings _promptWarnings;


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
        if (CheckPickaxeTier()) // Checks if can Mine
        {
            if (Input.GetKey(KeyCode.Mouse0) && !_isAttacking && targetHeap != null)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance?.PlaySFXLoop(_miningAudioClip);
                }

                playerAnimation.TriggerMiningStart();

                _ = StartHarvestingLoop();
            }

            else if ((Input.GetKeyUp(KeyCode.Mouse0)) || targetHeap != null && targetHeap.GetComponent<Health>().GetCurrentHealth() <= 0)
            {
                AudioManager.Instance?.StopSFXSound();

                targetHeap.StopParticlesDamage();
                playerAnimation.ForceIdleState();
                playerMovement.SetCanMove(true);


                targetHeap = null;
                _isAttacking = false;

                // ResetActions(); // Stop animations if we walk away while clicking
            }
        }
    }

    private async Task StartHarvestingLoop()
    {
        _isAttacking = true;

        while(_isAttacking) 
        {
            playerMovement.SetCanMove(false);

            // While the button is held and we have a target
            // Wait for the interval before the next hit
            await Task.Delay(attackIntervalMs);
            //playerAnimation.TriggerMiningLoop(false);

            // Damage Logic
            if (targetHeap._health != null)
            {
                targetHeap.AttackTriggerAnimation();
                targetHeap.PlayParticlesDamage();
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

    public bool CheckPickaxeTier()
    {
        if (targetHeap._garbageGroupType == GarbageObject.GarbageGroup.IronOre && pickaxeLevel.copperPick)
        {
            //_promptWarnings.SetPromptTextDisplay("You need the Iron Pickaxe for this");
            Debug.Log("Iron Pick needed");
            return false;
        }
        else if (targetHeap._garbageGroupType == GarbageObject.GarbageGroup.GoldOre && (pickaxeLevel.ironPick || pickaxeLevel.copperPick))
        {
            //_promptWarnings.SetPromptTextDisplay("You need the Gold Pickaxe for this");
            Debug.Log("Gold Pick needed");
            return false;
        }

        Debug.Log("Mine able for you");
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TrashHeap_ResourceSpawner>())
        {
            if (!CheckPickaxeTier())
            {
                _promptWarnings.SetPromptTextDisplay("You need the Stronger Pickaxe for this");
            }
        }
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
            //ResetActions(); // Stop animations if we walk away while clicking

            // playerAnimation.ForceIdleState();
            // playerAnimation.UpdateMovementAnimation(1, false);
        }
    }
}
