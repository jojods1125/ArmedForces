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
        LoadPrefs();
    }

    // ===========================================================
    //              Persisten Data (PlayerPrefs)
    // ===========================================================

    /// <summary>
    /// Save the important Achievement data based on the type of Achievement
    /// </summary>
    public void SavePrefs()
    {
        string data = "";
        // Go through all Achievements, cast to type, and get their data
        foreach (Achievement a in achievements)
		{
            if (a is A_Typed)
            {
                data += ((A_Typed)a).SaveToString() + "\n";
            }
            else if (a is A_Tiered)
            {
                data += ((A_Tiered)a).SaveToString() + "\n";
            }
            else if (a is A_Repeatable)
			{
                data += ((A_Repeatable)a).SaveToString() + "\n";
			}
		}

        // Put collective data in PlayerPrefs
        PlayerPrefs.SetString("AllAchs", data);
    }

    /// <summary>
    /// Loads the important Achievement data from PlayerPrefs
    /// </summary>
    public void LoadPrefs()
    {
        // Check if the Key exists
        if (PlayerPrefs.HasKey("AllAchs"))
        {
            // Get the data from storage
            string[] data = PlayerPrefs.GetString("AllAchs").Split('\n');
            for (int i = 0; i < data.Length - 1; i++)
            {
                string info = data[i];
                Achievement a = achievements[i];
                // Load the data into the correctly typed achievement
                if (info.Contains(a.achievementMessage))
                {
                    if (a is A_Typed)
                    {
                        ((A_Typed)a).LoadFromString(info);
                    }
                    else if (a is A_Tiered)
                    {
                        ((A_Tiered)a).LoadFromString(info);
                    }
                    else if (a is A_Repeatable)
                    {
                        ((A_Repeatable)a).LoadFromString(info);
                    }
                }
            }
        }
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
