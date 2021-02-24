using UnityEngine;

[CreateAssetMenu(fileName = "New Semi Gun", menuName = "Weapon/Semiautomatic")]
public class W_SemiGun : W_Shootable
{
    [Header("Semiautomatic Gun Properties")]

    [Min(0)]
    [Tooltip("Maximum radius of spread")]
    public float spreadRange;

    [Tooltip("Damage per bullet")]
    public float bulletDamage;

    [Min(0)]
    [Tooltip("Number of bullets shot on trigger press")]
    public int burstCount;
}
