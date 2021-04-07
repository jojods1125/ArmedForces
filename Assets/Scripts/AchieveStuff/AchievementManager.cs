using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Manager for the Achievement system
/// Logan Hobbs
/// 
/// Inspiration : https://mikeadev.net/2014/05/simple-achievement-system-in-csharp/
/// </summary>
[Serializable]
public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    public List<Achievement> achievements;

    private AchievementListContainer alc = new AchievementListContainer();

    // ===========================================================
    //                    SINGLETON PATTERN
    // ===========================================================
    private static AchievementManager instance;
    public static AchievementManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        // Singleton
        if ( instance != null && instance != this )
        {
            Destroy( this.gameObject );
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // PlayerPrefs
        // LoadPrefs();
    }

    // ===========================================================
    //              Persisten Data (PlayerPrefs)
    // ===========================================================

    public void SavePrefs()
    {
        alc.CreateSeperateLists(achievements);

        /*//string lists = JsonConvert.SerializeObject(alc);
        string lists = JsonUtility.ToJson(alc);
        PlayerPrefs.SetString("AchLists", lists);*/

        // Tiered
        //string tiered = JsonConvert.SerializeObject(alc.tiered);
        string tiered = JsonUtility.ToJson(alc.tiered, true);
        PlayerPrefs.SetString("TieredAchs", tiered);
        // Typed
        //string typed = JsonConvert.SerializeObject(alc.typed);
        string typed = JsonUtility.ToJson(alc.typed, true);
        PlayerPrefs.SetString("TypedAchs", typed);
        // Repeatable
        //string repeatable = JsonConvert.SerializeObject(alc.repeatable);
        string repeatable = JsonUtility.ToJson(alc.repeatable, true);
        PlayerPrefs.SetString("RepeatableAchs", repeatable);

        /*// string aListJSON = JsonUtility.ToJson(this, true);
        string aListJSON = JsonConvert.SerializeObject(achievements);
        PlayerPrefs.SetString("AllAchievements", aListJSON);*/
    }

    public void LoadPrefs()
    {
        /*if (PlayerPrefs.HasKey("AchLists"))
        {
            string lists = PlayerPrefs.GetString("AchLists");
            JsonUtility.FromJsonOverwrite(lists, alc);
            Debug.LogError("Loading");
            foreach (A_Typed at in alc.typed)
            {
                Debug.LogError("Typed: " + at.ToString());
            }
            foreach (A_Tiered at in alc.tiered)
            {
                Debug.LogError("Tiered: " + at.ToString());
            }
            foreach (A_Repeatable ar in alc.repeatable)
            {
                Debug.LogError("Repeatable: " + ar.ToString());
            }

            //JsonConvert.PopulateObject(lists, alc);
            achievements = alc.CreateBaseList();
        }*/

        if (PlayerPrefs.HasKey("TieredAchs") && PlayerPrefs.HasKey("TypedAchs") && PlayerPrefs.HasKey("RepeatableAchs"))
        {
            // Tiered
            string tieredString = PlayerPrefs.GetString("TieredAchs");
            //List<A_Tiered> tiered = JsonConvert.DeserializeObject<List<A_Tiered>>(tieredString);
            List<A_Tiered> tiered = JsonUtility.FromJson<List<A_Tiered>>(tieredString);
            // Typed
            string typedString = PlayerPrefs.GetString("TypedAchs");
            //List<A_Typed> typed = JsonConvert.DeserializeObject<List<A_Typed>>(typedString);
            List<A_Typed> typed = JsonUtility.FromJson<List<A_Typed>>(typedString);
            // Repeatable
            string repeatableString = PlayerPrefs.GetString("RepeatableAchs");
            //List<A_Repeatable> repeatable = JsonConvert.DeserializeObject<List<A_Repeatable>>(repeatableString);
            List<A_Repeatable> repeatable = JsonUtility.FromJson<List<A_Repeatable>>(repeatableString);
            alc.SetLists(tiered, typed, repeatable);
            achievements = alc.CreateBaseList();
        }

        /*if (PlayerPrefs.HasKey("AllAchievements"))
        {
            string aListJSON = PlayerPrefs.GetString("AllAchievements");
            // achievements = JsonUtility.FromJson<AchievementManager>(aListJSON).achievements;
            achievements = JsonConvert.DeserializeObject<List<Achievement>>(aListJSON);
        }*/
    }

    // ===========================================================
    //                      EVENT SYSTEM
    // ===========================================================

    /// <summary>
    /// Given the Achievement Type, add the amount to the Achievement
    /// </summary>
    /// <param name="aType"> Type of Achievement </param>
    /// <param name="amount"> Amount to add to the Achievement </param>
    /// <param name="wType"> Type of Weapon if needed</param>
    public void OnEvent(AchievementType aType, int amount = 1, WeaponType wType = WeaponType.none)
    {
        bool updated = false;
        // iterate through logged achievements and check against parameters
        Achievement curr = null;
        foreach ( Achievement ach in achievements )
        {
            if ( ach.type == aType )
            {
                // if no weapon type specified, add
                if (wType == WeaponType.none)
                {
                    ach.AddValue(amount);
                    updated = true;
                    curr = ach;
                    break;
                }
                else if (((A_Typed)ach).weaponType == wType)
                {
                    // weapon type was specified, check
                    ach.AddValue( amount );
                    updated = true;
                    curr = ach;
                    break;
                }
            }            
        }

        // Check if milestone completed?
        if (updated && curr != null)
        {
            if (curr is A_Tiered)
            {
                if (((A_Tiered)curr).CheckNext()) // print out now done in CheckNext method
                {
                    GameManager.Instance().uiManager.DisplayAchievementPopUp(curr);
                }
                updated = false;
            }
            // Debug.Log("Saving achievements");
            SavePrefs();
        }
        else
        {
            // Problem occured, should not happen
            Debug.LogError("No Achievement found OnEvent() call\n" +
                "Achievement Type: " + aType + "\n" +
                "Weapon Type: " + wType + "\n");
        }
    }

    /// <summary>
    /// Fixed Update called every fixed tic
    /// Used to check if Reset key is pressed
    /// </summary>
    private void FixedUpdate()
    {
        // Check for reset button (Start and Select)
        if (Gamepad.current != null && Gamepad.current.added && Gamepad.current[GamepadButton.Start].isPressed && Gamepad.current[GamepadButton.Select].isPressed)
        {
            // Go through each achievement and reset the currentValue
            foreach (Achievement ach in achievements)
            {
                ach.currentValue = 0;
                ach.achieved = false;
                // Set nextTier to 0 if tiered
                if (ach is A_Tiered)
                {
                    ((A_Tiered)ach).nextTier = 0;
                }
            }
            SavePrefs();
        }
    }
}
