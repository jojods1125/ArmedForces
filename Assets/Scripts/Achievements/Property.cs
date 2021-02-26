using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Basic idea of what Achievements need as properties
 * Logan Hobbs
 * 
 * Inspiration : https://unitylist.com/p/jdu/Achievements-Unity
 */

public enum PropertyName
{
    kills, deaths, wins, shotsFired
}

[CreateAssetMenu(fileName = "New Property", menuName = "Property")]
/// <summary>
/// Base properties of an Achievement
/// </summary>
public class Property : ScriptableObject
{
    [Header("Achievement Properties")]
    [Tooltip("Name of the property type")]
    public PropertyName _pName;
    [Min(0)]
    [Tooltip("Value used to keep track of progress")]
    public int value;
    [Tooltip("Goals of the property")]
    public int[] activationValues;
    [Min(0)]
    [Tooltip("Initial value of the property (typically 0)")]
    public int initialValue;
}
