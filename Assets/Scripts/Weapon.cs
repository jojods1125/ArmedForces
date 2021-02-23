using UnityEngine;

public class Weapon : ScriptableObject
{
    [Header("Weapon Properties")]
    public string weaponName;
    public string description;
    public Mesh mesh;
    public Material material;
}
