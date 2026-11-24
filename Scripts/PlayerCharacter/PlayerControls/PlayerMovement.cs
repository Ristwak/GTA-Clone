using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Character Health")]
    float characterHealth = 100f;
    public float presentHealth;
    HealthBar healthBar;

    [Header("Scripts Ref")]
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;

    [Header("Movement")]
    Vector3 moveDirection;
    public Transform camObject;
    Rigidbody playerRigidBody;

    [Header("Movement Flags")]
    public bool isMoving;
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Values")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float rotationSpeed = 12f;

    [Header("Falling and Landing")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Jump Variables")]
    public float jumpHeight = 4f;
    public float gravityIntensity = -15f;

    bool isDead;

    void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        inputManager = FindObjectOfType<InputManager>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();

        presentHealth = characterHealth;

        if (healthBar != null)
            healthBar.GiveFullHealth(presentHealth);
    }

    public void HandleAllMovement()
    {
        if (isDead || playerRigidBody == null)
            return;

        HandleFallingandLanding();

        if (playerManager != null && playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        if (isJumping || inputManager == null || camObject == null)
            return;

        moveDirection = camObject.forward * inputManager.verticalInput;
        moveDirection += camObject.right * inputManager.horizontalInput;

        moveDirection.Normalize();
        moveDirection.y = 0f;

        if (isSprinting)
        {
            moveDirection *= sprintingSpeed;
        }
        else if (inputManager.moveAmount >= 0.5f)
        {
            moveDirection *= runningSpeed;
            isMoving = true;
        }
        else
        {
            moveDirection *= walkingSpeed;
            isMoving = false;
        }

        // Preserve vertical velocity so movement does not overwrite jumping/falling.
        Vector3 movementVelocity = playerRigidBody.velocity;
        movementVelocity.x = moveDirection.x;
        movementVelocity.z = moveDirection.z;

        playerRigidBody.velocity = movementVelocity;
    }

    void HandleRotation()
    {
        if (isJumping || inputManager == null || camObject == null)
            return;

        Vector3 targetDirection =
            camObject.forward * inputManager.verticalInput;

        targetDirection += camObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0f;

        if (targetDirection.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void HandleFallingandLanding()
    {
        if (playerRigidBody == null)
            return;

        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffset;

        Vector3 targetPosition = transform.position;

        if (!isGrounded && !isJumping)
        {
            if (playerManager == null || !playerManager.isInteracting)
            {
                if (animatorManager != null)
                    animatorManager.PlayTargetAnim("Falling", true);
            }

            inAirTimer += Time.deltaTime;

            playerRigidBody.AddForce(
                transform.forward * leapingVelocity
            );

            playerRigidBody.AddForce(
                -Vector3.up * fallingVelocity * inAirTimer
            );
        }

        RaycastHit hit;

        // Correct SphereCast overload: max distance and LayerMask are separate.
        float groundCheckDistance = rayCastHeightOffset + 0.3f;

        if (Physics.SphereCast(
            rayCastOrigin,
            0.3f,
            Vector3.down,
            out hit,
            groundCheckDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore))
        {
            if (!isGrounded &&
                (playerManager == null || !playerManager.isInteracting))
            {
                if (animatorManager != null)
                    animatorManager.PlayTargetAnim("Landing", true);
            }

            targetPosition.y = hit.point.y;
            inAirTimer = 0f;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            if (playerManager != null &&
                playerManager.isInteracting ||
                inputManager != null && inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    Time.deltaTime / 0.1f
                );
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (!isGrounded || isDead ||
            playerRigidBody == null || animatorManager == null)
            return;

        animatorManager.animator.SetBool("isJumping", true);
        animatorManager.PlayTargetAnim("Jump", false);

        float jumpingVelocity = Mathf.Sqrt(
            -2f * gravityIntensity * jumpHeight
        );

        Vector3 playerVelocity = playerRigidBody.velocity;
        playerVelocity.x = moveDirection.x;
        playerVelocity.z = moveDirection.z;
        playerVelocity.y = jumpingVelocity;

        playerRigidBody.velocity = playerVelocity;

        isJumping = false;
    }

    public void SetIsJumping(bool jumping)
    {
        isJumping = jumping;
    }

    public void CharacterHitDamage(float takeDamage)
    {
        if (isDead || takeDamage <= 0f)
            return;

        presentHealth = Mathf.Max(0f, presentHealth - takeDamage);

        if (healthBar != null)
            healthBar.SetHealth(presentHealth);

        if (presentHealth <= 0f)
            CharacterDie();
    }

    void CharacterDie()
    {
        if (isDead)
            return;

        isDead = true;

        if (playerRigidBody != null)
        {
            playerRigidBody.velocity = Vector3.zero;
            playerRigidBody.isKinematic = true;
        }

        Destroy(gameObject);
    }
}
