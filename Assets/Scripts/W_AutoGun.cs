using UnityEngine;

[CreateAssetMenu(fileName = "New Auto Gun", menuName = "Weapon/Automatic")]
public class W_AutoGun : W_Shootable
{
    [Header("Automatic Gun Properties")]
    [Min(0)]
    public float spreadRange;
    public float bulletDamage;
}
