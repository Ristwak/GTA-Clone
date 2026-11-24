using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 100f;
    float currentHealth;
    GameManager gameManager;
    public Animator characterAnimator;
    bool isDead;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentHealth = maxHealth;
    }

    public void CharacterHitDamage(float damageAmount)
    {
        if (isDead || damageAmount <= 0f)
            return;
        currentHealth = Mathf.Max(0f, currentHealth - damageAmount);
        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        if (isDead)
            return;
        isDead = true;
        if (characterAnimator != null)
            characterAnimator.SetBool("Die", false);
        if (gameManager != null && gameManager.Mission1 && gameManager.Mission2 && !gameManager.Mission3 && !gameManager.Mission4 && !gameManager.Mission5)
        {
            gameManager.Mission3 = true;
            gameManager.playerMoney += 100;
        }
        Invoke(nameof(DestroyCharacter), 5f);
    }

    void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}