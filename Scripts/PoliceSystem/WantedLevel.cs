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
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.LogError("WantedLevel: GameManager not found.", this);
    }

    void Update()
    {
        if (gameManager == null)
            return;
        int kills = gameManager.currentKills;
        Level1 = kills >= 1;
        Level2 = kills >= 3;
        Level3 = kills >= 5;
        Level4 = kills >= 10;
        Level5 = kills >= 15;
        SetStarColor(level1Star, Level1);
        SetStarColor(level2Star, Level2);
        SetStarColor(level3Star, Level3);
        SetStarColor(level4Star, Level4);
        SetStarColor(level5Star, Level5);
    }

    void SetStarColor(Image star, bool isVisible)
    {
        if (star == null)
            return;
        Color color = star.color;
        color.a = isVisible ? 1f : alphaValue;
        star.color = color;
    }
}