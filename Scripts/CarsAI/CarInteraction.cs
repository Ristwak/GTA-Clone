using System.Collections;
using System.Collections.Generic;
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
        if (playerInCar == true)
        {
            playerCamera.transform.position = exitPosition.transform.position;
            player.transform.position = exitPosition.transform.position;
            inputManager.jumpInput = false;
        }

        if (gameManager.useMobileInputs)
        {
            if (CrossPlatformInputManager.GetButton("EKey"))
            {
                if (currentCar == null)
                {
                    TryEnterCar(car1);
                    TryEnterCar(car2);
                    TryEnterCar(car3);
                }
                else
                {
                    ExitCar();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(carIntercatKey))
            {
                if (currentCar == null)
                {
                    TryEnterCar(car1);
                    TryEnterCar(car2);
                    TryEnterCar(car3);
                }
                else
                {
                    ExitCar();
                }
            }
        }

    }

    void TryEnterCar(GameObject car)
    {
        if (car != null && Vector3.Distance(player.transform.position, car.transform.position) < carSeatRange)
        {
            currentCar = car;
            exitPosition = currentCar.transform.Find("CarExitPoint");
            car.GetComponent<PrometeoCarController>().enabled = true;
            car.transform.Find("CarCamera").gameObject.SetActive(true);
            player.SetActive(false);
            playerCamera.gameObject.SetActive(false);
            playerInCar = true;
        }
    }

    void ExitCar()
    {
        currentCar.GetComponent<PrometeoCarController>().enabled = false;
        currentCar.transform.Find("CarCamera").gameObject.SetActive(false);

        exitPosition = currentCar.transform.Find("CarExitPoint");
        if (exitPosition != null)
        {
            player.transform.position = exitPosition.position;
        }
        else
        {
            Debug.Log("CarExit point is not found");
            player.transform.position = currentCar.transform.position;
        }

        player.SetActive(true);
        playerCamera.gameObject.SetActive(true);
        currentCar = null;
        playerInCar = false;
    }
}
