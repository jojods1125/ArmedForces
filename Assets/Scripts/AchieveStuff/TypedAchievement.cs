using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type of weapons (currently just base types)
/// none used as base type for checking in Manager
/// </summary>
public enum WeaponType
{
    none, auto, semi, launcher, sprayer
}

[CreateAssetMenu(fileName = "New Kill Achievement", menuName = "Achievement/Weapon Specific")]
/// <summary>
/// More properties for the Kill Achievement
/// inlcuding specifying type of kills
/// </summary>
public class TypedAchievement : Achievement
{

    [Header("Typing Properties")]
    [Tooltip("Name of the Weapon type")]
    public WeaponType weaponType;
    /** More properties? */
    /*[Tooltip("What weapon")]
    public Weapon gun;*/

}
