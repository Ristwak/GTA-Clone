using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Mission1 : MonoBehaviour
{
    GameManager gameManager;
    InputManager inputManager;
    public Animator characterAnimator;
    public GameObject playerCharacter;
    public Text subtitleText;
    public string[] textArray;
    private float timer = 0f;
    private int currentIndex = 0;
    bool isCollided = false;
    public GameObject mainCamera;
    public GameObject missionCamera;
    public GameObject missionLight;
    PlayerMovement playerMovement;
    float originalWalkingSpeed;
    float originalRunningSpeed;
    float originalSprintingSpeed;
    bool movementSpeedsCached;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();
        if (playerCharacter != null)
        {
            playerMovement = playerCharacter.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                originalWalkingSpeed = playerMovement.walkingSpeed;
                originalRunningSpeed = playerMovement.runningSpeed;
                originalSprintingSpeed = playerMovement.sprintingSpeed;
                movementSpeedsCached = true;
            }
        }
        if (textArray != null && textArray.Length > 0 && subtitleText != null)
            subtitleText.text = textArray[0];
    }

    void Update()
    {
        if (subtitleText != null)
            subtitleText.gameObject.SetActive(isCollided);
        if (isCollided)
        {
            if (playerMovement != null)
            {
                playerMovement.walkingSpeed = 0f;
                playerMovement.runningSpeed = 0f;
                playerMovement.sprintingSpeed = 0f;
            }
            if (mainCamera != null)
                mainCamera.SetActive(false);
            if (missionCamera != null)
                missionCamera.SetActive(true);
            if (characterAnimator != null)
                characterAnimator.SetBool("StartTalking", true);
            if (textArray != null && textArray.Length > 0)
            {
                timer += Time.deltaTime;
                if (timer >= 10f)
                {
                    timer = 0f;
                    currentIndex = (currentIndex + 1) % textArray.Length;
                    if (subtitleText != null)
                        subtitleText.text = textArray[currentIndex];
                }
            }
        }
        if (gameManager == null)
            return;
        bool interactPressed = gameManager.useMobileInputs
            ? CrossPlatformInputManager.GetButtonDown("EKey")
            : inputManager != null && inputManager.interactInput;
        if (interactPressed && isCollided)
        {
            if (inputManager != null)
                inputManager.interactInput = false;
            CompleteMission();
        }
    }

    void CompleteMission()
    {
        isCollided = false;
        if (characterAnimator != null)
            characterAnimator.Play("Idle");
        if (subtitleText != null)
            subtitleText.text = "";
        Destroy(gameObject);
        if (characterAnimator != null)
            Destroy(characterAnimator.gameObject);
        RestorePlayerMovement();
        if (mainCamera != null)
            mainCamera.SetActive(true);
        if (missionCamera != null)
            missionCamera.SetActive(false);
        if (!gameManager.Mission1 && !gameManager.Mission2 && !gameManager.Mission3 && !gameManager.Mission4 && !gameManager.Mission5)
            gameManager.Mission1 = true;
    }

    void RestorePlayerMovement()
    {
        if (playerMovement == null || !movementSpeedsCached)
            return;
        playerMovement.walkingSpeed = originalWalkingSpeed;
        playerMovement.runningSpeed = originalRunningSpeed;
        playerMovement.sprintingSpeed = originalSprintingSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (missionLight != null)
                missionLight.SetActive(false);
            isCollided = true;
        }
    }
}