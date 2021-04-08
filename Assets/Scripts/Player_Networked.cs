using UnityEngine;
using System.Collections;
using System;
using Mirror;

[RequireComponent(typeof(Player))]
public class Player_Networked : NetworkBehaviour
{
    // ===========================================================
    //                          VARIABLES
    // ===========================================================

    public Player player;

    [SyncVar]
    private float currHealth = 100;
    [SyncVar]// [HideInInspector]
    private int playerID;
    [SyncVar]
    private int lastAttackedID;
    [SyncVar]
    private WeaponType lastAttackedType;


    // ===========================================================
    //                            DAMAGE
    // ===========================================================

    /// <summary>
    /// Increases the player's health by health
    /// </summary>
    /// <param name="health"> Amount to increase health by </param>
    public void IncreaseHealth(float health)
    {
        player.IncreaseHealth(health);

        currHealth = player.GetCurrHealth();

        // If you die by healing for some ungodly reason, you played yourself
        if (player.GetCurrHealth() <= 0)
            RpcKill(player.GetPlayerID(), WeaponType.none);
    }
    

    /// <summary>
    /// Decreases the player's health by health and kills if at 0 (must call from server)
    /// </summary>
    /// <param name="health"> Amount to decrease health by </param>
    /// <param name="attackerID"> ID of the attacker </param>
    /// <param name="weaponType"> Type of weapon that attacked </param>
    public void DecreaseHealth(float health, int attackerID, WeaponType weaponType)
    {
        // Only calculate damage on the server
        if (!isServer)
            return;

        player.DecreaseHealth(health, attackerID, weaponType);

        currHealth = player.GetCurrHealth();
        lastAttackedID = player.GetLastAttackedID();
        lastAttackedType = player.GetLastAttackedType();

        // If the Player runs out of health, kill them
        if (player.GetCurrHealth() <= 0)
            RpcKill(attackerID, weaponType);
    }


    [Command]
    void CmdDecreaseHealthFromAI(float health, int attackerID, WeaponType weaponType)
    {
        DecreaseHealth(health, attackerID, weaponType);
    }

    public void DecreaseHealthFromAI(float health, int attackerID, WeaponType weaponType)
    {
        CmdDecreaseHealthFromAI(health, attackerID, weaponType);
    }


    /// <summary>
    /// Retrieves the player's current health
    /// </summary>
    /// <returns> currHealth float </returns>
    public float GetCurrHealth()
    {
        return player.GetCurrHealth();
    }



    // ===========================================================
    //                      DEATH AND REBIRTH
    // ===========================================================

    /// <summary>
    /// Tells the server to kill this Player
    /// </summary>
    /// <param name="killerID"> ID of the Player that killed them </param>
    /// <param name="weaponType"> Type of weapon that killed them </param>
    [Command]
    public void CmdKill(int killerID, WeaponType weaponType)
    {
        if (!isServer)
            return;

        RpcKill(killerID, weaponType);
    }


    /// <summary>
    /// Kills this instance of Player on every client
    /// </summary>
    /// <param name="killerID"> ID of the Player that killed them </param>
    /// <param name="weaponType"> Type of weapon that killed them </param>
    [ClientRpc]
    public void RpcKill(int killerID, WeaponType weaponType)
    {
        player.Kill(killerID, weaponType);

        // Updates KDR based on who killed; if a self-kill, gives kill to last attacker
        if (killerID == -1 || killerID == player.GetPlayerID())
            TrackDeath(player.GetLastAttackedID(), player.GetPlayerID(), player.GetLastAttackedType());
        else
            TrackDeath(killerID, player.GetPlayerID(), weaponType);

        // if local player, call achievement event
        if (isLocalPlayer)
            AchievementManager.Instance().OnEvent(AchievementType.deaths);
    }


    void TrackDeath(int killerID, int playerID, WeaponType weaponType)
    {
        CmdTrackDeath(killerID, playerID, weaponType);
    }


    /// <summary>
    /// Tells the server's GameManager to track the death
    /// </summary>
    /// <param name="killerID"> ID of the killer </param>
    /// <param name="playerID"> ID of the killed </param>
    /// /// <param name="weaponType"> Type of weapon that killed them </param>
    [Command]
    void CmdTrackDeath(int killerID, int playerID, WeaponType weaponType)
    {
        if (!isServer)
            return;

        GameManager_Networked.Instance().TrackDeath(killerID, playerID, weaponType);

        // Resets the last attacker and type
        player.SetLastAttackedID(player.GetPlayerID());
        player.SetLastAttackedType(WeaponType.none);
        //lastAttackedID = player.GetLastAttackedID();
        //lastAttackedType
    }


