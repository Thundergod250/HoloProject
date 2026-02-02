using System.Collections;
using UnityEngine;

public class Tower_Defensive_HealingTower : TowerDefensiveBase
{
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected int healAmount = 10;
    [SerializeField] protected float delayPerTick = 1f;
    [SerializeField] public float detectionRadius = 5f;

    private float healCooldown;

    private void Update()
    {
        healCooldown -= Time.deltaTime;

        if (healCooldown <= 0f)
        {
            CheckforPlayerToHeal();
            healCooldown = delayPerTick;
        }

    }

    private void CheckforPlayerToHeal()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            playerController = hit.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController?.gameObject.GetComponentInChildren<Health>().Heal(healAmount);
                break; // Only fire at one target
            }
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<PlayerController>())
    //    {
    //        playerController = other.GetComponent<PlayerController>();

    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<PlayerController>())
    //    {
    //        playerController = null;
    //    }
    //}


    //IEnumerator CO_HealPerTick()
    //{
    //    yield return new WaitForSeconds(delayPerTick);
    //    playerController?.gameObject.GetComponentInChildren<Health>()?.Heal(healAmount);

    //    if (playerController != null)
    //    {
    //        StartCoroutine(CO_HealPerTick());
    //    }
    //}
}
