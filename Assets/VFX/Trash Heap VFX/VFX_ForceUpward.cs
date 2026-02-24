using UnityEngine;

public class VFX_ForceUpward : MonoBehaviour
{
    [SerializeField] private GameObject _vfxForcedTarget;
    void Update()
    {
        if (_vfxForcedTarget != null)
        {
            transform.position = _vfxForcedTarget.transform.up;
        }
    }
}
