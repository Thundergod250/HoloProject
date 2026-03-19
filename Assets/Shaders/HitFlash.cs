using UnityEngine;

public class HitFlash : MonoBehaviour
{
    public Renderer rend;
    public float duration = 0.1f;

    Material mat;

    void Start()
    {
        mat = rend.material;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        mat.SetFloat("_FlashAmount", 1f);
        yield return new WaitForSeconds(duration);
        mat.SetFloat("_FlashAmount", 0f);
    }
}
