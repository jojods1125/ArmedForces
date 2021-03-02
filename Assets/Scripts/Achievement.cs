using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Base idea of what Achievements are
 * Logan Hobbs
 * 
 * Inspiration : https://unitylist.com/p/jdu/Achievements-Unity
 */

/// <summary>
/// Types of Achievements
/// Used for checking in Manager
/// </summary>
public enum AchievementType
{
    kills, deaths, wins, autoShotsFired, semiShotsFired, launcherShotsFired, sprayerShotsFired
}

/** Secondary Type Enum & List of Weapons with new scriptable objs */

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
/// <summary>
/// Base Achievement variables and methods
/// </summary>
public class Achievement : ScriptableObject
{
    [Header("Achievement")]
    [Tooltip("Name of the achievement")]
    public string achievementMessage;
    [Min(0)]
    [Tooltip("Tier working towards (last met + 1)")]
    public int nextTier;
    [Tooltip("Whether the Achievement has been achieved")]
    public bool achieved;

    [Header("Achievement Properties")]
    [Tooltip("Name of the property type")]
    public AchievementType type;
    [Min(0)]
    [Tooltip("Value used to keep track of progress")]
    public int currentValue;
    [Tooltip("Goals of the property")]
    public int[] activationValues;
    [Min(0)]
    [Tooltip("Initial value of the property (typically 0)")]
    public int initialValue;

    /// <summary>
    /// Adds a given amount to the property value
    /// </summary>
    /// <param name="amount"> Amount to add to the currentValue for counting </param>
    public void AddValue(int amount)
    {
        currentValue += amount;
    }

    /// <summary>
    /// Checks if the current value has passed a given threshold in activationValues
    /// </summary>
    /// <param name="i"> Index of the activationValues to check </param>
    /// <returns>True if passed, False if not</returns>
    public bool CheckValue(int i)
    {
        if (currentValue >= activationValues[i])
            return true;
        return false;
    }

    /// <summary>
    /// Checks to see if the next milestone has been met
    /// If met, increments nextTier
    /// </summary>
    /// <returns> True if milestone met, False otherwise </returns>
    public bool CheckNext ()
    {
        if ( CheckValue(nextTier) )
        {
            nextTier++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks to see if all milestones have been met (is the achievement complete)
    /// If completed, set acheived to true
    /// </summary>
    /// <returns> True if all milestones met, False otherwise </returns>
    public bool IsComplete ()
    {
        if ( CheckValue(activationValues.Length - 1) )
        {
            achieved = true;
            return true;
        }
        return false;
    }
}
