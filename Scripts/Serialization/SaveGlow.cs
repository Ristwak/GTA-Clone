using UnityEngine;

public class SaveGlow : MonoBehaviour
{
    GameManager gameManager;
    public GameObject saveText;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (saveText != null)
        {
            saveText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (gameManager != null)
        {
            gameManager.SavePlayer();
        }

        if (saveText != null)
        {
            saveText.SetActive(true);
        }
    }
}