using System.Collections;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float _timerSelfDestroy = 1f;

    private void Start()
    {
        StartCoroutine(CO_SelfDestroy());
    }
    IEnumerator CO_SelfDestroy()
    {
        yield return new WaitForSeconds(_timerSelfDestroy);

        Destroy(this.gameObject);
    }
}
