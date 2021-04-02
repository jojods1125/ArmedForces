﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement/Tiered")]
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
}
