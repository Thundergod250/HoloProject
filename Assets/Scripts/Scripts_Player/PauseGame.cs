using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private bool isPaused = false;

    void Update()
    {
        // Check for the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseTheGame();
        }
    }

    public void PauseTheGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Freezes physics and animations
        isPaused = true;

        // Optional: Unlock cursor so player can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Returns game to normal speed
        isPaused = false;

        // Optional: Re-lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadMainMenu(int target)
    {
        // Always reset time before switching scenes!
        Time.timeScale = 1f;

        // Loads the scene at index 0 (usually the Main Menu)
        SceneManager.LoadScene(target);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }
}
