//using UnityEngine;
//using System.Collections;
//using System;
//using Mirror;

//public class Player_AI : MonoBehaviour
//{
//    // ===========================================================
//    //                          VARIABLES
//    // ===========================================================

//    [Header("Player Data")]
//    [Min(0)]
//    public float maxHealth = 100;
//    public float walkMomentum = 10;
//    public float maxWalkSpeed = 5;
//    [Min(0)]
//    public int jumpForce = 100;

//    [Header("Weapon Loadouts")]
//    public Weapon[] backArmWeapons = new Weapon[4];
//    public Weapon[] frontArmWeapons = new Weapon[4];

//    protected Rigidbody rb;
//    //private Arm_AI[] arms;
//    private float currHealth = 100;
//    private bool dying = false;

//    //Used for differentiating each player in GameManager
//    public int playerID;

//    // Last player ID to have attacked, reset on death
//    private int lastAttackedID;

//    // Last weapon to have attacked, reset to none on death
//    private WeaponType lastAttackedType;

//    // UI Manager
//    protected UIManager uiManager;



//    // ===========================================================
//    //                            DAMAGE
//    // ===========================================================

//    /// <summary>
//    /// Increases the player's health by health
//    /// </summary>
//    /// <param name="health"> Amount to increase health by </param>
//    public void IncreaseHealth(float health)
//    {
//        // Increases health and prevents invalid values
//        currHealth = Mathf.Max(Mathf.Min(currHealth + health, maxHealth), 0);

//        // If you die by healing for some ungodly reason, you played yourself
//        if (currHealth == 0)
//            RpcKill(playerID, WeaponType.none);
//    }


//    /// <summary>
//    /// Decreases the player's health by health and kills if at 0 (must call from server)
//    /// </summary>
//    /// <param name="health"> Amount to decrease health by </param>
//    /// <param name="attackerID"> ID of the attacker </param>
//    /// <param name="weaponType"> Type of weapon that attacked </param>
//    public void DecreaseHealth(float health, int attackerID, WeaponType weaponType)
//    {
//        // Decreases health and prevents invalid values
//        currHealth = Mathf.Min(Mathf.Max(currHealth - health, 0), maxHealth);

//        // Sets last attacked ID to the attacker as long as it's not the player or an external object
//        if (attackerID != playerID && attackerID != -1)
//        {
//            lastAttackedID = attackerID;
//            lastAttackedType = weaponType;
//        }


//        // If the Player runs out of health, kill them
//        if (currHealth == 0)
//            RpcKill(attackerID, weaponType);
//    }


//    /// <summary>
//    /// Retrieves the player's current health
//    /// </summary>
//    /// <returns> currHealth float </returns>
//    public float GetCurrHealth()
//    {
//        return currHealth;
//    }



//    // ===========================================================
//    //                      DEATH AND REBIRTH
//    // ===========================================================

//    /// <summary>
//    /// Tells the server to kill this Player
//    /// </summary>
//    /// <param name="killerID"> ID of the Player that killed them </param>
//    /// <param name="weaponType"> Type of weapon that killed them </param>
//    public void CmdKill(int killerID, WeaponType weaponType)
//    {
//        RpcKill(killerID, weaponType);
//    }


//    /// <summary>
//    /// Kills this instance of Player on every client
//    /// </summary>
//    /// <param name="killerID"> ID of the Player that killed them </param>
//    /// /// <param name="weaponType"> Type of weapon that killed them </param>
//    public void RpcKill(int killerID, WeaponType weaponType)
//    {
//        // Prevent double kill
//        if (!dying)
//        {
//            dying = true;

//            // If life still exists, snuff it out
//            if (currHealth != 0)
//            {
//                currHealth = 0;
//                if (uiManager) uiManager.ui_Players[0].UpdateHealthBar(currHealth / maxHealth);
//            }

//            // Activate the GameManager's respawn process
//            GameManager.Instance().Respawn(gameObject);

//            // Updates KDR based on who killed; if a self-kill, gives kill to last attacker
//            if (killerID == -1 || killerID == playerID)
//                CmdTrackDeath(lastAttackedID, playerID, lastAttackedType);
//            else
//                CmdTrackDeath(killerID, playerID, weaponType);

//            // Deactivates the GameObject
//            gameObject.SetActive(false);
//        }
//    }


