using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unlockable", menuName = "Unlockable")]
public class Unlockable : ScriptableObject
{
    [Header("Unlockable Information")]
    [Tooltip("Tier/RepeatValue to unlock reward")]
    public float value;
    [Tooltip("Weapon to reward")]
    public Weapon reward;
    [Tooltip("Is the reward unlocked")]
    public bool unlocked = false;

    public void Unlock()
    {
        unlocked = true;
        reward.unlocked = true;
    }

    public override string ToString()
    {
        return value + "<" + reward.weaponName + "<" + unlocked;
    }
}
