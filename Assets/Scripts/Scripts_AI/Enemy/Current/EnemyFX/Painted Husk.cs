using UnityEngine;

public class PaintedHusk : Effects_Enemy
{
    [Header("Deathrattle Effect")]
    [SerializeField] private GameObject paintedHusk;
    [SerializeField] private int spawnCount;

    public void OnDeath()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject pHusk = Instantiate(paintedHusk, transform.position, Quaternion.identity);
            pHusk.GetComponent<Navigation_Enemy>().wayPoints = this.gameObject.GetComponent<Navigation_Enemy>().wayPoints;

        }
    }
}
