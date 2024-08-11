using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerMovement playerMovement;
    CameraPositionManager cameraPositionManager;
    GameManager gameManager;

    public float moveAmount;
    private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    private Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;

    [Header("Input Button Flags")]
    public bool jumpInput;
    public bool bInput;
    public bool interactInput;
    public bool previousInput;
    public bool nextInput;
    public bool buyInput;
    public bool changeInput;
    public bool shootInput;
    public bool aimInput;
    public bool reloadInput;
    public bool carInteractInput;
    public bool pauseInput;

    [Header("Joysticks")]
    public FixedJoystick movementJoyStick;
    public FixedJoystick cameraJoyStick;

    void Awake()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraPositionManager = FindObjectOfType<CameraPositionManager>();
        gameManager = FindObjectOfType<GameManager>();
    }
    void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.CameraMovement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.B.performed += i => bInput = true;
            playerControls.PlayerActions.B.canceled += i => bInput = false;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Interact.performed += i => interactInput = true;
            playerControls.PlayerActions.Previous.performed += i => previousInput = true;
            playerControls.PlayerActions.Next.performed += i => nextInput = true;
            playerControls.PlayerActions.Buy.performed += i => buyInput = true;
            playerControls.PlayerActions.ChangeRifle.performed += i => changeInput = true;
            playerControls.PlayerActions.Shoot.performed += i => shootInput = true;
            playerControls.PlayerActions.Shoot.canceled += i => shootInput = false;
            playerControls.PlayerActions.Scope.performed += i => aimInput = true;
            playerControls.PlayerActions.Scope.canceled += i => aimInput = false;
            playerControls.PlayerActions.Reload.performed += i => reloadInput = true;
            playerControls.PlayerActions.Reload.canceled += i => reloadInput = false;
            playerControls.PlayerActions.Interact.performed += i => carInteractInput = true;
            playerControls.PlayerActions.Interact.canceled += i => carInteractInput = false;
            playerControls.PlayerActions.Pause.performed += i => pauseInput = true;
        }

        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleInteractInput();
        HandleNextInput();
        HandlePreviousInput();
        HandleBuyInput();
        HandleRifleChangeInput();
        HandlePauseInput();
    }

    void HandleMovementInput()
    {
        if (gameManager.useMobileInputs)
        {
            verticalInput = movementJoyStick.Vertical;
            horizontalInput = movementJoyStick.Horizontal;

            cameraInputX = cameraJoyStick.Horizontal;
            cameraInputY = cameraJoyStick.Vertical;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            animatorManager.UpdateAnimValues(0, moveAmount, playerMovement.isSprinting);
        }
        else
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            cameraInputX = cameraInput.x;
            cameraInputY = cameraInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            animatorManager.UpdateAnimValues(0, moveAmount, playerMovement.isSprinting);
        }
    }

    void HandleSprintingInput()
    {
        if (gameManager.useMobileInputs && CrossPlatformInputManager.GetButton("Sprint"))
        {
            playerMovement.isSprinting = true;
        }

        if (bInput && moveAmount > 0.5f)
        {
            playerMovement.isSprinting = true;
        }
        else
        {
            playerMovement.isSprinting = false;
        }
    }

    void HandleJumpingInput()
    {
        if (gameManager.useMobileInputs && CrossPlatformInputManager.GetButton("Jump"))
        {
            jumpInput = false;
            playerMovement.isJumping = true;
            playerMovement.HandleJumping();
        }

        if (jumpInput)
        {
            jumpInput = false;
            playerMovement.isJumping = true;
            playerMovement.HandleJumping();
        }
    }

    void HandleInteractInput()
    {
        if (jumpInput)
        {
            interactInput = false;
        }
    }

    void HandleNextInput()
    {
        if (nextInput)
        {
            cameraPositionManager.NextPosition();
            nextInput = false;
        }
    }

    void HandlePreviousInput()
    {
        if (previousInput)
        {
            cameraPositionManager.PreviousPosition();
            previousInput = false;
        }
    }

    void HandleBuyInput()
    {
        if (buyInput)
        {
            cameraPositionManager.BuyItem();
            if (gameManager.Mission1 == true && gameManager.Mission2 == false && gameManager.Mission3 == false && gameManager.Mission4 == false && gameManager.Mission5 == false)
            {
                gameManager.Mission2 = true;
            }
            buyInput = false;
        }
    }

    void HandleRifleChangeInput()
    {
        if (changeInput)
        {
            changeInput = false;
        }
    }

    void HandlePauseInput()
    {
        if (pauseInput)
        {
            pauseInput = false;
        }
    }
}
