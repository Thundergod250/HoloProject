using UnityEngine;
using System.Threading.Tasks;

public class TowerWard : TowerOffensiveBase
{
    [Header("Tower Stats")]
    public float range = 10f;
    public float freezeDuration = 2f;
    public float attackInterval = 5f;
    public LayerMask enemyLayer; // Set this to your Enemy layer in the Inspector


    [SerializeField] private AudioSource _wardTowerAudioSource;
    private bool canAttack = true;

    void Update()
    {
        if (canAttack)
        {
            ScanAndDetect();
        }
    }

    private async void ScanAndDetect()
    {
        // Find all colliders within range on the Enemy layer
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            canAttack = false;

            foreach (Collider col in enemiesInRange)
            {
                Debug.Log(col.name + " entered ward");


                Navigation_Enemy movement = col.GetComponentInParent<Navigation_Enemy>();

                if (movement != null)
                {
                    //movement.ApplyFreeze(freezeDuration);
                    // just check if invisible here, then function to be detected and raise

                    if (_wardTowerAudioSource != null)
                    {
                        _wardTowerAudioSource.Play();
                    }

                    if(col.GetComponentInParent<Fairy>() != null)
                    {
                        col.GetComponentInParent<Fairy>().isInvis = false;
                    }
                }
            }

            // Tower Cooldown
            await Task.Delay((int)(attackInterval * 1000));
            canAttack = true;
        }
    }

    // Visualizes the range in the Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
