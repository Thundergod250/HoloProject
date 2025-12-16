using UnityEngine;

public class GrabManipulator : MonoBehaviour
{
    public void _Grab(GameObject obj) => GameManager.Instance.PlayerController.PlayerGrab?.GrabObject(obj);

    public void _Release(GameObject obj) => GameManager.Instance.PlayerController.PlayerGrab?.ReleaseGrabbedObject(obj);

    public void _Remove(GameObject obj) => GameManager.Instance.PlayerController.PlayerGrab?.RemoveGrabObject(obj);
}