using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    /// <param name="type"> Type of Achievement </param>
    /// <param name="amount"> Amount to add to the Achievement </param>
    public void OnEvent(AchievementType type, int amount)
    {
        foreach ( Achievement ach in achievements )
        {
            if ( ach.type == type )
            {
                ach.AddValue( amount );
                // Check if milestone completed?
                /*if (ach.CheckNext())
                {
                    // print out?
                    // tie to UI?
                    // log "message! Achieved: 'milestone'
                    //      Total: 'currentValue'"
                    Debug.Log(ach.achievementMessage + " Achieved: " + ach.activationValues[ach.nextTier - 1] + "\n Total: " + ach.currentValue);
                }*/

            }
        }
    }

}
