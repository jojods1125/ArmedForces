using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Repeatable Achievement", menuName = "Achievement/Repeatable")]
[Serializable]
public class A_Repeatable : Achievement
{
    [Header("Repeat Value")]
    public int repeatValue;

    // Number of times the repeatValue has been met
    public int timesAchieved = 0;

    /// <summary>
    /// Checks to see if the repeatable milestone has been met
    /// If met, increment timesAchieved and "reset" count
    /// </summary>
    /// <returns> True if milestone met, False otherwise </returns>
    public bool CheckMet ()
    {
        if (currentValue >= repeatValue)
        {
            // add number of time over achieved
            timesAchieved += currentValue / repeatValue;
            // better than setting to 0; in case over achieved
            currentValue %= repeatValue;
            Debug.Log(achievementMessage + "Achieved " + timesAchieved + " times");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates a string description of the Achievement based on the type
    /// and tells number of times goal has been reached
    /// </summary>
    /// <returns> Description of Achievement </returns>
    public override string ToString()
    {
        return base.ToString() + ": Goal of " + repeatValue + " reached " + timesAchieved + " times";
    }
}
