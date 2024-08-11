using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WantedLevel : MonoBehaviour
{
    GameManager gameManager;
    public float alphaValue = 0.31f;

    [Header("Star Image")]
    public Image level1Star;
    public Image level2Star;
    public Image level3Star;
    public Image level4Star;
    public Image level5Star;

    [Header("Wanted Level")]
    public bool Level1 = false;
    public bool Level2 = false;
    public bool Level3 = false;
    public bool Level4 = false;
    public bool Level5 = false;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.currentKills == 1)
        {
            SetStarColor(level1Star, true);
            Level1 = true;
        }

        if (gameManager.currentKills == 3)
        {
            SetStarColor(level2Star, true);
            Level2 = true;
        }

        if (gameManager.currentKills == 5)
        {
            SetStarColor(level3Star, true);
            Level3 = true;
        }

        if (gameManager.currentKills == 10)
        {
            SetStarColor(level4Star, true);
            Level4 = true;
        }

        if (gameManager.currentKills == 15)
        {
            SetStarColor(level5Star, true);
            Level5 = true;
        }
        
        SetStarColor(level1Star, Level1);
        SetStarColor(level2Star, Level2);
        SetStarColor(level3Star, Level3);
        SetStarColor(level4Star, Level4);
        SetStarColor(level5Star, Level5);
    }

    void SetStarColor(Image star, bool isVisible)
    {
        Color color = star.color;
        color.a = isVisible ? 1f : alphaValue;
        star.color = color;
    }
}
