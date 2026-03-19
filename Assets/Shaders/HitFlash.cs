using UnityEngine;
using System.Collections;
public class HitFlash : MonoBehaviour
{
    public Renderer rend;
    public float duration = 0.1f;

    [SerializeField] GameObject _hitObject;

    Material mat;

    void Start()
    {
        mat = rend.material;
    }

    public void Flash()
    {
        StopAllCoroutines();
        // StartCoroutine(FlashRoutine());
        StartCoroutine(CO_PlayHit());
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        mat.SetFloat("_FlashAmount", 1f);
        yield return new WaitForSeconds(duration);
        mat.SetFloat("_FlashAmount", 0f);
    }

    private IEnumerator CO_PlayHit()
    {
        _hitObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        _hitObject.SetActive(true);
    }

}
