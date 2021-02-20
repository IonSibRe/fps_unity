using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Guns Stats
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private float impactForce = 30.0f;
    [SerializeField] private float fireRate = 15.0f;
    [SerializeField] private float reloadTime = 1f;

    private float nextTimeToFire = 0f;

    // Ammo
    public int maxAmmo = 10;
    public int currentAmmo;

    // Conditionals
    public bool isAutomatic;
    private bool isReloading;

    // Animation Reset
    private Vector3 startPos;

    public Camera fpsCam;
    public Animator animator;
    public Animator localAnimator;

    public ParticleSystem muzzleFlash;
    public GameObject muzzleLight;

    public AudioClip gunShotSound;
    public AudioSource shootSound;
    public GameObject impactEffect;

    void Start()
    {
        currentAmmo = maxAmmo;
        startPos = transform.localPosition;
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    private void OnDisable()
    {
        transform.localPosition = startPos;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo)) 
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && isAutomatic)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && !isAutomatic)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - 0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    private IEnumerator MuzzleLightSwitch()
    {
        muzzleLight.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        muzzleLight.SetActive(false);
    }

    private void Shoot()
    {
        // Muzzle Flash
        muzzleFlash.Play();
        StartCoroutine(MuzzleLightSwitch());

        // Gun Sound
        shootSound.PlayOneShot(gunShotSound);

        // Recoil
        localAnimator.Play("Fire");

        currentAmmo--;
       
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            // Damage && Score
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Force
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            // Impact Hit
            GameObject impactObject = Instantiate(impactEffect, hit.point - fpsCam.transform.forward * 0.1f, Quaternion.LookRotation(hit.normal));
            Destroy(impactObject, 0.1f);
        }
    }
}
