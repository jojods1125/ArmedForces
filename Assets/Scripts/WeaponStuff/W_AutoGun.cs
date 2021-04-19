using UnityEngine;

[CreateAssetMenu(fileName = "New Auto Gun", menuName = "Weapon/Automatic")]
public class W_AutoGun : W_Shootable
{
    [Header("Automatic Gun Properties")]

    [Min(0)]
    [Tooltip("Maximum radius of spread")]
    public float spreadRange;

    [Tooltip("Damage per bullet")]
    public float bulletDamage;

    [Tooltip("Pushback the bullets inflict on collision")]
    public float bulletPushback;
}
