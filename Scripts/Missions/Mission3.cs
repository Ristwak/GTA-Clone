using UnityEngine;
using UnityEngine.UI;

public class Mission3 : MonoBehaviour
{
    public Animator characterAnimator;
    public Animator cameraAnimator;
    public GameObject playerCharacter;
    public Text subtitleText;
    public string[] textArray;
    private float timer = 0f;
    private int currentIndex = 0;
    bool isCollided = false;
    public GameObject mainCamera;
    public GameObject missionCamera;
    public float targetTime = 10f;
    PlayerMovement playerMovement;
    float originalWalkingSpeed;
    float originalRunningSpeed;
    float originalSprintingSpeed;
    bool movementSpeedsCached;

    void Start()
    {
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
        if (!isCollided)
            return;
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
        if (cameraAnimator != null)
            cameraAnimator.SetBool("StartMoving", true);
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
        targetTime -= Time.deltaTime;
        if (targetTime <= 0f)
            CompleteMission();
    }

    void CompleteMission()
    {
        isCollided = false;
        if (subtitleText != null)
            subtitleText.text = "";
        Destroy(gameObject);
        if (playerMovement != null && movementSpeedsCached)
        {
            playerMovement.walkingSpeed = originalWalkingSpeed;
            playerMovement.runningSpeed = originalRunningSpeed;
            playerMovement.sprintingSpeed = originalSprintingSpeed;
        }
        if (mainCamera != null)
            mainCamera.SetActive(true);
        if (missionCamera != null)
            missionCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isCollided = true;
    }
}