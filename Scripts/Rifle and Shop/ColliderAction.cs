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
        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();

        if (shopCamera != null) shopCamera.SetActive(false);
        if (notificationText != null) notificationText.gameObject.SetActive(false);
        if (priceText != null) priceText.SetActive(false);

        previousInteractInput = false;
        previousMobileInteractInput = false;
    }

    void Update()
    {
        GameobjectActive();
    }

    void GameobjectActive()
    {
        if (!playerTriggered || inputManager == null) return;

        bool currentMobileInteractInput = CrossPlatformInputManager.GetButton("EKey");

        if (currentMobileInteractInput && !previousMobileInteractInput)
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

    void ToggleShop()
    {
        inShop = !inShop;

        if (mainCamera != null) mainCamera.SetActive(!inShop);
        if (shopCamera != null) shopCamera.SetActive(inShop);
        if (priceText != null) priceText.SetActive(inShop);
        if (notificationText != null) notificationText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerTriggered = true;
        previousInteractInput = false;
        previousMobileInteractInput = false;

        if (notificationText != null)
        {
            notificationText.text = "Press E";
            notificationText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerTriggered = false;
        ResetToDefaultState();
    }

    void ResetToDefaultState()
    {
        inShop = false;

        if (mainCamera != null) mainCamera.SetActive(true);
        if (shopCamera != null) shopCamera.SetActive(false);
        if (priceText != null) priceText.SetActive(false);
        if (notificationText != null) notificationText.gameObject.SetActive(false);

        previousInteractInput = false;
        previousMobileInteractInput = false;
    }
}