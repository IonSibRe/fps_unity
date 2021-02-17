using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /* TODO: 
            - Fix Gun Skew          - Done
            - Add MuzzleFlash       - Done
            - Add Impact Effect
    */

    public float damage = 10.0f;
    public float range = 100.0f;
    public float impactForce = 30.0f;
    public float fireRate = 15.0f;
    public float reloadTime = 1f;

    private float nextTimeToFire = 0f;

    public int maxAmmo = 10;
    private int currentAmmo;

    public bool isAutomatic;
    private bool isReloading;

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
    }

    void OnEnable()
    {
        isReloading = false;
        localAnimator.SetTrigger("WeaponIdle");
        animator.SetBool("Reloading", false);
    }

    void Update()
    {

        if (isReloading)
            return;

        if (currentAmmo <= 0)
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

    //private IEnumerator Recoil()
    //{
    //    localAnimator.SetBool("Fire", true);
    //    yield return new WaitForSeconds(0.2f);
    //    localAnimator.SetBool("Fire", false);
    //}

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
       
        RaycastHit hit;
    
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactObject = Instantiate(impactEffect, hit.point - fpsCam.transform.forward * 0.1f, Quaternion.LookRotation(hit.normal));
            Destroy(impactObject, 0.1f);
        }
    }
}