//    /// <summary>
//    /// Tells the server's GameManager to track the death
//    /// </summary>
//    /// <param name="killerID"> ID of the killer </param>
//    /// <param name="playerID"> ID of the killed </param>
//    /// /// <param name="weaponType"> Type of weapon that killed them </param>
//    void CmdTrackDeath(int killerID, int playerID, WeaponType weaponType)
//    {
//        GameManager.Instance().TrackDeath(killerID, playerID, weaponType);

//        // Resets the last attacker and type
//        lastAttackedID = playerID;
//        lastAttackedType = WeaponType.none;
//    }


//    /// <summary>
//    /// Respawns the player at the specified location
//    /// </summary>
//    /// <param name="spawnLocation"> Location to respawn the player at </param>
//    public void Respawn(Vector3 spawnLocation)
//    {
//        IncreaseHealth(maxHealth);
//        rb.velocity = Vector3.zero;
//        gameObject.transform.position = spawnLocation;

//        foreach (Arm_AI arm in arms)
//        {
//            arm.FullReload();
//        }

//        dying = false;
//    }



//    // ===========================================================
//    //                   START-UP AND CONNECTING
//    // ===========================================================

//    /// <summary>
//    /// Tells the server that the Player is connected
//    /// </summary>
//    void CmdPlayerConnected()
//    {
//        // Debug.LogError("CALLING AICONNECTED");
//        // Retrieves an ID from GameManager
//        int newID = GameManager.Instance().AIConnected();
//        // Updates all instances of this Player across clients
//        // Debug.LogError("ABOUT TO CALL RPCPLAYERCONNECTED");
//        RpcPlayerConnected(newID);
//    }


//    /// <summary>
//    /// Gets info from the server for this Player on every client
//    /// </summary>
//    /// <param name="playerID"> ID being given to this Player </param>
//    void RpcPlayerConnected(int playerID)
//    {
//        // Sets the Player's ID and the last attacked ID
//        // Debug.LogError("ID: " + this.playerID);
//        this.playerID = playerID;
//        // Debug.LogError("ID: " + this.playerID);
//        lastAttackedID = this.playerID;
//    }


///*    protected void Start()
//    {
//        // Tells the server that the Player is connected
//        Debug.LogError("STARTING AI");
//        CmdPlayerConnected();
//    }*/

//    // Activate the AI
//    public void Activate()
//    {
//        // Tells the server that the Player is connected
//        // Debug.LogError("STARTING AI");
//        CmdPlayerConnected();
//    }

//    protected void Awake()
//    {
//        // Start with full health
//        currHealth = maxHealth;

//        // Initiate variables
//        rb = gameObject.GetComponent<Rigidbody>();
//        arms = gameObject.GetComponentsInChildren<Arm_AI>();
//    }

//    void Update()
//    {
//        // Constantly update health bar (kinda nasty but works better than event-based)
//        if (uiManager) uiManager.ui_Players[0].UpdateHealthBar(currHealth / maxHealth);
//    }


//    /// <summary>
//    /// Tells the server to update the appearance of both Arms
//    /// </summary>
//    public void UpdateAppearance()
//    {
//        arms[0].CmdSwitchAppearance(arms[0].GetEquippedWeapon().mesh.name, arms[0].GetEquippedWeapon().material.name);
//        arms[1].CmdSwitchAppearance(arms[1].GetEquippedWeapon().mesh.name, arms[1].GetEquippedWeapon().material.name);
//    }


//    /// <summary>
//    /// Returns references to the Arm objects of the player
//    /// </summary>
//    /// <returns> Player's Arm objects </returns>
//    //public Arm_AI[] GetArms()
//    //{
//    //    return arms;
//    //}



//    // ===========================================================
//    //                           MOVEMENT
//    // ===========================================================

//    /// <summary>
//    /// Determines if the player is on the ground
//    /// </summary>
//    /// <returns> True if player is on ground, false if not </returns>
//    public bool IsGrounded()
//    {
//        float capsuleHeight = gameObject.GetComponent<CapsuleCollider>().bounds.extents.y;
//        return Physics.Raycast(transform.position, -Vector3.up, capsuleHeight + 0.05f);
//    }


//    /// <summary>
//    /// Adds the specified force to the player's RigidBody (used from local calls)
//    /// </summary>
//    /// <param name="force"> Force to add to the player </param>
//    public void EnactForce(Vector3 force)
//    {
//        rb.AddForce(force);
//    }


//    /// <summary>
//    /// Enacts force on this player on every client (used from server calls)
//    /// </summary>
//    /// <param name="force"> Amount of force to enact </param>
//    public void RpcEnactForce(Vector3 force)
//    {
//        rb.AddForce(force);
//    }


//}
