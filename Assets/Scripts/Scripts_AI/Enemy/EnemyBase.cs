using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public Health _healthReference;
    public EnemyMovement Movement;
    public EnemyAttack EnemyAttack;

    [SerializeField] private GameObject _enemyDeathPrefabReference;

    [SerializeField] private Slider _healthSlider;

    private bool _hasSpawnedPrefab = false;
    private void Start()
    {
        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _healthReference.GetMaxHealth();
            _healthSlider.value = _healthReference.GetCurrentHealth();
        }
    }

    private void LateUpdate()
    {
        HPSliderUpdater();
    }

    private void HPSliderUpdater()
    {
        if (_healthSlider != null)
        {
            _healthSlider.value = _healthReference.GetCurrentHealth();
        }

        if (_healthReference.GetCurrentHealth() <= 0 && !_hasSpawnedPrefab)
        {
            _hasSpawnedPrefab = true;
            Instantiate(_enemyDeathPrefabReference, transform.position, Quaternion.identity);
        }
    }
}
