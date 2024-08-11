using System.Collections;
using System.Collections.Generic;
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
        if (gameManager.useMobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButton("Fire") && Time.time >= nextFireTime && currentMagazine > 0 && !isReloading)
            {
                Shoot();
                nextFireTime = Time.time + 1 / fireRate;
            }

            if (CrossPlatformInputManager.GetButton("Reload") && currentMagazine < magazineCapacity && currentAmmo > 0 && !isReloading)
            {
                StartCoroutine(Reload());
            }
        }
        else
        {
            if (inputManager.shootInput && Time.time >= nextFireTime && currentMagazine > 0 && !isReloading)
            {
                Shoot();
                nextFireTime = Time.time + 1 / fireRate;
            }

            if (inputManager.reloadInput && currentMagazine < magazineCapacity && currentAmmo > 0 && !isReloading)
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange))
        {
            // perform action on the object hit
            Debug.Log("Hit: " + hit.transform.name);

            CharacterNavigatorScript characterNavigatorScript = hit.transform.GetComponent<CharacterNavigatorScript>();

            if (characterNavigatorScript != null)
            {
                characterNavigatorScript.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 1f);
            }

            PoliceOfficer policeOfficer = hit.transform.GetComponent<PoliceOfficer>();

            if (policeOfficer != null)
            {
                policeOfficer.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 1f);
            }

            PoliceOfficer2 policeOfficer2 = hit.transform.GetComponent<PoliceOfficer2>();

            if (policeOfficer2 != null)
            {
                policeOfficer2.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 1f);
            }

            Character character = hit.transform.GetComponent<Character>();

            if (character != null)
            {
                character.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 1f);
            }

            Guard guard = hit.transform.GetComponent<Guard>();

            if (guard != null)
            {
                guard.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 1f);
            }

            Diaz diaz = hit.transform.GetComponent<Diaz>();

            if (diaz != null)
            {
                diaz.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 1f);
            }

            Lance lance = hit.transform.GetComponent<Lance>();

            if (lance != null)
            {
                lance.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 1f);
            }

            if (characterNavigatorScript == null && policeOfficer == null && policeOfficer2 == null && character == null && guard == null && diaz == null && lance == null)
            {
                GameObject justEffectGo = Instantiate(justEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(justEffectGo, 1f);
            }
        }
        currentMagazine--;
    }

    IEnumerator Reload()
    {
        isReloading = true;

        int ammoToReload = Mathf.Min(magazineCapacity - currentMagazine, currentAmmo);
        yield return new WaitForSeconds(reloadTime);
        currentMagazine += ammoToReload;
        currentAmmo -= ammoToReload;

        if (currentAmmo < maxAmmo - magazineCapacity)
        {
            maxAmmo = currentAmmo + magazineCapacity;
        }

        isReloading = false;
    }
}
