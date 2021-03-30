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
    kills, deaths, wins, shotsFired
}

/** Secondary Type Enum & List of Weapons with new scriptable objs */

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement/Normal")]
/// <summary>
/// Base Achievement variables and methods
/// </summary>
public class Achievement : ScriptableObject
{
    [Header("Achievement")]
    [Tooltip("Name of the achievement")]
    public string achievementMessage;
    /*[Tooltip("Description of achievement")]
    public string achievementDescription;*/
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
    /// Checks if the current value has passed a given threshold in activationValues
    /// </summary>
    /// <param name="i"> Index of the activationValues to check </param>
    /// <returns>True if passed, False if not</returns>
    public bool CheckValue(int i)
    {
        if ( ( i < activationValues.Length ) && (currentValue >= activationValues[i]) )
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
        if ( nextTier < activationValues.Length && CheckValue(nextTier) )
        {
            nextTier++;
            Debug.Log(achievementMessage + " Achieved: " + activationValues[nextTier - 1] + "\n Total: " + currentValue);
            return CheckNext() || true;
        }
        else if ( nextTier >= activationValues.Length )
        {
            // should be complete if at this point
            if ( !achieved )
            {
                IsComplete();
            }
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
        achieved = false;
        return false;
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
    /// and secondary type, if applicable.
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
            default:
                returnable = "ERROR IN \'Achievement.toString()\' METHOD";
                break;
        }

        return returnable;
    }
}
