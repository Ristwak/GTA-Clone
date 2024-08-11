using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Lance : MonoBehaviour
{
    float bossHealth = 120f;
    Animator animator;
    GameManager gameManager;
    public float damageAmount = 10f;
    public float shootingCooldown = 2f;
    float lastShotTime;
    public GameObject bloodEffect;
    public Transform shootingArea;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (bossHealth <= 0)
        {
            if (gameManager.Mission1 && gameManager.Mission2 && gameManager.Mission3 && gameManager.Mission4)
            {
                gameManager.Mission5 = true;
            }

            Object.Destroy(gameObject, 4f);
            animator.SetBool("Died", true);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }

        IncreaseMoney();
        ShootPlayer();
    }

    public void CharacterHitDamage(float takeDamage)
    {
        bossHealth -= takeDamage;
    }

    void IncreaseMoney()
    {
        if (bossHealth <= 0)
        {
            gameManager.playerMoney += 2000;
        }
    }

    void ShootPlayer()
    {
        if (Time.time - lastShotTime >= shootingCooldown)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                RaycastHit hit;
                Vector3 direction = (player.transform.position - transform.position).normalized;
                if (Physics.Raycast(shootingArea.position, direction, out hit))
                {
                    // Check if hit player
                    if (hit.collider.CompareTag("Player"))
                    {
                        // Damage Player
                        PlayerMovement playerHealth = hit.collider.GetComponent<PlayerMovement>();
                        if (playerHealth != null)
                        {
                            playerHealth.CharacterHitDamage(damageAmount);

                            // Instantiate blood effect at hit point
                            GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                        }

                        // update last shot time
                        lastShotTime = Time.time;
                    }
                }
            }
        }
    }
}
