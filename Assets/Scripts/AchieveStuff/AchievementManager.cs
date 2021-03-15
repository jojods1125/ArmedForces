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
public class AchievementManager : MonoBehaviour
{
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
        if ( instance != null && instance != this )
        {
            Destroy( this.gameObject );
        }
        else
        {
            instance = this;
        }
    }

    // ===========================================================
    //                      EVENT SYSTEM
    // ===========================================================

    public List<Achievement> achievements;

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
                else if (((TypedAchievement)ach).weaponType == wType)
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
            /*if (curr.CheckNext())
            {
                // print out?
                // tie to UI?
                // log "message! Achieved: 'milestone'
                //      Total: 'currentValue'"
                Debug.Log(curr.achievementMessage + " Achieved: " + curr.activationValues[curr.nextTier - 1] + "\n Total: " + curr.currentValue);
            }*/
            curr.CheckNext(); // print out now done in CheckNext method
            updated = false;
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
        if ( Gamepad.current[GamepadButton.Start].isPressed && Gamepad.current[GamepadButton.Select].isPressed)
        {
            // Go through each achievement and reset the currentValue and nextTier to 0
            foreach ( Achievement ach in achievements )
            {
                ach.currentValue = 0;
                ach.nextTier = 0;
            }
        }
    }

}
