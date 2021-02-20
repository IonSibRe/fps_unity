using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;

    private GameObject weaponHolder;
    private WeaponSwitching weaponSwitch;
    private Gun gun;

    private int score;

    void Start()
    {
        weaponHolder = GameObject.Find("Weapon Holder");
        weaponSwitch = weaponHolder.GetComponent<WeaponSwitching>();

        GetCurrentGun();
    }

    void Update()
    {
        // Ammo
        if (weaponSwitch.previousSelectedWeapon != weaponSwitch.selectedWeapon)
            GetCurrentGun();

        // Update HUD
        ammoText.text = $"{gun.currentAmmo}/{gun.maxAmmo}";
        scoreText.text = $"Score: {score}";
    }

    private void GetCurrentGun()
    {
        foreach (Transform child in weaponHolder.gameObject.transform)
        {
            if (child.gameObject.activeSelf)
            {
                gun = child.gameObject.GetComponent<Gun>();
            }
        }
    }

    public int UpdateScore()
    {
        return score += 10; 
    }
}
