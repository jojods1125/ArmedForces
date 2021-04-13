using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Shootable : Weapon
{
    [Header("Firing Properties")]

    [Min(0)]
    [Tooltip("Delay between shots in seconds")]
    public float fireRate;

    [Min(0)]
    [Tooltip("Capacity of ammo for weapon")]
    public int ammoCapacity;

    [Tooltip("Recoil force per bullet fired")]
    public float recoil;

    [Min(0)]
    [Tooltip("Time it takes to reload one bullet while grounded")]
    public float reloadRate = 0.1f;

    [Min(0)]
    [Tooltip("Increment of distance for damage dropoff  ( DMG * [DROPMOD ^ DIS(U) / DROPDIST] )")]
    public float dropoffDist = 0;

    [Range(0, 1)]
    [Tooltip("Modifier of damage decreased per dropoff distance  ( DMG * [DROPMOD ^ DIS(U) / DROPDIST] )")]
    public float dropoffMod = 1;
}
