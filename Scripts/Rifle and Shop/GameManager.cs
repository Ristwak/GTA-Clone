using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    [Header("Player Rifle Things")]
    public GameObject pistolGameobjectPrefab;
    public GameObject akmGameobjectPrefab;
    public GameObject m416GameobjectPrefab;
    public Transform playerGameObject;
    public Transform cameraGameObject;

    public bool pistolPrefab;
    public bool akmPrefab;
    public bool m416Prefab;

    bool rifle1Active;
    bool rifle2Active;
    bool rifle3Active;

    [Header("Player Money and Kills")]
    public int playerMoney = 150;
    public int currentKills = 0;

    [Header("UI Animations")]
    AnimatorManager animatorManager;
    GameManager gameManager;
    InputManager inputManager;
    public Text playerMoneyText;
    public GameObject pistolImage;
    public GameObject akmImage;
    public GameObject m416Image;
    public GameObject fistImage;

    [Header("Missions")]
    public bool Mission1;
    public GameObject mission1Area;
    public bool Mission2;
    public bool Mission3;
    public GameObject mission3Area;
    public GameObject mission3CarsArea;
    public bool Mission4;
    public bool Mission5;
    public GameObject mission4and5Area;

    public bool useMobileInputs = false;

    [Header("UI")]
    public GameObject carControls;
    public GameObject shootingControls;
    public GameObject shopControls;
    public GameObject playerControls;

    ColliderAction colliderAction;
    CarInteraction carInteraction;

    void Awake()
    {
        if (MainMenu.instance.useMobileInput == true)
        {
            useMobileInputs = true;
        }
        else
        {
            useMobileInputs = false;
        }
    }

    void Start()
    {
        // if(MainMenu.instance.continueGame == true)
        // {
        //     LoadPlayer();
        //     cameraGameObject.position = playerGameObject.position;
        // }

        animatorManager = FindObjectOfType<AnimatorManager>();
        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();
        colliderAction = FindObjectOfType<ColliderAction>();
        carInteraction = FindObjectOfType<CarInteraction>();
        playerMoneyText.text = "$" + playerMoney;
        Debug.Log("Mobile input state GameManager " + useMobileInputs);

    }

    void Update()
    {
        // if (useMobileInputs == false)
        // {
        //     playerControls.SetActive(false);
        //     shopControls.SetActive(false);
        //     shootingControls.SetActive(false);
        //     carControls.SetActive(false);
        // }

        if (carInteraction.playerInCar == true && colliderAction.inShop == false && rifle1Active == false && rifle2Active == false && rifle3Active == false && useMobileInputs)
        {
            playerControls.SetActive(false);
            shopControls.SetActive(false);
            shootingControls.SetActive(false);
            carControls.SetActive(true);
        }
        else if (carInteraction.playerInCar == false && colliderAction.inShop == true && rifle1Active == false && rifle2Active == false && rifle3Active == false && useMobileInputs)
        {
            playerControls.SetActive(false);
            shopControls.SetActive(true);
            shootingControls.SetActive(false);
            carControls.SetActive(false);
        }
        else if (carInteraction.playerInCar == false && colliderAction.inShop == false && rifle1Active == true || rifle2Active == true || rifle3Active == true && useMobileInputs)
        {
            playerControls.SetActive(true);
            shopControls.SetActive(false);
            shootingControls.SetActive(true);
            carControls.SetActive(false);
        }
        else
        {
            playerControls.SetActive(true);
            shopControls.SetActive(false);
            shootingControls.SetActive(false);
            carControls.SetActive(false);
        }

        playerMoneyText.text = "$" + playerMoney;

        if (useMobileInputs)
        {
            if (CrossPlatformInputManager.GetButton("RifleChange"))
            {
                if (pistolPrefab == true && !rifle1Active)
                {
                    SetPistol();
                    rifle1Active = true;
                }
                else if (akmPrefab == true && !rifle2Active)
                {
                    SetAKM();
                    rifle2Active = true;
                }
                else if (m416Prefab == true && !rifle3Active)
                {
                    SetM416();
                    rifle3Active = true;
                }
                else if (pistolPrefab == true && rifle1Active || akmPrefab == true && rifle2Active || m416Prefab == true && rifle3Active)
                {
                    rifle1Active = false;
                    rifle2Active = false;
                    rifle3Active = false;

                    animatorManager.animator.SetBool("rifleActive", false);
                    animatorManager.animator.SetBool("pistolActive", false);

                    pistolGameobjectPrefab.SetActive(false);
                    akmGameobjectPrefab.SetActive(false);
                    m416GameobjectPrefab.SetActive(false);
                }
            }
        }
        else
        {
            if (inputManager.changeInput)
            {
                if (pistolPrefab == true && !rifle1Active)
                {
                    SetPistol();
                    rifle1Active = true;
                }
                else if (akmPrefab == true && !rifle2Active)
                {
                    SetAKM();
                    rifle2Active = true;
                }
                else if (m416Prefab == true && !rifle3Active)
                {
                    SetM416();
                    rifle3Active = true;
                }
                else if (pistolPrefab == true && rifle1Active || akmPrefab == true && rifle2Active || m416Prefab == true && rifle3Active)
                {
                    rifle1Active = false;
                    rifle2Active = false;
                    rifle3Active = false;

                    animatorManager.animator.SetBool("rifleActive", false);
                    animatorManager.animator.SetBool("pistolActive", false);

                    pistolGameobjectPrefab.SetActive(false);
                    akmGameobjectPrefab.SetActive(false);
                    m416GameobjectPrefab.SetActive(false);
                }
            }
        }

        // UI
        if (rifle1Active == true && rifle2Active == false && rifle3Active == false)
        {
            pistolImage.SetActive(true);
            akmImage.SetActive(false);
            m416Image.SetActive(false);
            fistImage.SetActive(false);
        }
        else if (rifle1Active == true && rifle2Active == true && rifle3Active == false)
        {
            pistolImage.SetActive(false);
            akmImage.SetActive(true);
            m416Image.SetActive(false);
            fistImage.SetActive(false);
        }
        else if (rifle1Active == true & rifle2Active == true & rifle3Active == true)
        {
            pistolImage.SetActive(false);
            akmImage.SetActive(false);
            m416Image.SetActive(true);
            fistImage.SetActive(false);
        }
        else if (rifle1Active == false || rifle2Active == false || rifle3Active == false)
        {
            pistolImage.SetActive(false);
            akmImage.SetActive(false);
            m416Image.SetActive(false);
            fistImage.SetActive(true);
        }

        // Missions

        if (Mission1 == true || Mission2 == true)
        {
            mission1Area.SetActive(false);
        }

        if (Mission1 == true && Mission2 == true)
        {
            mission3Area.SetActive(true);
        }

        if (Mission1 == true && Mission2 == true && Mission3 == true)
        {
            mission3CarsArea.SetActive(true);
            mission4and5Area.SetActive(true);
        }

        if (Mission1 == true && Mission2 == true && Mission3 == true && Mission4 == true && Mission5 == true)
        {
            mission4and5Area.SetActive(false);
        }
    }

    void SetPistol()
    {
        if (!rifle1Active)
        {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", true);

            pistolGameobjectPrefab.SetActive(true);
            akmGameobjectPrefab.SetActive(false);
            m416GameobjectPrefab.SetActive(false);
            rifle1Active = true;
        }
        else
        {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", false);

            pistolGameobjectPrefab.SetActive(false);
            rifle1Active = false;
        }
    }

    void SetAKM()
    {
        if (!rifle2Active)
        {
            animatorManager.animator.SetBool("rifleActive", true);
            animatorManager.animator.SetBool("pistolActive", false);

            pistolGameobjectPrefab.SetActive(false);
            akmGameobjectPrefab.SetActive(true);
            m416GameobjectPrefab.SetActive(false);
            rifle2Active = true;
        }
        else
        {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", false);

            akmGameobjectPrefab.SetActive(false);
            rifle2Active = false;
        }
    }

    void SetM416()
    {
        if (!rifle3Active)
        {
            animatorManager.animator.SetBool("rifleActive", true);
            animatorManager.animator.SetBool("pistolActive", false);

            pistolGameobjectPrefab.SetActive(false);
            akmGameobjectPrefab.SetActive(false);
            m416GameobjectPrefab.SetActive(true);
            rifle3Active = true;
        }
        else
        {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", false);

            m416GameobjectPrefab.SetActive(false);
            rifle3Active = false;
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        playerMoney = data.playerMoney;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        playerGameObject.position = position;

        pistolPrefab = data.pistolPrefab;
        akmPrefab = data.akmPrefab;
        m416Prefab = data.m416Prefab;

        Mission1 = data.Mission1;
        Mission2 = data.Mission2;
        Mission3 = data.Mission3;
        Mission4 = data.Mission4;
        Mission5 = data.Mission5;
    }
}
