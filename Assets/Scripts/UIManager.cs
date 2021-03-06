using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Weapon Icon References")]
    public Image weaponA_L_image;
    public Image weaponB_L_image;
    public Image weaponC_L_image;
    public Image weaponD_L_image;

    public Image weaponA_R_image;
    public Image weaponB_R_image;
    public Image weaponC_R_image;
    public Image weaponD_R_image;

    [Header("Weapon Ammo References")]
    public TMP_Text weaponA_L_ammo;
    public TMP_Text weaponB_L_ammo;
    public TMP_Text weaponC_L_ammo;
    public TMP_Text weaponD_L_ammo;

    public TMP_Text weaponA_R_ammo;
    public TMP_Text weaponB_R_ammo;
    public TMP_Text weaponC_R_ammo;
    public TMP_Text weaponD_R_ammo;

    [Header("Weapon Selection References")]
    public Image weaponA_L_select;
    public Image weaponB_L_select;
    public Image weaponC_L_select;
    public Image weaponD_L_select;

    public Image weaponA_R_select;
    public Image weaponB_R_select;
    public Image weaponC_R_select;
    public Image weaponD_R_select;

    [Header("Health Bar References")]
    public GameObject healthBar_bar;

    private Image previous_L_select;
    private Image previous_R_select;



    // Start is called before the first frame update
    void Start()
    {
        Image[] weapons_L_images = new Image[] { weaponA_L_image, weaponB_L_image, weaponC_L_image, weaponD_L_image };
        Image[] weapons_R_images = new Image[] { weaponA_R_image, weaponB_R_image, weaponC_R_image, weaponD_R_image };

        for (int i = 0; i < GameManager.Instance().mainPlayer.backArmWeapons.Length; i++)
        {
            weapons_L_images[i].sprite = GameManager.Instance().mainPlayer.backArmWeapons[i].icon;
        }

        for (int i = 0; i < GameManager.Instance().mainPlayer.frontArmWeapons.Length; i++)
        {
            weapons_R_images[i].sprite = GameManager.Instance().mainPlayer.frontArmWeapons[i].icon;
        }

    }


    public void UpdateSelectedUI(ArmType armType, char index)
    {
        // If back arm, update
        if (armType == ArmType.Back)
        {
            if (previous_L_select) previous_L_select.enabled = false;
            switch (index)
            {
                case 'A':
                    weaponA_L_select.enabled = true;
                    previous_L_select = weaponA_L_select;
                    break;

                case 'B':
                    weaponB_L_select.enabled = true;
                    previous_L_select = weaponB_L_select;
                    break;

                case 'C':
                    weaponC_L_select.enabled = true;
                    previous_L_select = weaponC_L_select;
                    break;

                case 'D':
                    weaponD_L_select.enabled = true;
                    previous_L_select = weaponD_L_select;
                    break;
            }
        }

        // If front arm, update
        else if (armType == ArmType.Front)
        {
            if (previous_R_select) previous_R_select.enabled = false;
            switch (index)
            {
                case 'A':
                    weaponA_R_select.enabled = true;
                    previous_R_select = weaponA_R_select;
                    break;

                case 'B':
                    weaponB_R_select.enabled = true;
                    previous_R_select = weaponB_R_select;
                    break;

                case 'C':
                    weaponC_R_select.enabled = true;
                    previous_R_select = weaponC_R_select;
                    break;

                case 'D':
                    weaponD_R_select.enabled = true;
                    previous_R_select = weaponD_R_select;
                    break;
            }
        }
    }


    public void UpdateAmmoUI(ArmType armType, char index, int currAmmo, int maxAmmo)
    {
        // If back arm, update
        if (armType == ArmType.Back)
        {
            switch (index)
            {
                case 'A':
                    weaponA_L_ammo.text = currAmmo + "/" + maxAmmo;
                    break;

                case 'B':
                    weaponB_L_ammo.text = currAmmo + "/" + maxAmmo;
                    break;

                case 'C':
                    weaponC_L_ammo.text = currAmmo + "/" + maxAmmo;
                    break;

                case 'D':
                    weaponD_L_ammo.text = currAmmo + "/" + maxAmmo;
                    break;
            }
        }

        // If front arm, update
        else if (armType == ArmType.Front)
        {
            switch (index)
            {
                case 'A':
                    weaponA_R_ammo.text = currAmmo + "/" + maxAmmo;
                    break;

                case 'B':
                    weaponB_R_ammo.text = currAmmo + "/" + maxAmmo;
                    break;

                case 'C':
                    weaponC_R_ammo.text = currAmmo + "/" + maxAmmo;
                    break;

                case 'D':
                    weaponD_R_ammo.text = currAmmo + "/" + maxAmmo;
                    break;
            }
        }
    }


    // Sets Health Bar Scale to Current Health
    public void UpdateHealthBar(float myHealth)
    {
        healthBar_bar.transform.localScale = new Vector3(myHealth, healthBar_bar.transform.localScale.y,
            healthBar_bar.transform.localScale.z);
    }
}
