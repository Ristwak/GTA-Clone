using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigatorScript : MonoBehaviour
{
    [Header("References")]
    GameManager gameManager;

    [Header("Charcater Info")]
    public float movingSpeed;
    public float turningSpeed = 300f;
    public float stopSpeed = 1f;

    [Header("Destination Variable")]
    public Vector3 destination;
    public bool destinationReached;
    public Animator animator;
    private float waypointTimeout = 30f;
    public float currentTimeout = 0f;

    [Header("Health")]
    private float characterHealth = 100;
    public float presentHealth;

    void Start()
    {
        presentHealth = characterHealth;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        Walk();
        if(!destinationReached)
        {
            currentTimeout += Time.deltaTime;
            if(currentTimeout >= waypointTimeout)
            {
                KillCharacter();
            }
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
        currentTimeout = 0f;
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
            gameManager.playerMoney += 10;
        }
    }

    void CharacterDie()
    {
        movingSpeed = 0f;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Object.Destroy(gameObject, 4f);
    }

    void KillCharacter()
    {
        presentHealth = 0;
        CharacterDie();
    }
}
