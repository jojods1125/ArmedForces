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
}
