using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();

        if (textArray.Length > 0)
        {
            subtitleText.text = textArray[0];
        }
    }

    void Update()
    {
        if (!isCollided)
        {
            subtitleText.gameObject.SetActive(false);
        }
        else
        {
            subtitleText.gameObject.SetActive(true);
        }

        if (isCollided)
        {
            if (playerCharacter != null)
            {
                PlayerMovement playerMovement = playerCharacter.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.walkingSpeed = 0f;
                    playerMovement.runningSpeed = 0f;
                    playerMovement.sprintingSpeed = 0f;
                }
            }

            mainCamera.SetActive(false);
            missionCamera.SetActive(true);

            characterAnimator.SetBool("StartTalking", true);
            timer += Time.deltaTime;

            if (timer >= 10f)
            {
                timer = 0f;
                currentIndex = (currentIndex + 1) % textArray.Length;
                subtitleText.text = textArray[currentIndex];
            }
        }
        if (gameManager.useMobileInputs)
        {
            if (CrossPlatformInputManager.GetButton("EKey") && isCollided == true)
            {
                isCollided = false;

                characterAnimator.Play("Idle");
                subtitleText.text = "";

                Destroy(gameObject);
                Destroy(characterAnimator.gameObject);

                if (playerCharacter != null)
                {
                    PlayerMovement playerMovement = playerCharacter.GetComponent<PlayerMovement>();
                    if (playerMovement != null)
                    {
                        playerMovement.walkingSpeed = 1.5f;
                        playerMovement.runningSpeed = 5f;
                        playerMovement.sprintingSpeed = 7f;
                    }
                }

                mainCamera.SetActive(true);
                missionCamera.SetActive(false);

                if (gameManager.Mission1 == false && gameManager.Mission2 == false && gameManager.Mission3 == false && gameManager.Mission4 == false && gameManager.Mission5 == false)
                {
                    gameManager.Mission1 = true;
                }
            }
        }
        if (inputManager.interactInput && isCollided == true)
        {
            isCollided = false;

            characterAnimator.Play("Idle");
            subtitleText.text = "";

            Destroy(gameObject);
            Destroy(characterAnimator.gameObject);

            if (playerCharacter != null)
            {
                PlayerMovement playerMovement = playerCharacter.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.walkingSpeed = 1.5f;
                    playerMovement.runningSpeed = 5f;
                    playerMovement.sprintingSpeed = 7f;
                }
            }

            mainCamera.SetActive(true);
            missionCamera.SetActive(false);

            if (gameManager.Mission1 == false && gameManager.Mission2 == false && gameManager.Mission3 == false && gameManager.Mission4 == false && gameManager.Mission5 == false)
            {
                gameManager.Mission1 = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            missionLight.SetActive(false);
            isCollided = true;
        }
    }
}
