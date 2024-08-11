using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    GameManager gameManager;

    public Transform playerTransform;
    public Transform cameraPivot;
    private Vector3 camFollowVelocity = Vector3.zero;

    [Header("Camera Movement and Rotation")]
    public float camFollowSpeed = 0.1f;
    public float camLookSpeed = 0.1f;
    public float camPivotSpeed = 0.1f;
    public float lookAngle;
    public float pivotAngle;

    public float minPivotAngle = -30f;
    public float maxPivotAngle = 30f;

    [Header("Scoped Settings")]
    public float scopeFOV = 20f;
    public float defaultFOV = 60f;
    bool isScoped = false;
    PlayerMovement playerMovement;
    public Camera camera;

    [Header("Camera Sensitivity")]
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputManager = FindObjectOfType<InputManager>();
        playerTransform = FindObjectOfType<PlayerManager>().transform;
        playerMovement = FindObjectOfType<PlayerMovement>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleScopedFOV();
    }

    void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerTransform.position, ref camFollowVelocity, camFollowSpeed);
        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        // Use sensitivity values from MainMenu instance
        sensitivityX = MainMenu.instance.sensitivityX;
        sensitivityY = MainMenu.instance.sensitivityY;

        // Calculation Horizontal and Vertical Rotation
        lookAngle = lookAngle + (inputManager.cameraInputX * camLookSpeed * sensitivityX);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * camPivotSpeed * sensitivityY);

        // Restricting Camera 360 movement
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        // Horizontal Rotation
        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        // Vertical Rotation
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

        if (gameManager.useMobileInputs && isScoped == true)
        {
            camLookSpeed = 0.3f;
            camPivotSpeed = 0.3f;
            minPivotAngle = -0.5f;
            maxPivotAngle = 6f;

            playerTransform.rotation = Quaternion.Euler(pivotAngle, lookAngle, 0f);
        }
        else
        {
            camLookSpeed = 3f;
            camPivotSpeed = 1f;
            minPivotAngle = -30f;
            maxPivotAngle = 30f;
        }

        if (playerMovement.isMoving == false && playerMovement.isSprinting == false)
        {
            playerTransform.rotation = Quaternion.Euler(0f, lookAngle, 0f);
        }
    }

    private void HandleScopedFOV()
    {
        if (gameManager.useMobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButtonDown("Scope"))
            {
                camera.fieldOfView = scopeFOV;
                isScoped = true;
            }
            else
            {
                camera.fieldOfView = defaultFOV;
                isScoped = false;
            }
        }
        else
        {
            if (inputManager.aimInput)
            {
                camera.fieldOfView = scopeFOV;
                isScoped = true;
            }
            else
            {
                camera.fieldOfView = defaultFOV;
                isScoped = false;
            }
        }
    }
}