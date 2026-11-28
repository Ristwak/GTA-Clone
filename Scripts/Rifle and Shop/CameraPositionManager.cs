using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CameraPositionManager : MonoBehaviour
{
    public Transform[] cameraPositions;
    public int currentPositionIndex = 0;
    GameManager gameManager;
    ColliderAction colliderAction;
    public Text priceText;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        colliderAction = FindObjectOfType<ColliderAction>();
        if (cameraPositions == null || cameraPositions.Length == 0)
        {
            Debug.LogError("CameraPositionManager: Camera positions are missing.", this);
            enabled = false;
            return;
        }
        if (currentPositionIndex < 0 || currentPositionIndex >= cameraPositions.Length)
        {
            Debug.LogError("CameraPositionManager: Current position index is invalid.", this);
            enabled = false;
            return;
        }
        if (cameraPositions[currentPositionIndex] == null)
        {
            Debug.LogError("CameraPositionManager: Selected camera position is missing.", this);
            enabled = false;
            return;
        }
        MoveCamera();
    }

    void Update()
    {
        if (gameManager == null || colliderAction == null)
            return;
        if (gameManager.useMobileInputs)
        {
            if (CrossPlatformInputManager.GetButtonDown("N"))
                NextPosition();
            if (CrossPlatformInputManager.GetButtonDown("P"))
                PreviousPosition();
            if (CrossPlatformInputManager.GetButtonDown("B"))
            {
                BuyItem();
                if (gameManager.Mission1 && !gameManager.Mission2 && !gameManager.Mission3 && !gameManager.Mission4 && !gameManager.Mission5)
                    gameManager.Mission2 = true;
            }
        }
        if (!colliderAction.playerTriggered || priceText == null)
            return;
        if (currentPositionIndex == 0)
            priceText.text = "Price: 50 money";
        else if (currentPositionIndex == 1)
            priceText.text = "Price: 70 money";
        else if (currentPositionIndex == 2)
            priceText.text = "Price: 35 money";
    }

    void MoveCamera()
    {
        if (cameraPositions == null || currentPositionIndex < 0 || currentPositionIndex >= cameraPositions.Length || cameraPositions[currentPositionIndex] == null)
            return;
        transform.position = cameraPositions[currentPositionIndex].position;
        transform.rotation = cameraPositions[currentPositionIndex].rotation;
    }

    public void NextPosition()
    {
        if (cameraPositions == null || cameraPositions.Length == 0)
            return;
        currentPositionIndex = (currentPositionIndex + 1) % cameraPositions.Length;
        MoveCamera();
    }

    public void PreviousPosition()
    {
        if (cameraPositions == null || cameraPositions.Length == 0)
            return;
        currentPositionIndex = (currentPositionIndex - 1 + cameraPositions.Length) % cameraPositions.Length;
        MoveCamera();
    }

    public void BuyItem()
    {
        if (gameManager == null || colliderAction == null || !colliderAction.playerTriggered)
            return;
        if (currentPositionIndex == 0 && !gameManager.akmPrefab)
        {
            int itemPrice = 50;
            if (gameManager.playerMoney >= itemPrice)
            {
                gameManager.playerMoney -= itemPrice;
                Debug.Log("Item bought for " + itemPrice + " money");
                gameManager.akmPrefab = true;
            }
        }
        else if (currentPositionIndex == 1 && !gameManager.m416Prefab)
        {
            int itemPrice = 70;
            if (gameManager.playerMoney >= itemPrice)
            {
                gameManager.playerMoney -= itemPrice;
                Debug.Log("Item bought for " + itemPrice + " money");
                gameManager.m416Prefab = true;
            }
        }
        else if (currentPositionIndex == 2 && !gameManager.pistolPrefab)
        {
            int itemPrice = 35;
            if (gameManager.playerMoney >= itemPrice)
            {
                gameManager.playerMoney -= itemPrice;
                Debug.Log("Item bought for " + itemPrice + " money");
                gameManager.pistolPrefab = true;
            }
        }
    }
}