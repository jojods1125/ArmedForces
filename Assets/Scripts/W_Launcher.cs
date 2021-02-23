using UnityEngine;

[CreateAssetMenu(fileName = "New Launcher", menuName = "Weapon/Launcher")]
public class W_Launcher : W_Shootable
{
    [Header("Launcher Properties")]
    [Min(0)]
    public float projectilePower;
    public GameObject projectilePrefab;
    [Min(0)]
    public float explosionRadius;
    public float coreDamage;
    public float corePushback;
    public bool explosive;
    public bool rocketPowered;
}