    /// <summary>
    /// Respawns the player at the specified location
    /// </summary>
    /// <param name="spawnLocation"> Location to respawn the player at </param>
    public void Respawn(Vector3 spawnLocation)
    {
        player.Respawn(spawnLocation);

        currHealth = player.GetCurrHealth();
        //player.SetLastAttackedID(playerID);
        //player.SetLastAttackedType(WeaponType.none);
    }



    // ===========================================================
    //                   START-UP AND CONNECTING
    // ===========================================================

    /// <summary>
    /// Tells the server that the Player is connected
    /// </summary>
    [Command]
    void CmdPlayerConnected()
    {
        // Exits if not the server
        if (!isServer)
            return;

        // Retrieves an ID from GameManager
        int newID = GameManager_Networked.Instance().ClientConnected();

        // Updates all instances of this Player across clients
        RpcPlayerConnected(newID);
    }


    /// <summary>
    /// Gets info from the server for this Player on every client
    /// </summary>
    /// <param name="playerID"> ID being given to this Player </param>
    [ClientRpc]
    void RpcPlayerConnected(int playerID)
    {
        // Sets the Player's ID and the last attacked ID
        player.SetPlayerID(playerID);
        player.SetLastAttackedID(player.GetPlayerID());

        this.playerID = playerID;
    }


    protected void Start()
    {
        if (isLocalPlayer)
            GameManager_Networked.Instance().gameManager.localPlayers[0] = player;

        // Tells the server that the Player is connected
        CmdPlayerConnected();

        // If a copy of the Player on a client, freeze
        if (!isLocalPlayer)
        {
            player.FreezeConstraints();
        }

        // If UI exists (only local player), connect health bar and weapon UI
        if (player.GetUIManager())
        {
            player.GetUIManager().ui_Players[0].UpdateHealthBar(player.GetCurrHealth() / player.maxHealth);
            player.GetUIManager().ui_Players[0].UpdateWeaponIcons();
        }
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        //currHealth = player.GetCurrHealth();
        //playerID = player.GetPlayerID();
        //lastAttackedID = player.GetLastAttackedID();
        //lastAttackedType = player.GetLastAttackedType();

        // Constantly update health bar (kinda nasty but works better than event-based)
        if (player.GetUIManager()) player.GetUIManager().ui_Players[0].UpdateHealthBar(currHealth / player.maxHealth);
        //player.SetLastAttackedID(lastAttackedID);
        //player.SetLastAttackedType(lastAttackedType);
    }

    /// <summary>
    /// Tells the server to update the appearance of both Arms
    /// </summary>
    public void UpdateAppearance()
    {
        player.UpdateAppearance();
    }


    /// <summary>
    /// Returns references to the Arm objects of the player
    /// </summary>
    /// <returns> Player's Arm objects </returns>
    public Arm[] GetArms()
    {
        return player.GetArms();
    }

    public void SetPlayerID(int newID)
    {
        player.SetPlayerID(newID);
    }

    public int GetPlayerID()
    {
        return player.GetPlayerID();
    }

    public void SetLastAttackedID(int newID)
    {
        player.SetLastAttackedID(newID);
    }

    public int GetLastAttackedID()
    {
        return player.GetLastAttackedID();
    }

    public void SetLastAttackedType(WeaponType type)
    {
        player.SetLastAttackedType(type);
    }

    public WeaponType GetLastAttackedType()
    {
        return player.GetLastAttackedType();
    }

    public UIManager GetUIManager()
    {
        return player.GetUIManager();
    }

    // ===========================================================
    //                           MOVEMENT
    // ===========================================================

    /// <summary>
    /// Determines if the player is on the ground
    /// </summary>
    /// <returns> True if player is on ground, false if not </returns>
    public bool IsGrounded()
    {
        return player.IsGrounded();
    }


    /// <summary>
    /// Adds the specified force to the player's RigidBody (used from local calls)
    /// </summary>
    /// <param name="force"> Force to add to the player </param>
    public void EnactForce(Vector3 force)
    {
        player.EnactForce(force);
    }


    /// <summary>
    /// Enacts force on this player on every client (used from server calls)
    /// </summary>
    /// <param name="force"> Amount of force to enact </param>
    [ClientRpc]
    public void RpcEnactForce(Vector3 force)
    {
        if (!isLocalPlayer)
            return;

        player.EnactForce(force);
    }

}
