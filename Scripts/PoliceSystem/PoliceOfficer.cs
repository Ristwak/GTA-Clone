using System.Collections;
using System.Collections.Generic;
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
        if (isDead == true)
            return;
        
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if (wantedLevelScript.Level1 == false && wantedLevelScript.Level2 == false && wantedLevelScript.Level3 == false && wantedLevelScript.Level4 == false && wantedLevelScript.Level5 == false)
        {
            Walk();
        }

        if (playerInVisionRadius && !playerInShootingRadius && wantedLevelScript.Level1 == true || wantedLevelScript.Level2 == true || wantedLevelScript.Level3 == true || wantedLevelScript.Level4 == true || wantedLevelScript.Level5 == true)
        {
            ChasePlayer();
        }

        if (playerInVisionRadius && playerInShootingRadius && wantedLevelScript.Level1 == true || wantedLevelScript.Level2 == true || wantedLevelScript.Level3 == true || wantedLevelScript.Level4 == true || wantedLevelScript.Level5 == true)
        {
            ShootPlayer();
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

                animator.SetBool("Walk", true);
                animator.SetBool("Shoot", false);
                animator.SetBool("Run", false);
            }
            else
            {
                destinationReached = true;
            }
        }
    }

    public void LocateDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;
    }

    public void ChasePlayer()
    {
        transform.position += transform.forward * currentMovingSpeed * Time.deltaTime;
        transform.LookAt(playerBody.transform);

        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Shooting", false);

        currentMovingSpeed = runningSpeed;
    }

    void ShootPlayer()
    {
        currentMovingSpeed = 0f;
        transform.LookAt(playerBody.transform);

        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Shoot", true);

        if (!previouslyShoot)
        {
            RaycastHit hit;
            if (Physics.Raycast(shootingRaycastArea.transform.position, shootingRaycastArea.transform.forward, out hit, shootingRange))
            {
                Debug.Log("Police is shooting at " + hit.transform.name);

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
            gameManager.playerMoney += 50;
        }
    }

    void CharacterDie()
    {
        isDead = true;
        shootingRadius = 0f;
        movingSpeed = 0f;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Object.Destroy(gameObject, 4f);
    }
}
