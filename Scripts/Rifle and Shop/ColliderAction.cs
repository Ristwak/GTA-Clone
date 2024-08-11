using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ColliderAction : MonoBehaviour
{
    InputManager inputManager;
    GameManager gameManager;
    public GameObject mainCamera;
    public GameObject shopCamera;

    public Text notificationText;
    public GameObject priceText;

    public bool playerTriggered = false;
    public bool inShop;

    bool previousInteractInput;
    bool previousMobileInteractInput;

    void Start()
    {
        shopCamera.SetActive(false);
        notificationText.gameObject.SetActive(false);
        priceText.SetActive(false);

        gameManager = FindAnyObjectByType<GameManager>();
        inputManager = FindAnyObjectByType<InputManager>();

        previousInteractInput = false;
        previousMobileInteractInput = false;
    }

    void Update()
    {
        GameobjectActive();
    }

    void GameobjectActive()
    {
        if (playerTriggered)
        {
            bool currentMobileInteractInput = CrossPlatformInputManager.GetButton("EKey");

            if(currentMobileInteractInput && !previousMobileInteractInput)
            {
                ToggleShop();
            }
            if (inputManager.interactInput && !previousInteractInput)
            {
                ToggleShop();
            }

            previousMobileInteractInput = currentMobileInteractInput;
            previousInteractInput = inputManager.interactInput;
        }
    }

    void ToggleShop()
    {
        if (inShop)
        {
            mainCamera.SetActive(true);
            shopCamera.SetActive(false);
            priceText.SetActive(false);
        }
        else
        {
            mainCamera.SetActive(false);
            shopCamera.SetActive(true);
            priceText.SetActive(true);
        }
        notificationText.gameObject.SetActive(false);
        inShop = !inShop;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            notificationText.text = "Press E";
            notificationText.gameObject.SetActive(true);
            playerTriggered = true;
            previousInteractInput = false; // Reset interact input state
            previousMobileInteractInput = false; // Reset mobile interact input state
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTriggered = false;
            ResetToDefaultState();
        }
    }

    void ResetToDefaultState()
    {
        mainCamera.SetActive(true);
        shopCamera.SetActive(false);
        priceText.SetActive(false);
        notificationText.gameObject.SetActive(false);
        inShop = false;
    }
}
