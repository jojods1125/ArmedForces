using UnityEngine;

[CreateAssetMenu(fileName = "New Semi Gun", menuName = "Weapon/Semiautomatic")]
public class W_SemiGun : W_Shootable
{
    [Header("Semiautomatic Gun Properties")]
    [Min(0)]
    public float spreadRange;
    public float bulletDamage;
    [Min(0)]
    public int burstCount;
}
