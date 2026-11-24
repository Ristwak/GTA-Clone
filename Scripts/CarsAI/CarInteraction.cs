using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CarInteraction : MonoBehaviour
{
    InputManager inputManager;
    GameManager gameManager;
    public GameObject player;
    public GameObject car1;
    public GameObject car2;
    public GameObject car3;
    Transform exitPosition;
    GameObject currentCar;
    public GameObject playerCamera;
    public bool playerInCar;
    public int carSeatRange = 3;
    public KeyCode carIntercatKey = KeyCode.E;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (playerInCar)
        {
            if (exitPosition != null && player != null && playerCamera != null)
            {
                playerCamera.transform.position = exitPosition.position;
                player.transform.position = exitPosition.position;
            }
            if (inputManager != null)
                inputManager.jumpInput = false;
        }
        if (gameManager == null)
            return;
        bool interactPressed = gameManager.useMobileInputs
            ? CrossPlatformInputManager.GetButtonDown("EKey")
            : Input.GetKeyDown(carIntercatKey);
        if (!interactPressed)
            return;
        if (currentCar == null)
        {
            TryEnterCar(car1);
            if (currentCar == null)
                TryEnterCar(car2);
            if (currentCar == null)
                TryEnterCar(car3);
        }
        else
        {
            ExitCar();
        }
    }

    void TryEnterCar(GameObject car)
    {
        if (car == null || player == null || playerCamera == null)
            return;
        if (Vector3.Distance(player.transform.position, car.transform.position) >= carSeatRange)
            return;
        PrometeoCarController carController = car.GetComponent<PrometeoCarController>();
        Transform carExit = car.transform.Find("CarExitPoint");
        Transform carCamera = car.transform.Find("CarCamera");
        if (carController == null || carExit == null || carCamera == null)
        {
            Debug.LogError("CarInteraction: Car is missing PrometeoCarController, CarExitPoint or CarCamera.", car);
            return;
        }
        currentCar = car;
        exitPosition = carExit;
        carController.enabled = true;
        carCamera.gameObject.SetActive(true);
        player.SetActive(false);
        playerCamera.SetActive(false);
        playerInCar = true;
    }

    void ExitCar()
    {
        if (currentCar == null)
        {
            playerInCar = false;
            return;
        }
        PrometeoCarController carController = currentCar.GetComponent<PrometeoCarController>();
        Transform carCamera = currentCar.transform.Find("CarCamera");
        exitPosition = currentCar.transform.Find("CarExitPoint");
        if (carController != null)
            carController.enabled = false;
        if (carCamera != null)
            carCamera.gameObject.SetActive(false);
        if (player != null)
        {
            player.transform.position = exitPosition != null ? exitPosition.position : currentCar.transform.position;
            player.SetActive(true);
        }
        if (playerCamera != null)
            playerCamera.SetActive(true);
        currentCar = null;
        exitPosition = null;
        playerInCar = false;
    }
}