using UnityEngine;

[CreateAssetMenu(fileName = "New Launcher", menuName = "Weapon/Launcher")]
public class W_Launcher : W_Shootable
{
    [Header("Launcher Properties")]
    [Min(0)]
    [Tooltip("Speed at which the projectile travels")]
    public float projectilePower;
    [Tooltip("Prefab for the projectile")]
    public GameObject projectilePrefab;
    [Tooltip("Determines if gravity affects projectile; set to true for no gravity")]
    public bool rocketPowered;
    [Space(7)]
    [Min(0)]
    [Tooltip("Radius of AoE explosion; set to 0 for no explosion")]
    public float explosionRadius;
    [Tooltip("Damage dealt at the core of the projectile's explosion or impact point")]
    public float coreDamage;
    [Tooltip("Pushback dealt at the core of the projectile's explosion or impact point")]
    public float corePushback;
    
}
