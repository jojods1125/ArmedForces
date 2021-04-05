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

    /// <summary> Dictionary of player IDs to kills </summary>
    private SyncDictionary<int, int> kills = new SyncDictionary<int, int>();
    /// <summary> Dictionary of player IDs to deaths </summary>
    private SyncDictionary<int, int> deaths = new SyncDictionary<int, int>();



    // ===========================================================
    //                           START-UP
    // ===========================================================

    //Returns the closest spawn point to a position
    public Vector3 getCloseRespawnPoint(Vector3 pos)
    {
        return gameManager.getCloseRespawnPoint(pos);
    }


    public Vector3 getRandomRespawnPoint()
    {
        return gameManager.getRandomRespawnPoint();
    }

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
        return gameManager.ClientConnected();
    }

    public int AIConnected()
    {
        // Gives ID to Player
        return gameManager.AIConnected();
    }


    /// <summary>
    /// Tells every instance of GameManager that a player connected
    /// </summary>
    /// <param name="newID"> ID of the new player </param>
    [ClientRpc]
    public void RpcClientConnected(int newID)
    {
        gameManager.RpcClientConnected(newID);
    }


    /// <summary>
    /// Updates playerCount on the server and retrieves it as the ID
    /// </summary>
    /// <returns> New ID for the player </returns>
    int getID()
    {
        // THIS RUNS ON SERVER
        if (!isServer)
            return -1;

        return gameManager.getID();
    }

    int getAIID()
    {
        return gameManager.getAIID();
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

        gameManager.TrackDeath(killer, deceased, weaponType);
    }


    /// <summary>
    /// Respawn the specified GameObject
    /// </summary>
    /// <param name="obj"> GameObject to respawn </param>
    public void Respawn(GameObject obj)
    {
        gameManager.Respawn(obj);
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
