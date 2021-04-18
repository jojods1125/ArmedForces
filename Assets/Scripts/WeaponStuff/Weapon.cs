using UnityEngine;

public class Weapon : ScriptableObject
{
    [Header("Weapon Properties")]

    [Tooltip("Name of the weapon as it appears in-game")]
    public string weaponName;

    [Tooltip("Description of the weapon as it appears in-game")]
    public string description;

    [Tooltip("Mesh of the weapon as it appears in-game")]
    public Mesh mesh;

    [Tooltip("Material of the weapon as it appears in-game")]
    public Material material;

    [Tooltip("Icon to represent the weapon in UI")]
    public Sprite icon;

    [Tooltip("Rarity level of the weapon")]
    public WeaponRarity rarity;

    [Tooltip("Is Weapon Unlocked")]
    public bool unlocked = false;
}

public enum WeaponRarity
{
    Common,     // Gray bg
    Uncommon,   // Green bg
    Rare,       // Blue bg
    Legendary   // Purple bg
}