using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Global reference to the PlayerController
    public PlayerController PlayerController;
    public CameraManager CameraManager;
    public UI_Manager UIManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}