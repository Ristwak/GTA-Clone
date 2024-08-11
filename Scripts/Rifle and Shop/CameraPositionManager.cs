using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        transform.position = cameraPositions[currentPositionIndex].position;
        transform.rotation = cameraPositions[currentPositionIndex].rotation;
    }

    void Update()
    {
        if (gameManager.useMobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButton("N"))
            {
                NextPosition();
            }

            if (CrossPlatformInputManager.GetButton("P"))
            {
                PreviousPosition();
            }

            if (CrossPlatformInputManager.GetButton("B"))
            {
                BuyItem();
                if (gameManager.Mission1 == true && gameManager.Mission2 == false && gameManager.Mission3 == false && gameManager.Mission4 == false && gameManager.Mission5 == false)
                {
                    gameManager.Mission2 = true;
                }
            }
        }
        
        if (currentPositionIndex == 0 && colliderAction.playerTriggered == true)
        {
            int itemPrice = 50;
            priceText.text = "Price: " + itemPrice + " money";
        }

        if (currentPositionIndex == 1 && colliderAction.playerTriggered == true)
        {
            int itemPrice = 70;
            priceText.text = "Price: " + itemPrice + " money";
        }

        if (currentPositionIndex == 2 && colliderAction.playerTriggered == true)
        {
            int itemPrice = 35;
            priceText.text = "Price: " + itemPrice + " money";
        }
    }

    void MoveCamera()
    {
        transform.position = cameraPositions[currentPositionIndex].position;
        transform.rotation = cameraPositions[currentPositionIndex].rotation;
    }

    public void NextPosition()
    {
        currentPositionIndex = (currentPositionIndex + 1) % cameraPositions.Length;
        MoveCamera();
    }

    public void PreviousPosition()
    {
        currentPositionIndex = (currentPositionIndex - 1 + cameraPositions.Length) % cameraPositions.Length;
        MoveCamera();
    }

    public void BuyItem()
    {
        if (currentPositionIndex == 0 && colliderAction.playerTriggered == true && gameManager.akmPrefab == false)
        {
            int itemPrice = 50;
            if (gameManager.playerMoney >= itemPrice)
            {
                gameManager.playerMoney -= itemPrice;
                Debug.Log("Item bought for " + itemPrice + " money");
                gameManager.akmPrefab = true;
            }
        }
        else if (currentPositionIndex == 1 && colliderAction.playerTriggered == true && gameManager.m416Prefab == false)
        {
            int itemPrice = 70;
            if (gameManager.playerMoney >= itemPrice)
            {
                gameManager.playerMoney -= itemPrice;
                Debug.Log("Item bought for " + itemPrice + " money");
                gameManager.m416Prefab = true;
            }
        }
        else if (currentPositionIndex == 2 && colliderAction.playerTriggered == true && gameManager.pistolPrefab == false)
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
