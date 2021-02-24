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

    [Tooltip("Pushback force per bullet fired")]
    public float pushback;
}
