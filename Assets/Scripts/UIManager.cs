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
    public GameObject achievementPopUpPrefab;
    [Tooltip("Tier Prefab")]
    public GameObject tierPrefab;
    [Tooltip("Progress Bar Prefab")]
    public GameObject progressBarPrefab;
    [Tooltip("Panel containing Achievement")]
    public GameObject panel;
    [Tooltip("Animator Controller")]
    public RuntimeAnimatorController rac;

    // number of popUps made
    private float popUpCount = 0f;
    private Queue<Achievement> popUpQueue = new Queue<Achievement>();

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

    /// <summary>
    /// Display a PopUp achievement when called and start an animation for it
    /// </summary>
    /// <param name="a"></param>
    public void DisplayAchievementPopUp( Achievement a )
    {
        popUpQueue.Enqueue(a);
        // call coroutine
        //StartCoroutine( DisplayPopUp( a ) );
    }

    private void Update()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        // if animation not playing
        if (animState.length <= animState.normalizedTime && popUpQueue.Count > 0)
        {
            // Delete popUp if one exists
            if (popUpCount > 0)
            {
                Destroy(transform.Find("Canvas_Achievements").Find("Panel").GetChild(0).gameObject);
                popUpCount--;
            }

            // get achievement
            Achievement a = popUpQueue.Dequeue();
            GameObject popUp = Instantiate(achievementPopUpPrefab, panel.transform);
            Debug.Log("DISPLAYING ANIMATION FOR" + " " + a.name);
            //popUp.gameObject.name += popUpCount++;

            /** Set fields in display */
            // Name
            popUp.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = a.achievementMessage;

            // Reward
            if (a.hadReward())
            {
                popUp.transform.Find("Reward?").gameObject.GetComponent<TMP_Text>().text = "Unlocked: " + a.lastReward();
            }
            // Set finished
            popUp.transform.Find("Finished").gameObject.SetActive(a.IsComplete());

            // Play Animation
            anim.Play("Achievement PopUp");
            popUpCount++;
            // Animation Event calls to destroy Pop up after animation
        }
    }

    /*public IEnumerator DisplayPopUp( Achievement a )
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        while ( animState.length > animState.normalizedTime && popUpCount > 0)
        {
            Debug.Log("WAITING FOR ANIMATION" + " " + a.name);
            yield return new WaitForSeconds(3);
        }

        // Animation not running, delete popUp if one exists
        if (popUpCount > 0)
        {
            Destroy(transform.Find("Canvas_Achievements").Find("Panel").GetChild(0).gameObject);
            popUpCount--;
        }

        GameObject popUp = Instantiate(achievementPopUpPrefab, panel.transform);
        Debug.Log("DISPLAYING ANIMATION FOR" + " " + a.name);
        //popUp.gameObject.name += popUpCount++;

        //                              Set fields in display 
        // Name
        popUp.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = a.achievementMessage;

        // Reward
        if (a.hadReward())
        {
            popUp.transform.Find("Reward?").gameObject.GetComponent<TMP_Text>().text = "Unlocked: " + a.lastReward();
        }
        // Set finished
        popUp.transform.Find("Finished").gameObject.SetActive(a.IsComplete());

        // Play Animation
        anim.Play("Achievement PopUp");
        popUpCount++;
        // Animation Event calls to destroy Pop up after animation
    }

    /// <summary>
    /// Destroys the Achievement PopUp
    /// Gets called as an Animation Event
    /// </summary>
    /*public void DestroyPopUpAchievement( GameObject popUp )
    {

        Transform panel = transform.Find("Canvas_Achievements").Find("Panel");
        StartCoroutine( DeletePopUp( panel ) );
    }*/

    /// <summary>
    /// Helper Method to DestroyPopUpAchievement
    /// waits while childCount is greater than 0
    /// </summary>
    /// <param name="panel"> Panel object to look at </param>
    /// <returns> wait </returns>
    /*public IEnumerator DeletePopUp(Transform panel)
    {
        while (panel.childCount > 0)
        {
            Destroy(panel.GetChild(0).gameObject);
            popUpCount--;
            yield return new WaitForSeconds(2);
        }
    }*/
}
