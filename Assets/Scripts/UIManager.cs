using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Player UI Managers")]
    public UI_Player[] ui_Players = new UI_Player[4];

    [Header("Match Info References")]
    public GameObject matchClock;

    [Header("DEBUG: Player Stat References")]
    public GameObject p1_KDR;
    public GameObject p2_KDR;
    public GameObject p3_KDR;
    public GameObject p4_KDR;
    

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
            //Debug.Log("DISPLAYING ANIMATION FOR" + " " + a.name);
            //popUp.gameObject.name += popUpCount++;

            /** Set fields in display */
            // Name
            popUp.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = a.achievementMessage;

            // Reward
            if (a.hadReward())
            {
                popUp.transform.Find("Reward?").gameObject.GetComponent<TMP_Text>().text = "Unlocked: " + a.lastReward();
            }
            // Set finished if tiered
            if (a is A_Tiered)
            {
                popUp.transform.Find("Finished").gameObject.SetActive(((A_Tiered)a).IsComplete());
            }

            // Play Animation
            anim.Play("Achievement PopUp");
            popUpCount++;
            // Animation Event calls to destroy Pop up after animation
        }
    }

    internal void UpdateAmmoUI(ArmSide armSide, char index, int currAmmo, int maxAmmo, int playerID)
    {
        ui_Players[playerID].UpdateAmmoUI(armSide, index, currAmmo, maxAmmo);
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
