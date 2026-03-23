using UnityEngine;

public class VFX_ForceUpward : MonoBehaviour
{
    [SerializeField] private GameObject _vfxForcedTarget;
    void Update()
    {
        if (_vfxForcedTarget != null)
        {
            Vector3 targetDirection = transform.forward; // Or your target vector
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // Adjust 5f to change how fast it "rights" itself
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
