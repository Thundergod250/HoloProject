using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneManager_MainMenu : MonoBehaviour
{
    private enum CinematicType { Video, ImageSequence }

    [Header("Global Settings")]
    [SerializeField] private CinematicType mode = CinematicType.Video;
    [SerializeField] private string mainGameSceneName = "MainGame";
    [SerializeField] private GameObject skipButton;

    [Header("Video Mode Settings")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoPanel;

    [Header("Image Sequence Settings")]
    [SerializeField] private RawImage displayImage;
    [SerializeField] private Texture2D[] cinematicFrames;
    [SerializeField] private float fps = 24f;

    [Header("Camera Move Settings")]
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDuration = 3f;

    private Coroutine currentCinematicRoutine;

    private void Start()
    {
        // Keep hidden until the movement phase is over
        if (skipButton != null) skipButton.SetActive(false);

        if (mode == CinematicType.Video && videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void _StartCinematic()
    {
        StartCoroutine(MasterCinematicFlow());
    }

    private IEnumerator MasterCinematicFlow()
    {
        // 1. Initial Camera Movement
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            float currentSpeed = Mathf.Lerp(0f, moveSpeed, t * t);
            cameraObj.transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);
            elapsed += Time.deltaTime;

            yield return null;
        }

        if (elapsed >= moveDuration)
        {
            videoPanel.SetActive(true);
        }

        // 2. Show Skip Button for BOTH modes
        if (skipButton != null) skipButton.SetActive(true);

        // 3. Play Content
        if (mode == CinematicType.Video)
        {
            if (videoPlayer != null) videoPlayer.Play();
            else LoadMainScene();
        }
        else
        {
            // Assign the routine to a variable so we can stop it if skipped
            currentCinematicRoutine = StartCoroutine(PlayImageSequence());
        }
    }

    private IEnumerator PlayImageSequence()
    {
        if (cinematicFrames == null || cinematicFrames.Length == 0)
        {
            LoadMainScene();
            yield break;
        }

        float frameDelay = 1f / fps;
        for (int i = 0; i < cinematicFrames.Length; i++)
        {
            displayImage.texture = cinematicFrames[i];
            yield return new WaitForSeconds(frameDelay);
        }

        LoadMainScene();
    }

    // Link your UI Button to this function
    public void SkipEverything()
    {
        // Stop the JPEG sequence if it's running
        if (currentCinematicRoutine != null) StopCoroutine(currentCinematicRoutine);

        // Stop the Master flow
        StopAllCoroutines();

        // Stop Video Player if it's running
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        LoadMainScene();
    }

    private void OnVideoFinished(VideoPlayer vp) => LoadMainScene();

    private void LoadMainScene() => SceneManager.LoadScene(mainGameSceneName);

    private void OnDestroy()
    {
        if (videoPlayer != null) videoPlayer.loopPointReached -= OnVideoFinished;
    }
}