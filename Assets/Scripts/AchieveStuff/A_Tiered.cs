using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement/Tiered")]
[Serializable]
public class A_Tiered : Achievement
{
    [Header("Tiered Properties")]
    [Min(0)]
    [Tooltip("Tier working towards (last met + 1)")]
    public int nextTier;
    [Tooltip("Goals of the property")]
    public int[] activationValues;

    /// <summary>
    /// Checks if the current value has passed a given threshold in activationValues
    /// </summary>
    /// <param name="i"> Index of the activationValues to check </param>
    /// <returns>True if passed, False if not</returns>
    public bool CheckValue(int i)
    {
        if ((i < activationValues.Length) && (currentValue >= activationValues[i]))
            return true;
        return false;
    }

    /// <summary>
    /// Checks to see if the next milestone has been met
    /// If met, increments nextTier
    /// </summary>
    /// <returns> True if milestone met, False otherwise </returns>
    public bool CheckNext()
    {
        if (nextTier < activationValues.Length && CheckValue(nextTier))
        {
            nextTier++;
            unlockReward(nextTier);
            Debug.Log(achievementMessage + " Achieved: " + activationValues[nextTier - 1] + "\n Total: " + currentValue);
            return CheckNext() || true;
        }
        else if (nextTier >= activationValues.Length)
        {
            // should be complete if at this point
            if (!achieved)
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
    public bool IsComplete()
    {
        if (CheckValue(activationValues.Length - 1))
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
    public void unlockReward(int tierReached)
    {
        // check to reset bool
        if (hadUnlock)
        {
            hadUnlock = false;
        }
        // go through all unlockables
        foreach (Unlockable u in unlockables)
        {
            if (!u.unlocked && u.value <= tierReached)
            {
                u.Unlock();
                lastUnlocked = u.reward.weaponName;
                hadUnlock = true;
            }
        }
    }

    /// <summary>
    /// Puts the important data into a CSV string in order to save it
    /// </summary>
    /// <returns> CSV string to save into PlayerPrefs </returns>
    public override string SaveToString()
    {
        string ret = achievementMessage + ">" + achieved + ">" + currentValue + ">" + nextTier + "|";
        foreach (Unlockable u in unlockables)
        {
            ret += u.ToString() + "\\";
        }
        return ret;
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
            nextTier = int.Parse(aData[3]);
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
