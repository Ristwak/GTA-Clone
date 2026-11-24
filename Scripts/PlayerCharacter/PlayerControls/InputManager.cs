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
            playerControls.PlayerMovement.Movement.canceled += i => movementInput = Vector2.zero;
            playerControls.PlayerMovement.CameraMovement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.CameraMovement.canceled += i => cameraInput = Vector2.zero;
            playerControls.PlayerActions.B.performed += i => bInput = true;
            playerControls.PlayerActions.B.canceled += i => bInput = false;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Interact.performed += i =>
            {
                interactInput = true;
                carInteractInput = true;
            };
            playerControls.PlayerActions.Interact.canceled += i =>
            {
                interactInput = false;
                carInteractInput = false;
            };
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
            playerControls.PlayerActions.Pause.performed += i => pauseInput = true;
        }
        playerControls.Enable();
    }

    void OnDisable()
    {
        if (playerControls != null)
            playerControls.Disable();
        movementInput = Vector2.zero;
        cameraInput = Vector2.zero;
        bInput = false;
        jumpInput = false;
        interactInput = false;
        previousInput = false;
        nextInput = false;
        buyInput = false;
        changeInput = false;
        shootInput = false;
        aimInput = false;
        reloadInput = false;
        carInteractInput = false;
        pauseInput = false;
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleNextInput();
        HandlePreviousInput();
        HandleBuyInput();
    }

    void HandleMovementInput()
    {
        if (gameManager == null || playerMovement == null)
            return;
        if (gameManager.useMobileInputs)
        {
            verticalInput = movementJoyStick != null ? movementJoyStick.Vertical : 0f;
            horizontalInput = movementJoyStick != null ? movementJoyStick.Horizontal : 0f;
            cameraInputX = cameraJoyStick != null ? cameraJoyStick.Horizontal : 0f;
            cameraInputY = cameraJoyStick != null ? cameraJoyStick.Vertical : 0f;
        }
        else
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            cameraInputX = cameraInput.x;
            cameraInputY = cameraInput.y;
        }
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        if (animatorManager != null)
            animatorManager.UpdateAnimValues(0f, moveAmount, playerMovement.isSprinting);
    }

    void HandleSprintingInput()
    {
        if (gameManager == null || playerMovement == null)
            return;
        bool mobileSprint = gameManager.useMobileInputs && CrossPlatformInputManager.GetButton("Sprint");
        bool keyboardSprint = !gameManager.useMobileInputs && bInput;
        playerMovement.isSprinting = (mobileSprint || keyboardSprint) && moveAmount > 0.5f;
    }

    void HandleJumpingInput()
    {
        if (gameManager == null || playerMovement == null)
            return;
        bool jumpPressed = gameManager.useMobileInputs
            ? CrossPlatformInputManager.GetButtonDown("Jump")
            : jumpInput;
        if (!jumpPressed)
            return;
        jumpInput = false;
        playerMovement.isJumping = true;
        playerMovement.HandleJumping();
    }

    void HandleNextInput()
    {
        if (!nextInput)
            return;
        if (cameraPositionManager != null)
            cameraPositionManager.NextPosition();
        nextInput = false;
    }

    void HandlePreviousInput()
    {
        if (!previousInput)
            return;
        if (cameraPositionManager != null)
            cameraPositionManager.PreviousPosition();
        previousInput = false;
    }

    void HandleBuyInput()
    {
        if (!buyInput)
            return;
        if (cameraPositionManager != null)
            cameraPositionManager.BuyItem();
        if (gameManager != null && gameManager.Mission1 && !gameManager.Mission2 && !gameManager.Mission3 && !gameManager.Mission4 && !gameManager.Mission5)
            gameManager.Mission2 = true;
        buyInput = false;
    }
}