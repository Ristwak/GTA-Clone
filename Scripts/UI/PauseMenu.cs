using System.Collections;
using System.Collections.Generic;
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
        if (gameManager.useMobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButtonDown("Pause"))
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        else
        {
            if (inputManager.pauseInput)
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        otherUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
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
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Show loading UI immediately
        if (mainMenu != null)
        {
            mainMenu.LoadingMenuUI.SetActive(true);
        }
        pauseMenuUI.SetActive(false);
        otherUI.SetActive(false);

        // Force UI update
        yield return null;

        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Ensure time scale is reset
        Time.timeScale = 1f;

        // Re-initialize game states
        InitializeGameStates();
    }

    private void InitializeGameStates()
    {
        // Re-initialize your game states here
        // For example, you can reset static variables, re-enable game objects, etc.
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
