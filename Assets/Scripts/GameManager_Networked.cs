using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;



public class GameManager_Networked : NetworkBehaviour
{
    public GameManager gameManager;

    /// <summary> Current number of seconds since match began </summary>
    [SyncVar]
    private float currentGameTime = 0;



    // ===========================================================
    //                           START-UP
    // ===========================================================

    /// <summary> Singleton instance </summary>
    private static GameManager_Networked instance;

    /// <summary>
    /// Retrieve singleton instance
    /// </summary>
    /// <returns> Instance of GameManager </returns>
    public static GameManager_Networked Instance()
    {
        return instance;
    }


    void Awake()
    {
        // Debug.LogError("I SHOULD BE FIRST");
        // Set singleton
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        // Increase game time every serverGameTimeStep
        InvokeRepeating(nameof(IncrementTime), 0, gameManager.serverGameTimeStep);
    }

    //private void Update()
    //{
    //    foreach (int death in deaths)
    //    {
    //        gameManager.Set
    //    }
    //}

    // ===========================================================
    //                      CLIENT CONNECTIONS
    // ===========================================================

    /// <summary>
    /// Called on server when a player connects to the game
    /// </summary>
    /// <returns> New ID for the player </returns>
    public int ClientConnected()
    {
        // THIS RUNS ON SERVER
        if (!isServer)
            return -1;

        // Gives ID to Player
        int newID = gameManager.ClientConnected();

        int[] killsArray = new int[gameManager.GetKills().Keys.Count];
        for (int i = 0; i < killsArray.Length; i++)
        {
            killsArray[i] = gameManager.GetKills()[i];
        }

        int[] deathsArray = new int[gameManager.GetDeaths().Keys.Count];
        for (int i = 0; i < deathsArray.Length; i++)
        {
            deathsArray[i] = gameManager.GetDeaths()[i];
        }

        RpcUpdateKills(killsArray);
        RpcUpdateDeaths(deathsArray);

        return newID;
    }


    /// <summary>
    /// Tells every instance of GameManager that a player connected
    /// </summary>
    /// <param name="newID"> ID of the new player </param>
    [ClientRpc]
    public void RpcUpdateKills(int[] killsArray)
    {
        if (isServer)
            return;

        Dictionary<int, int> newKills = new Dictionary<int, int>();

        for (int i = 0; i < killsArray.Length; i++)
        {
            newKills.Add(i, killsArray[i]);
        }

        gameManager.SetKills(newKills);
    }

    [ClientRpc]
    public void RpcUpdateDeaths(int[] deathsArray)
    {
        if (isServer)
            return;

        Dictionary<int, int> newDeaths = new Dictionary<int, int>();

        for (int i = 0; i < deathsArray.Length; i++)
        {
            newDeaths.Add(i, deathsArray[i]);
        }

        gameManager.SetDeaths(newDeaths);
    }


    // ===========================================================
    //                     TIME AND UI UPDATES
    // ===========================================================

    /// <summary>
    /// Increments time
    /// </summary>
    void IncrementTime()
    {
        // Increases the server's time by serverGameTimeStep, which is a SyncVar
        if (isServer)
        {
            currentGameTime += gameManager.serverGameTimeStep;
        }

        gameManager.SetCurrentGameTime(currentGameTime);

        gameManager.IncrementTime();
    }



    // ===========================================================
    //                      DEATH AND REBIRTH
    // ===========================================================

    /// <summary>
    /// Updates the server's kills and deaths dictionaries
    /// </summary>
    /// <param name="killer"> ID of the player that killed </param>
    /// <param name="deceased"> ID of the player that died </param>
    /// /// <param name="weaponType"> Type of weapon that killed </param>
    public void TrackDeath(int killer, int deceased, WeaponType weaponType)
    {
        // THIS RUNS ON SERVER
        if (!isServer)
            return;

        //gameManager.TrackDeath(killer, deceased, weaponType);

        RpcTrackDeath(killer, deceased, weaponType);
    }
    
    [ClientRpc]
    public void RpcTrackDeath(int killer, int deceased, WeaponType weaponType)
    {
        gameManager.TrackDeath(killer, deceased, weaponType);
    }



    // ===========================================================
    //                       SERVER MESSAGING
    // ===========================================================

    /// <summary>
    /// Tell the server to send a message across all clients
    /// </summary>
    /// <param name="msg"> Message to send </param>
    public void ServerMessage(string msg) //this will be serverside
    {
        if (!isServer)
            return;

        RpcServerMessage(msg);
    }


    /// <summary>
    /// Send a message across all clients
    /// </summary>
    /// <param name="msg"> Message to send </param>
    [ClientRpc]
    private void RpcServerMessage(string msg)
    {
        Debug.LogError("SERVER MSG: " + msg);
    }


}
