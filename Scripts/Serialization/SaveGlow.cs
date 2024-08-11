using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGlow : MonoBehaviour
{
    GameManager gameManager;

    public GameObject saveText;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        saveText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PLayer")
        {
            gameManager.SavePlayer();
            saveText.SetActive(true);
        }
    }
}
