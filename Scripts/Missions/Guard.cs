using System.Collections;
using System.Collections.Generic;
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
        if (isDead == true)
            return;

        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if (!playerInVisionRadius && !playerInShootingRadius)
        {
            Idle();
        }

        if (playerInVisionRadius && !playerInShootingRadius)
        {
            ChasePlayer();
        }

        if (playerInVisionRadius && playerInShootingRadius)
        {
            ShootPlayer();
        }
    }

    public void Idle()
    {
        currentMovingSpeed = 0f;
        animator.SetBool("Run", false);
        animator.SetBool("Shoot", false);
    }

    public void ChasePlayer()
    {
        transform.position += transform.forward * currentMovingSpeed * Time.deltaTime;
        transform.LookAt(playerBody.transform);

        animator.SetBool("Run", true);
        animator.SetBool("Shooting", false);

        currentMovingSpeed = runningSpeed;
    }

    void ShootPlayer()
    {
        currentMovingSpeed = 0f;
        transform.LookAt(playerBody.transform);

        animator.SetBool("Run", false);
        animator.SetBool("Shoot", true);

        if (!previouslyShoot)
        {
            RaycastHit hit;
            if (Physics.Raycast(shootingRaycastArea.transform.position, shootingRaycastArea.transform.forward, out hit, shootingRange))
            {
                Debug.Log("Guard is shooting at " + hit.transform.name);

                PlayerMovement playerScript = hit.transform.GetComponent<PlayerMovement>();

                if (playerScript != null)
                {
                    playerScript.CharacterHitDamage(giveDamageOf);
                    GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(bloodEffectGo, 1f);
                }
            }

            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timeBtwShoot);
        }
    }

    void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void CharacterHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if (presentHealth <= 0)
        {
            animator.SetBool("Die", true);
            // kill character
            CharacterDie();
            // increase player kills and player money
            gameManager.currentKills += 1;
            gameManager.playerMoney += 30;
        }
    }

    void CharacterDie()
    {
        isDead = true;
        shootingRadius = 0f;
        currentMovingSpeed = 0f;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Object.Destroy(gameObject, 4f);
    }
}
