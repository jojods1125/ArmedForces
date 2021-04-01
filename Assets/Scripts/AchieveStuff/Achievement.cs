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
    kills, deaths, wins, shotsFired, games
}

/// <summary>
/// Base Achievement variables and methods
/// </summary>
public class Achievement : ScriptableObject
{
    [Header("Achievement Info")]
    [Tooltip("Name of the achievement")]
    public string achievementMessage;
    [Tooltip("Whether the Achievement has been achieved")]
    public bool achieved;

    [Header("Achievement Properties")]
    [Tooltip("Name of the property type")]
    public AchievementType type;
    [Min(0)]
    [Tooltip("Value used to keep track of progress")]
    public int currentValue;

    /**
     * CREATE DICTIONARY<INT, WEAPON> FOR UNLOCKABLES
     * CREATE METHOD TO CHECK FOR UNLOCKABLES
     */


    /// <summary>
    /// Adds a given amount to the property value
    /// </summary>
    /// <param name="amount"> Amount to add to the currentValue for counting </param>
    public void AddValue(int amount)
    {
        currentValue += amount;
    }

    /// <summary>
    /// Unlocks the corresponding reward if its point is reached
    /// </summary>
    public void unlockReward ()
    {
        
    }

    /// <summary>
    /// Says whether the last tier had a reward tied to it
    /// </summary>
    /// <returns> Whether or not the last tier had a reward </returns>
    public bool hadReward ()
    {
        return true;
    }

    /// <summary>
    /// Gives the last reward unlocked if any
    /// </summary>
    /// <returns> Last unlocked reward, null otherwise </returns>
    public string lastReward()
    {
        return null;
    }

    /// <summary>
    /// Creates a string description of the Achievement based on the type
    /// Meant to overidden if more info needed
    /// </summary>
    /// <returns> Description of Achievement </returns>
    public override string ToString()
    {
        // Initial Description
        string returnable = "Number of ";

        // Depending on type, add to Description
        switch (type)
        {
            case AchievementType.kills:
                returnable += "Kills";
                break;
            case AchievementType.deaths:
                returnable += "Deaths";
                break;
            case AchievementType.shotsFired:
                returnable += "Shots Fired";
                break;
            case AchievementType.wins:
                returnable += "Wins";
                break;
            case AchievementType.games:
                returnable += "Games Played";
                break;
            default:
                returnable = "ERROR IN \'Achievement.toString()\' METHOD";
                break;
        }

        return returnable;
    }
}
