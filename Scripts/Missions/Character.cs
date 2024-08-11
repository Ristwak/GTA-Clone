using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 100f;
    float currentHealth;
    GameManager gameManager;
    public Animator characterAnimator;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentHealth = maxHealth;
    }

    public void CharacterHitDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // kill character
            Die();
        }
    }

    void Die()
    {
        characterAnimator.SetBool("Die", false);

        if (gameManager.Mission1 == true && gameManager.Mission2 == true && gameManager.Mission3 == false && gameManager.Mission4 == false && gameManager.Mission5 == false)
        {
            gameManager.Mission3 = true;
            gameManager.playerMoney += 100;
        }

        Invoke("DestroyCharacter", 5f);
    }

    void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}
