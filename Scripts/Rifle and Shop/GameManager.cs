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
    int currentWeapon;

    [Header("Player Money and Kills")]
    public int playerMoney = 150;
    public int currentKills = 0;

    [Header("UI Animations")]
    AnimatorManager animatorManager;
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
        if (MainMenu.instance != null)
            useMobileInputs = MainMenu.instance.useMobileInput;
    }

    void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        inputManager = FindObjectOfType<InputManager>();
        colliderAction = FindObjectOfType<ColliderAction>();
        carInteraction = FindObjectOfType<CarInteraction>();
        UpdateMoneyUI();
        Debug.Log("Mobile input state GameManager " + useMobileInputs);
    }

    void Update()
    {
        UpdateMobileControls();
        UpdateMoneyUI();
        HandleWeaponInput();
        UpdateWeaponUI();
        UpdateMissionAreas();
    }

    void UpdateMobileControls()
    {
        if (!useMobileInputs)
            return;
        bool inCar = carInteraction != null && carInteraction.playerInCar;
        bool inShop = colliderAction != null && colliderAction.inShop;
        bool weaponActive = rifle1Active || rifle2Active || rifle3Active;
        if (carControls != null)
            carControls.SetActive(inCar);
        if (shopControls != null)
            shopControls.SetActive(!inCar && inShop);
        if (shootingControls != null)
            shootingControls.SetActive(!inCar && !inShop && weaponActive);
        if (playerControls != null)
            playerControls.SetActive(!inCar && !inShop);
    }

    void UpdateMoneyUI()
    {
        if (playerMoneyText != null)
            playerMoneyText.text = "$" + playerMoney;
    }

    void HandleWeaponInput()
    {
        bool changePressed = false;
        if (useMobileInputs)
            changePressed = CrossPlatformInputManager.GetButtonDown("RifleChange");
        else if (inputManager != null && inputManager.changeInput)
        {
            changePressed = true;
            inputManager.changeInput = false;
        }
        if (changePressed)
            SwitchWeapon();
    }

    void SwitchWeapon()
    {
        int nextWeapon = currentWeapon;
        for (int i = 0; i < 4; i++)
        {
            nextWeapon = (nextWeapon + 1) % 4;
            if (nextWeapon == 0 || IsWeaponAvailable(nextWeapon))
                break;
        }
        SetWeapon(nextWeapon);
    }

    bool IsWeaponAvailable(int weapon)
    {
        if (weapon == 1)
            return pistolPrefab && pistolGameobjectPrefab != null;
        if (weapon == 2)
            return akmPrefab && akmGameobjectPrefab != null;
        if (weapon == 3)
            return m416Prefab && m416GameobjectPrefab != null;
        return weapon == 0;
    }

    void SetWeapon(int weapon)
    {
        SetFists();
        currentWeapon = weapon;
        if (weapon == 1 && IsWeaponAvailable(1))
        {
            rifle1Active = true;
            pistolGameobjectPrefab.SetActive(true);
            if (animatorManager != null && animatorManager.animator != null)
                animatorManager.animator.SetBool("pistolActive", true);
        }
        else if (weapon == 2 && IsWeaponAvailable(2))
        {
            rifle2Active = true;
            akmGameobjectPrefab.SetActive(true);
            if (animatorManager != null && animatorManager.animator != null)
                animatorManager.animator.SetBool("rifleActive", true);
        }
        else if (weapon == 3 && IsWeaponAvailable(3))
        {
            rifle3Active = true;
            m416GameobjectPrefab.SetActive(true);
            if (animatorManager != null && animatorManager.animator != null)
                animatorManager.animator.SetBool("rifleActive", true);
        }
        else
        {
            currentWeapon = 0;
        }
        UpdateWeaponUI();
    }

    void SetFists()
    {
        rifle1Active = false;
        rifle2Active = false;
        rifle3Active = false;
        if (animatorManager != null && animatorManager.animator != null)
        {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", false);
        }
        if (pistolGameobjectPrefab != null)
            pistolGameobjectPrefab.SetActive(false);
        if (akmGameobjectPrefab != null)
            akmGameobjectPrefab.SetActive(false);
        if (m416GameobjectPrefab != null)
            m416GameobjectPrefab.SetActive(false);
    }

    void UpdateWeaponUI()
    {
        if (pistolImage != null)
            pistolImage.SetActive(rifle1Active);
        if (akmImage != null)
            akmImage.SetActive(rifle2Active);
        if (m416Image != null)
            m416Image.SetActive(rifle3Active);
        if (fistImage != null)
            fistImage.SetActive(!rifle1Active && !rifle2Active && !rifle3Active);
    }

    void UpdateMissionAreas()
    {
        if ((Mission1 || Mission2) && mission1Area != null)
            mission1Area.SetActive(false);
        if (Mission1 && Mission2 && mission3Area != null)
            mission3Area.SetActive(true);
        if (Mission1 && Mission2 && Mission3)
        {
            if (mission3CarsArea != null)
                mission3CarsArea.SetActive(true);
            if (mission4and5Area != null)
                mission4and5Area.SetActive(true);
        }
        if (Mission1 && Mission2 && Mission3 && Mission4 && Mission5 && mission4and5Area != null)
            mission4and5Area.SetActive(false);
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
            return;
        if (data.position == null || data.position.Length < 3)
        {
            Debug.LogError("GameManager: Saved player position is invalid.");
            return;
        }
        playerMoney = data.playerMoney;
        Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
        if (playerGameObject != null)
            playerGameObject.position = position;
        else
            Debug.LogWarning("GameManager: Player GameObject reference is missing.");
        pistolPrefab = data.pistolPrefab;
        akmPrefab = data.akmPrefab;
        m416Prefab = data.m416Prefab;
        Mission1 = data.Mission1;
        Mission2 = data.Mission2;
        Mission3 = data.Mission3;
        Mission4 = data.Mission4;
        Mission5 = data.Mission5;
        SetWeapon(0);
        UpdateMissionAreas();
        UpdateMoneyUI();
    }
}