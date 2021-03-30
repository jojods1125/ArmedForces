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

    [Header("Match Info References")]
    public GameObject matchClock;

    [Header("DEBUG: Player Stat References")]
    public GameObject p1_KDR;
    public GameObject p2_KDR;
    public GameObject p3_KDR;
    public GameObject p4_KDR;

    private Image previous_L_select;
    private Image previous_R_select;

    [Header("Achievement PopUp References")]
    [Tooltip("Display Prefab")]
    public GameObject achievementPrefab;
    [Tooltip("Tier Prefab")]
    public GameObject tierPrefab;
    [Tooltip("Progress Bar Prefab")]
    public GameObject progressBarPrefab;
    [Tooltip("Panel containing Achievement")]
    public GameObject panel;
    [Tooltip("Animator Controller")]
    public RuntimeAnimatorController rac;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Update the UI to show which weapons are in the loadout
    /// </summary>
    public void UpdateWeaponIcons()
    {
        // Gather all the image references
        Image[] weapons_L_images = new Image[] { weaponA_L_image, weaponB_L_image, weaponC_L_image, weaponD_L_image };
        Image[] weapons_R_images = new Image[] { weaponA_R_image, weaponB_R_image, weaponC_R_image, weaponD_R_image };

        // Update back arm images
        if (GameManager.Instance().localPlayer)
        {
            for (int i = 0; i < GameManager.Instance().localPlayer.backArmWeapons.Length; i++)
            {
                weapons_L_images[i].sprite = GameManager.Instance().localPlayer.backArmWeapons[i].icon;
            }

            // Update front arm images
            for (int i = 0; i < GameManager.Instance().localPlayer.frontArmWeapons.Length; i++)
            {
                weapons_R_images[i].sprite = GameManager.Instance().localPlayer.frontArmWeapons[i].icon;
            }
        }
    }


    /// <summary>
    /// Update the UI to show which arm is currently equipped
    /// </summary>
    /// <param name="armType"> Front or back arm </param>
    /// <param name="index"> Weapon index being updated </param>
    public void UpdateSelectedUI(ArmType armType, char index)
    {
        // If back arm, update
        if (armType == ArmType.Back)
        {
            // Deselect previous arm
            if (previous_L_select) previous_L_select.enabled = false;

            // Select new arm
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
            // Deselect previous arm
            if (previous_R_select) previous_R_select.enabled = false;

            // Select new arm
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


    /// <summary>
    /// Update the UI to show how much ammo a single weapon has
    /// </summary>
    /// <param name="armType"> Front or back arm </param>
    /// <param name="index"> Weapon index being updated </param>
    /// <param name="currAmmo"> Current ammo the weapon has </param>
    /// <param name="maxAmmo"> Max ammo the weapon can have </param>
    public void UpdateAmmoUI(ArmType armType, char index, int currAmmo, int maxAmmo)
    {
        // If back arm, update
        if (armType == ArmType.Back)
        {
            switch (index)
            {
                case 'A':
                    weaponA_L_ammo.text = currAmmo.ToString();
                    if (currAmmo == maxAmmo) weaponA_L_ammo.color = Color.green; else if (currAmmo == 0) weaponA_L_ammo.color = Color.red; else weaponA_L_ammo.color = Color.white;
                    break;

                case 'B':
                    weaponB_L_ammo.text = currAmmo.ToString();
                    if (currAmmo == maxAmmo) weaponB_L_ammo.color = Color.green; else if (currAmmo == 0) weaponB_L_ammo.color = Color.red; else weaponB_L_ammo.color = Color.white;
                    break;

                case 'C':
                    weaponC_L_ammo.text = currAmmo.ToString();
                    if (currAmmo == maxAmmo) weaponC_L_ammo.color = Color.green; else if (currAmmo == 0) weaponC_L_ammo.color = Color.red; else weaponC_L_ammo.color = Color.white;
                    break;

                case 'D':
                    weaponD_L_ammo.text = currAmmo.ToString();
                    if (currAmmo == maxAmmo) weaponD_L_ammo.color = Color.green; else if (currAmmo == 0) weaponD_L_ammo.color = Color.red; else weaponD_L_ammo.color = Color.white;
                    break;
            }
        }

        // If front arm, update
        else if (armType == ArmType.Front)
        {
            switch (index)
            {
                case 'A':
                    weaponA_R_ammo.text = currAmmo.ToString();
                    if (currAmmo == maxAmmo) weaponA_R_ammo.color = Color.green; else if (currAmmo == 0) weaponA_R_ammo.color = Color.red; else weaponA_R_ammo.color = Color.white;
                    break;

                case 'B':
                    weaponB_R_ammo.text = currAmmo.ToString();
                    if (currAmmo == maxAmmo) weaponB_R_ammo.color = Color.green; else if (currAmmo == 0) weaponB_R_ammo.color = Color.red; else weaponB_R_ammo.color = Color.white;
                    break;

                case 'C':
                    weaponC_R_ammo.text = currAmmo.ToString();
                    if (currAmmo == maxAmmo) weaponC_R_ammo.color = Color.green; else if (currAmmo == 0) weaponC_R_ammo.color = Color.red; else weaponC_R_ammo.color = Color.white;
                    break;

                case 'D':
                    weaponD_R_ammo.text = currAmmo.ToString();
                    if (currAmmo == maxAmmo) weaponD_R_ammo.color = Color.green; else if (currAmmo == 0) weaponD_R_ammo.color = Color.red; else weaponD_R_ammo.color = Color.white;
                    break;
            }
        }
    }


    /// <summary>
    /// Updates the UI to show how much health the player has
    /// </summary>
    /// <param name="myHealth"> Ratio of current health / max health </param>
    public void UpdateHealthBar(float myHealth)
    {
        // Sanitize input so it's between 0 and 1
        myHealth = Mathf.Min(Mathf.Max(0, myHealth), 1);

        // Scale health bar
        healthBar_bar.transform.localScale = new Vector3(myHealth, healthBar_bar.transform.localScale.y,
            healthBar_bar.transform.localScale.z);
    }

    public void DisplayAchievementPopUp( Achievement a )
    {

        GameObject popUp = Instantiate(achievementPrefab, panel.transform);

        // Get the Tiers child
        Transform tiers = popUp.transform.Find("Tiers");
        // Get the Progress Bar child
        Transform progressBars = popUp.transform.Find("Progress Bars");

        // clear children if existing
        if (tiers.childCount != 0)
        {
            foreach (Transform t in tiers.transform)
            {
                GameObject.Destroy( t.gameObject );
            }
        }


        // Create a tier for every activation value
        for (int i = 0; i < a.activationValues.Length; i++)
        {
            Instantiate(tierPrefab, tiers);
            Instantiate(progressBarPrefab, progressBars);
        }

        // Set size of tier
        GridLayoutGroup tglg = tiers.GetComponent<GridLayoutGroup>();
        tglg.cellSize = new Vector2(tglg.cellSize.x / a.activationValues.Length, tglg.cellSize.y);
        // Set size of progress bars
        GridLayoutGroup pbglg = progressBars.GetComponent<GridLayoutGroup>();
        pbglg.cellSize = new Vector2(pbglg.cellSize.x / a.activationValues.Length, pbglg.cellSize.y);

        // Set progress bars
        int currentValue = a.currentValue;
        for (int i = 0; i < a.activationValues.Length; i++)
        {
            // Current working bar
            Transform bar = progressBars.GetChild(i);

            // This tiers max
            int max = a.activationValues[i];

            // Check if this bar needs scaled at all, break if not
            if (i > 0 && currentValue < a.activationValues[i - 1])
            {
                break;
            }
            // Check if this bar has been met/exceeded
            else if (currentValue >= max)
            {
                bar.localScale = new Vector3(1f, bar.localScale.y, bar.localScale.z);
            }
            // Set the bar scale
            else
            {
                bar.localScale = new Vector3(currentValue / max, bar.localScale.y, bar.localScale.z);
            }
        }

        /** Set fields in display */
        // Name
        popUp.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = a.achievementMessage;
        // Description
        popUp.transform.Find("Description").gameObject.GetComponent<TMP_Text>().text = a.ToString();
        // Current Count
        popUp.transform.Find("Current Count").gameObject.GetComponent<TMP_Text>().text = "Current Count: " + a.currentValue.ToString();
        // Next Count & set finished
        if (!a.IsComplete())
        {
            popUp.transform.Find("Next Count").gameObject.GetComponent<TMP_Text>().text = "Next Count: " + a.activationValues[a.nextTier].ToString();
            popUp.transform.Find("Finished").gameObject.SetActive(false);
        }
        else
        {
            popUp.transform.Find("Next Count").gameObject.GetComponent<TMP_Text>().text = "Achieved";
            popUp.transform.Find("Finished").gameObject.SetActive(true);
        }

        // Play Animation
        Animator anim = GetComponent<Animator>();
        anim.Play("Achievement PopUp");
        // Animation Event calls to destroy Pop up after animation
    }

    /// <summary>
    /// Destroys the Achievement PopUp
    /// Gets called as an Animation Event
    /// </summary>
    public void DestroyPopUpAchievement( GameObject popUp )
    {

        Transform panel = transform.Find("Canvas_Achievements").Find("Panel");
        /*if (panel.childCount != 0)
        {
            GameObject.Destroy(panel.GetChild(0).gameObject);
        }*/
        StartCoroutine( DeletePopUp( panel ) );
    }

    public IEnumerator DeletePopUp(Transform panel)
    {
        /*for (int i = 0; i < panel.childCount; i++)
        {
            Destroy( panel.GetChild( 0 ).gameObject );
            yield return new WaitForSeconds(4);
        }*/
        while (panel.childCount > 0)
        {
            Destroy(panel.GetChild(0).gameObject);
            yield return new WaitForSeconds(2);
        }
    }
}
