﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manager for the Achievement system
/// Logan Hobbs
/// 
/// Inspiration : https://mikeadev.net/2014/05/simple-achievement-system-in-csharp/
/// </summary>
public class AchievementManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // ?
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ?
    }

    private static AchievementManager _instance;
    public static AchievementManager Instance
    {
        get
        {
            if ( _instance == null )
            {
                _instance = FindObjectOfType<AchievementManager>();
                if ( _instance == null )
                {
                    GameObject go = new GameObject();
                    go.name = "SingletonController";
                    _instance = go.AddComponent<AchievementManager>();

                    DontDestroyOnLoad(go);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            Debug.LogError("Destroyed a duplicate instance of AchievementManager!");
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public List<Achievement> achievements;

    /// <summary>
    /// Given the Achievement Type, add the amount to the Achievement
    /// </summary>
    /// <param name="type"> Type of Achievement </param>
    /// <param name="amount"> Amount to add to the Achievement </param>
    public void OnEvent(AchievementType type, int amount)
    {
        foreach ( Achievement ach in achievements )
        {
            if ( ach.type == type )
            {
                ach.AddValue( amount );
                // Check if milestone completed?
                /*if (ach.CheckNext())
                {
                    // print out?
                    // tie to UI?
                    // log "message! Achieved: 'milestone'
                    //      Total: 'currentValue'"
                    Debug.Log(ach.achievementMessage + " Achieved: " + ach.activationValues[ach.nextTier - 1] + "\n Total: " + ach.currentValue);
                }*/

            }
        }
    }

}
