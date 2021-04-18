using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type of weapons (currently just base types)
/// none used as base type for checking in Manager
/// </summary>
[Serializable]
public enum WeaponType
{
    none, auto, semi, launcher, sprayer
}

[CreateAssetMenu(fileName = "New Kill Achievement", menuName = "Achievement/Weapon Specific")]
/// <summary>
/// More properties for the Kill Achievement
/// inlcuding specifying type of kills
/// </summary>
[Serializable]
public class A_Typed : A_Tiered
{

    [Header("Typing Properties")]
    [Tooltip("Name of the Weapon type")]
    public WeaponType weaponType;
    /** More properties? */
    /*[Tooltip("What weapon")]
    public Weapon gun;*/

    public override string ToString()
    {
        // Get the base Achievements toString()
        string returnable = base.ToString() + " with a";

        // Depending on subtype, add to Description
        switch (weaponType)
        {
            case WeaponType.auto:
                returnable += "n Automatic Weapon";
                break;
            case WeaponType.semi:
                returnable += " Semi-Automatic Weapon";
                break;
            case WeaponType.launcher:
                returnable += " Launcher Weapon";
                break;
            case WeaponType.sprayer:
                returnable += " Sprayer Weapon";
                break;
            default:
                returnable = "ERROR IN \'TypedAchievement.toString()\' METHOD";
                break;
        }

        return returnable;
    }

}
