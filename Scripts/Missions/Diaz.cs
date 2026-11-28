using UnityEngine;

public class Diaz : MonoBehaviour
{
    float bossHealth = 120f;
    Animator animator;
    GameManager gameManager;
    public float damageAmount = 10f;
    public float shootingCooldown = 2f;
    float lastShotTime;
    public GameObject bloodEffect;
    public Transform shootingArea;
    bool isDead;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
            return;
        ShootPlayer();
    }

    public void CharacterHitDamage(float takeDamage)
    {
        if (isDead || takeDamage <= 0f)
            return;
        bossHealth = Mathf.Max(0f, bossHealth - takeDamage);
        if (bossHealth <= 0f)
            Die();
    }

    void Die()
    {
        if (isDead)
            return;
        isDead = true;
        if (gameManager != null)
        {
            if (gameManager.Mission1 && gameManager.Mission2 && gameManager.Mission3)
                gameManager.Mission4 = true;
            gameManager.playerMoney += 2000;
        }
        if (animator != null)
            animator.SetBool("Died", true);
        CapsuleCollider bossCollider = GetComponent<CapsuleCollider>();
        if (bossCollider != null)
            bossCollider.enabled = false;
        Destroy(gameObject, 4f);
    }

    void ShootPlayer()
    {
        if (shootingArea == null || Time.time - lastShotTime < shootingCooldown)
            return;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;
        Vector3 direction = (player.transform.position - shootingArea.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(shootingArea.position, direction, out hit))
        {
            if (!hit.collider.CompareTag("Player"))
                return;
            PlayerMovement playerHealth = hit.collider.GetComponentInParent<PlayerMovement>();
            if (playerHealth != null)
            {
                playerHealth.CharacterHitDamage(damageAmount);
                if (bloodEffect != null)
                {
                    GameObject effect = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(effect, 1f);
                }
                if (animator != null)
                    animator.SetBool("Shooting", true);
                lastShotTime = Time.time;
            }
        }
    }
}