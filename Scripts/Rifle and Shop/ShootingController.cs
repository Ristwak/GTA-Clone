using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ShootingController : MonoBehaviour
{
    InputManager inputManager;
    GameManager gameManager;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float reloadTime = 2f;
    public int magazineCapacity = 30;
    public int maxAmmo = 300;
    public float fireRange = 100f;
    public float giveDamageOf = 5f;
    private float nextFireTime;
    private int currentMagazine;
    private int currentAmmo;
    private bool isReloading;

    [Header("Rifle Effects and Sound")]
    public ParticleSystem muzzleFlash;
    public GameObject bloodEffect;
    public GameObject justEffect;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        currentMagazine = magazineCapacity;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (gameManager == null)
            return;
        if (gameManager.useMobileInputs)
        {
            if (CrossPlatformInputManager.GetButton("Fire") && Time.time >= nextFireTime && currentMagazine > 0 && !isReloading)
            {
                Shoot();
                nextFireTime = Time.time + 1f / Mathf.Max(fireRate, 0.01f);
            }
            if (CrossPlatformInputManager.GetButton("Reload") && currentMagazine < magazineCapacity && currentAmmo > 0 && !isReloading)
                StartCoroutine(Reload());
        }
        else if (inputManager != null)
        {
            if (inputManager.shootInput && Time.time >= nextFireTime && currentMagazine > 0 && !isReloading)
            {
                Shoot();
                nextFireTime = Time.time + 1f / Mathf.Max(fireRate, 0.01f);
            }
            if (inputManager.reloadInput && currentMagazine < magazineCapacity && currentAmmo > 0 && !isReloading)
                StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (firePoint == null)
            return;
        if (muzzleFlash != null)
            muzzleFlash.Play();
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, fireRange))
        {
            Debug.Log("Hit: " + hit.transform.name);
            bool hitCharacter = false;
            CharacterNavigatorScript characterNavigatorScript = hit.transform.GetComponentInParent<CharacterNavigatorScript>();
            PoliceOfficer policeOfficer = hit.transform.GetComponentInParent<PoliceOfficer>();
            PoliceOfficer2 policeOfficer2 = hit.transform.GetComponentInParent<PoliceOfficer2>();
            Character character = hit.transform.GetComponentInParent<Character>();
            Guard guard = hit.transform.GetComponentInParent<Guard>();
            Diaz diaz = hit.transform.GetComponentInParent<Diaz>();
            Lance lance = hit.transform.GetComponentInParent<Lance>();

            if (characterNavigatorScript != null)
            {
                characterNavigatorScript.CharacterHitDamage(giveDamageOf);
                hitCharacter = true;
            }
            else if (policeOfficer != null)
            {
                policeOfficer.CharacterHitDamage(giveDamageOf);
                hitCharacter = true;
            }
            else if (policeOfficer2 != null)
            {
                policeOfficer2.CharacterHitDamage(giveDamageOf);
                hitCharacter = true;
            }
            else if (character != null)
            {
                character.CharacterHitDamage(giveDamageOf);
                hitCharacter = true;
            }
            else if (guard != null)
            {
                guard.CharacterHitDamage(giveDamageOf);
                hitCharacter = true;
            }
            else if (diaz != null)
            {
                diaz.CharacterHitDamage(giveDamageOf);
                hitCharacter = true;
            }
            else if (lance != null)
            {
                lance.CharacterHitDamage(giveDamageOf);
                hitCharacter = true;
            }

            if (hitCharacter)
                SpawnEffect(bloodEffect, hit.point, hit.normal);
            else
                SpawnEffect(justEffect, hit.point, hit.normal);
        }
        currentMagazine--;
    }

    void SpawnEffect(GameObject effectPrefab, Vector3 position, Vector3 normal)
    {
        if (effectPrefab == null)
            return;
        GameObject effect = Instantiate(effectPrefab, position, Quaternion.LookRotation(normal));
        Destroy(effect, 1f);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        int ammoToReload = Mathf.Min(magazineCapacity - currentMagazine, currentAmmo);
        yield return new WaitForSeconds(reloadTime);
        currentMagazine += ammoToReload;
        currentAmmo -= ammoToReload;
        if (currentAmmo < maxAmmo - magazineCapacity)
            maxAmmo = currentAmmo + magazineCapacity;
        isReloading = false;
    }
}