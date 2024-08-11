using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    GameManager gameManager;
    public bool continueGame = false;
    public bool startGame = false;
    public GameObject MainMenuUI;
    public GameObject OptionMenuUI;
    public GameObject LoadingMenuUI;

    public static MainMenu instance;

    [Header("Slider")]
    public Scrollbar cameraSensitivityX;
    public Scrollbar cameraSensitivityY;

    [Header("Camera Sensitivity")]
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;

    public bool useMobileInput = false;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        instance = this;

        // Initialize slider values
        cameraSensitivityX.value = sensitivityX;
        cameraSensitivityY.value = sensitivityY;

        // Add listeners to sliders
        cameraSensitivityX.onValueChanged.AddListener(delegate { OnSensitivityXChanged(); });
        cameraSensitivityY.onValueChanged.AddListener(delegate { OnSensitivityYChanged(); });

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void onContinueButton()
    {
        Debug.Log("Continue");
        continueGame = true;
        StartCoroutine(ShowLoadingScreenAndLoadScene("mcp_day"));
    }

    public void onStartButton()
    {
        Debug.Log("Start");
        startGame = true;
        StartCoroutine(ShowLoadingScreenAndLoadScene("mcp_day"));
    }

    public void onOptionButton()
    {
        Debug.Log("Option");
        MainMenuUI.SetActive(false);
        OptionMenuUI.SetActive(true);
    }

    public void onQuitButton()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void onBackButton()
    {
        Debug.Log("Main Menu");
        MainMenuUI.SetActive(true);
        OptionMenuUI.SetActive(false);
    }

    public void OnSensitivityXChanged()
    {
        sensitivityX = cameraSensitivityX.value;
        Debug.Log("Sensitivity X changed to: " + sensitivityX);
    }

    public void OnSensitivityYChanged()
    {
        sensitivityY = cameraSensitivityY.value;
        Debug.Log("Sensitivity Y changed to: " + sensitivityY);
    }

    public void onMobileOn()
    {
        useMobileInput = true;
        Debug.Log("Mobile Input ON");
    }

    public void onMobileOff()
    {
        useMobileInput = false;
        Debug.Log("Mobile Input OFF");
    }

    private IEnumerator ShowLoadingScreenAndLoadScene(string sceneName)
    {
        // Show loading screen
        LoadingMenuUI.SetActive(true);
        MainMenuUI.SetActive(false);
        OptionMenuUI.SetActive(false);

        // Wait for one frame to ensure the UI updates
        yield return null;

        // Start loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-enable the main UI if necessary
        if (scene.name == "MainMenu")
        {
            MainMenuUI.SetActive(true);
            OptionMenuUI.SetActive(false);
            LoadingMenuUI.SetActive(false);
        }
    }
}
