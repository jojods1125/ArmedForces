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
        // check to reset bool
        if (hadUnlock)
        {
            hadUnlock = false;
        }
        // go through all unlockables
        if (currentValue >= repeatValue)
        {
            // add number of time over achieved
            timesAchieved += currentValue / repeatValue;
            // better than setting to 0; in case over achieved
            currentValue %= repeatValue;
            unlockReward(timesAchieved);
            Debug.Log(achievementMessage + "Achieved " + timesAchieved + " times");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Unlocks the corresponding reward if its point is reached
    /// </summary>
    public void unlockReward(int repNum)
    {
        foreach (Unlockable u in unlockables)
        {
            if (!u.unlocked && u.value <= repNum)
            {
                u.Unlock();
            }
        }
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

    /// <summary>
    /// Puts the important data into a CSV string in order to save it
    /// </summary>
    /// <returns> CSV string to save into PlayerPrefs </returns>
    public override string SaveToString()
    {
        return achievementMessage + ">" + achieved + ">" + currentValue + ">" + repeatValue + ">" + timesAchieved;
    }

    /// <summary>
    /// Takes a CSV string and parses it into the usable data
    /// </summary>
    /// <param name="json"> CSV string to parse through </param>
    public override void LoadFromString(string json)
    {
        string[] data = json.Split('|');
        // data[0] => achievement data
        // data[1] => unlockables data
        string[] aData = data[0].Split('>');
        if (achievementMessage.Equals(aData[0]))
        {
            achieved = aData[1].Equals("True");
            currentValue = int.Parse(aData[2]);
            repeatValue = int.Parse(aData[3]);
            timesAchieved = int.Parse(aData[4]);
            // split by unlockables
            if (data.Length > 1)
            {
                string[] uData = data[1].Split('\\');
                for (int i = 0; i < unlockables.Count; i++)
                {
                    string curr = uData[i];
                    string[] currData = curr.Split('<');
                    unlockables[i].value = float.Parse(currData[0]);
                    unlockables[i].reward = WeaponManager.Instance().getByName(currData[1]);
                    unlockables[i].reward.unlocked = bool.Parse(currData[2]);
                    unlockables[i].unlocked = bool.Parse(currData[2]);
                }
            }
        }
    }
}
