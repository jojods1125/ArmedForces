using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sprayer", menuName = "Weapon/Sprayer")]
public class W_Sprayer : W_Shootable
{
    [Header("Sprayer Properties")]

    [Min(0)]
    [Tooltip("Maximum radius of spread")]
    public float spreadRange;

    [Min(0)]
    [Tooltip("Distance of the spray")]
    public float sprayDistance;

    [Tooltip("Damage per bullet")]
    public float bulletDamage;

    [Tooltip("Pushback the bullets inflict on collision")]
    public float bulletPushback;

    //[Tooltip("Particle effects for current sprayer")]
    //public ParticleSystem[] sprayFX;
    

    //[Tooltip("Particle system of spray")]
    //public ParticleSystem particles;
}
