using UnityEngine;

public class Guard : MonoBehaviour
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
    Animator animator;
    [Header("Guard AI")]
    GameObject playerBody;
    public LayerMask playerLayer;
    public float visionRadius;
    public float shootingRadius;
    public bool playerInVisionRadius;
    public bool playerInShootingRadius;
    [Header("Guard Shooting Variables")]
    public float giveDamageOf = 3f;
    public float shootingRange = 100f;
    public GameObject shootingRaycastArea;
    public float timeBtwShoot;
    bool previouslyShoot;
    public GameObject bloodEffect;
    [Header("Script Refs")]
    GameManager gameManager;

    void Start()
    {
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
        if (playerBody == null)
        {
            Idle();
            return;
        }
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);
        if (!playerInVisionRadius)
        {
            Idle();
        }
        else if (playerInShootingRadius)
        {
            ShootPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    public void Idle()
    {
        currentMovingSpeed = 0f;
        if (animator != null)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Shoot", false);
        }
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
            animator.SetBool("Run", false);
            animator.SetBool("Shoot", true);
        }
        if (previouslyShoot || shootingRaycastArea == null)
            return;
        RaycastHit hit;
        if (Physics.Raycast(shootingRaycastArea.transform.position, shootingRaycastArea.transform.forward, out hit, shootingRange))
        {
            Debug.Log("Guard is shooting at " + hit.transform.name);
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
                gameManager.playerMoney += 30;
            }
        }
    }

    void CharacterDie()
    {
        if (isDead)
            return;
        isDead = true;
        shootingRadius = 0f;
        currentMovingSpeed = 0f;
        CapsuleCollider guardCollider = GetComponent<CapsuleCollider>();
        if (guardCollider != null)
            guardCollider.enabled = false;
        CancelInvoke(nameof(ActiveShooting));
        Destroy(gameObject, 4f);
    }
}