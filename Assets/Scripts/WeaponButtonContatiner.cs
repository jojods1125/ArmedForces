using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonContatiner : MonoBehaviour
{
    [Header("Weapon")]
    [Tooltip("Weapon tied to the button")]
    public Weapon weapon;
    [Tooltip("Weapon Damage")]
    public float damage;
    [Tooltip("Weapon Recoil")]
    public float recoil;
    [Tooltip("Weapon Pushback")]
    public float pushback;
    [Tooltip("Variable Attribute")]
    public float variable;
    [Tooltip("Ammo Capacity")]
    public float ammoCapacity;
    [Tooltip("Reload Speed")]
    public float reloadSpeed;
}
