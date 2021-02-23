using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Shootable : Weapon
{
    [Header("Firing Properties")]
    [Min(0)]
    public float fireRate;
    [Min(0)]
    public int ammoCapacity;
    public float pushback;
}
