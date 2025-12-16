using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int _maxHp = 100;
    public int _currentHp = 100;

    public void TakeDamage(int targetDamage)
    {
        _currentHp -= targetDamage;
    }

    private void CheckIfEnemyAlive()
    {
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            Debug.Log("Enemy Dead");

            Destroy(gameObject);
        }
    }

}
