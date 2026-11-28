using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour
{
    InputManager inputManager;
    MainMenu mainMenu;
    GameManager gameManager;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject otherUI;
    bool isPaused = false;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        mainMenu = FindObjectOfType<MainMenu>();
    }

    void Update()
    {
        if (gameManager == null)
            return;
        bool pausePressed = gameManager.useMobileInputs
            ? CrossPlatformInputManager.GetButtonDown("Pause")
            : inputManager != null && inputManager.pauseInput;
        if (!pausePressed)
            return;
        if (inputManager != null)
            inputManager.pauseInput = false;
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (otherUI != null)
            otherUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        if (otherUI != null)
            otherUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        StartCoroutine(LoadSceneAsync("mcp_day"));
    }

    public void MainMenu()
    {
        StartCoroutine(LoadSceneAsync("MainMenu"));
    }

    public void onOptionButton()
    {
        Debug.Log("Option");
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(true);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (mainMenu != null && mainMenu.LoadingMenuUI != null)
            mainMenu.LoadingMenuUI.SetActive(true);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (otherUI != null)
            otherUI.SetActive(false);
        yield return null;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        if (asyncLoad == null)
        {
            Time.timeScale = 1f;
            yield break;
        }
        while (!asyncLoad.isDone)
            yield return null;
        Time.timeScale = 1f;
        InitializeGameStates();
    }

    private void InitializeGameStates()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}