using UnityEngine;

public class PoliceOfficer : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float runningSpeed;
    float currentMovingSpeed;
    public float turningSpeed = 300f;
    public float stopSpeed = 1f;
    bool isDead = false;
    [Header("Character Health")]
    float characterHealth = 100f;
    public float presentHealth;
    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;
    Animator animator;
    [Header("Police AI")]
    GameObject playerBody;
    public LayerMask playerLayer;
    public float visionRadius;
    public float shootingRadius;
    public bool playerInVisionRadius;
    public bool playerInShootingRadius;
    [Header("Police Shooting Variables")]
    public float giveDamageOf = 3f;
    public float shootingRange = 100f;
    public GameObject shootingRaycastArea;
    public float timeBtwShoot;
    bool previouslyShoot;
    public GameObject bloodEffect;
    [Header("Script Refs")]
    WantedLevel wantedLevelScript;
    GameManager gameManager;

    void Start()
    {
        wantedLevelScript = FindObjectOfType<WantedLevel>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        currentMovingSpeed = movingSpeed;
        presentHealth = characterHealth;
        playerBody = GameObject.Find("Player");
    }

    void Update()
    {
        if (isDead)
            return;
        if (playerBody == null)
            playerBody = GameObject.Find("Player");
        if (wantedLevelScript == null)
            wantedLevelScript = FindObjectOfType<WantedLevel>();
        if (playerBody == null || wantedLevelScript == null)
            return;
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);
        bool isWanted = wantedLevelScript.Level1 || wantedLevelScript.Level2 || wantedLevelScript.Level3 || wantedLevelScript.Level4 || wantedLevelScript.Level5;
        if (!isWanted)
        {
            Walk();
            return;
        }
        if (playerInVisionRadius && playerInShootingRadius)
        {
            ShootPlayer();
        }
        else if (playerInVisionRadius)
        {
            ChasePlayer();
        }
        else
        {
            Walk();
        }
    }

    public void Walk()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDir = destination - transform.position;
            destinationDir.y = 0;
            float destinationDistance = destinationDir.magnitude;
            if (destinationDistance >= stopSpeed)
            {
                destinationReached = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
                if (animator != null)
                {
                    animator.SetBool("Walk", true);
                    animator.SetBool("Shoot", false);
                    animator.SetBool("Run", false);
                }
            }
            else
            {
                destinationReached = true;
            }
        }
        else
        {
            destinationReached = true;
        }
    }

    public void LocateDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;
    }

    public void ChasePlayer()
    {
        if (playerBody == null)
            return;
        currentMovingSpeed = runningSpeed;
        transform.LookAt(playerBody.transform);
        transform.position += transform.forward * currentMovingSpeed * Time.deltaTime;
        if (animator != null)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Shooting", false);
        }
    }

    void ShootPlayer()
    {
        if (playerBody == null)
            return;
        currentMovingSpeed = 0f;
        transform.LookAt(playerBody.transform);
        if (animator != null)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("Shoot", true);
        }
        if (previouslyShoot || shootingRaycastArea == null)
            return;
        RaycastHit hit;
        if (Physics.Raycast(shootingRaycastArea.transform.position, shootingRaycastArea.transform.forward, out hit, shootingRange))
        {
            Debug.Log("Police is shooting at " + hit.transform.name);
            PlayerMovement playerScript = hit.transform.GetComponentInParent<PlayerMovement>();
            if (playerScript != null)
            {
                playerScript.CharacterHitDamage(giveDamageOf);
                if (bloodEffect != null)
                {
                    GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(bloodEffectGo, 1f);
                }
            }
        }
        previouslyShoot = true;
        Invoke(nameof(ActiveShooting), timeBtwShoot);
    }

    void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void CharacterHitDamage(float takeDamage)
    {
        if (isDead || takeDamage <= 0f)
            return;
        presentHealth = Mathf.Max(0f, presentHealth - takeDamage);
        if (presentHealth <= 0f)
        {
            if (animator != null)
                animator.SetBool("Die", true);
            CharacterDie();
            if (gameManager != null)
            {
                gameManager.currentKills += 1;
                gameManager.playerMoney += 50;
            }
        }
    }

    void CharacterDie()
    {
        if (isDead)
            return;
        isDead = true;
        shootingRadius = 0f;
        movingSpeed = 0f;
        currentMovingSpeed = 0f;
        CapsuleCollider policeCollider = GetComponent<CapsuleCollider>();
        if (policeCollider != null)
            policeCollider.enabled = false;
        CancelInvoke(nameof(ActiveShooting));
        Destroy(gameObject, 4f);
    }
}