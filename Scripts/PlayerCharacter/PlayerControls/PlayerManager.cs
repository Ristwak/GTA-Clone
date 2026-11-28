using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    CameraManager cameraManager;
    Animator animator;

    public bool isInteracting;

    void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (inputManager != null)
            inputManager.HandleAllInputs();

        if (cameraManager != null)
            cameraManager.HandleAllCameraMovement();
    }

    void FixedUpdate()
    {
        if (playerMovement != null)
            playerMovement.HandleAllMovement();
    }

    void LateUpdate()
    {
        if (animator == null || playerMovement == null)
            return;

        isInteracting = animator.GetBool("isInteracting");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}
