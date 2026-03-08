using UnityEngine;

public class GameOverChecker : MonoBehaviour
{
    [SerializeField] private Health _baseHealthReference;
    [SerializeField] private SaveGameManager _saveGameManager;
    [SerializeField] private GameObject _gameOverUI;

    private bool _isGameOver = false;

    private void Start()
    {
        _gameOverUI.SetActive(false);
    }

    private void Update()
    {
        if (!_isGameOver && _baseHealthReference?.GetCurrentHealth() <= 0)
        {
            TriggerFailState();
        }
    }

    private void TriggerFailState()
    {
        _isGameOver = true;
        _gameOverUI.SetActive(true);

        Time.timeScale = 0f; // Freeze the action
        Cursor.visible = true; // Show mouse so they can click the button
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        // 1. Unfreeze time so the next scene actually runs
        Time.timeScale = 1f;

        // 2. Tell the SaveManager to reload the scene
        _saveGameManager.OnRestartButtonClick();
    }
}
