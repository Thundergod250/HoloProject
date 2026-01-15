using UnityEngine;
using Unity.Cinemachine;
public class CutsceneEvents : MonoBehaviour
{
    [SerializeField] protected CinemachineCamera _camera;

    private void Start()
    {
        CinemachineCore.CameraActivatedEvent.AddListener(OnCameraActivated);
        CinemachineCore.CameraUpdatedEvent.AddListener(OnBrainUpdated);
    }

    public void TriggerCameraEvent(ICinemachineMixer origin, ICinemachineCamera incomingCamera)
    {
        Debug.Log(this.name + " : Camera became active");
    }

    private void OnBrainUpdated(CinemachineBrain Brain)
    {

    }

    private void OnCameraActivated(ICinemachineCamera.ActivationEventParams evt)
    {
        if (evt.IncomingCamera == (ICinemachineCamera) _camera )
        {
            Debug.Log("Event Camera Activated");
        }
    }
}
