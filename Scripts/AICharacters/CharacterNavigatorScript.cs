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
    bool isDead;

    void Start()
    {
        presentHealth = characterHealth;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (isDead)
            return;
        Walk();
        if (!destinationReached)
        {
            currentTimeout += Time.deltaTime;
            if (currentTimeout >= waypointTimeout)
                KillCharacter();
        }
    }

    public void Walk()
    {
        if (isDead)
            return;
        Vector3 destinationDir = destination - transform.position;
        destinationDir.y = 0f;
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

    public void LocateDestination(Vector3 destination)
    {
        if (isDead)
            return;
        this.destination = destination;
        destinationReached = false;
        currentTimeout = 0f;
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
                gameManager.playerMoney += 10;
            }
        }
    }

    void CharacterDie()
    {
        if (isDead)
            return;
        isDead = true;
        movingSpeed = 0f;
        CapsuleCollider characterCollider = GetComponent<CapsuleCollider>();
        if (characterCollider != null)
            characterCollider.enabled = false;
        Destroy(gameObject, 4f);
    }

    void KillCharacter()
    {
        if (isDead)
            return;
        presentHealth = 0f;
        CharacterDie();
    }
}