using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    // ===========================================================
    //                          VARIABLES
    // ===========================================================

    [Header("Player Data")]

    public MatchType matchType;
    [Min(0)]
    public float maxHealth = 100;
    public float walkMomentum = 10;
    public float maxWalkSpeed = 5;
    [Min(0)]
    public int jumpForce = 100;


    [Header("Weapon Loadouts")]

    public Weapon[] backArmWeapons = new Weapon[4];
    public Weapon[] frontArmWeapons = new Weapon[4];

    [HideInInspector]
    public Rigidbody rb;
    private Arm[] arms;
    private float currHealth = 100;
    private bool dying = false;

    //Used for differentiating each player in GameManager
    [SerializeField]
    private int playerID;

    // Last player ID to have attacked, reset on death
    private int lastAttackedID;

    // Last weapon to have attacked, reset to none on death
    private WeaponType lastAttackedType;

    // UI Manager
    protected UIManager uiManager;

    public Player_Networked onlinePlayer;


    // ===========================================================
    //                            DAMAGE
    // ===========================================================

    /// <summary>
    /// Increases the player's health by health
    /// </summary>
    /// <param name="health"> Amount to increase health by </param>
    public void IncreaseHealth(float health)
    {
        // Increases health and prevents invalid values
        currHealth = Mathf.Max(Mathf.Min(currHealth + health, maxHealth), 0);

        if (uiManager) uiManager.ui_Players[playerID].UpdateHealthBar(currHealth / maxHealth);

        // If you die by healing for some ungodly reason, you played yourself
        if (currHealth <= 0 && matchType != MatchType.Online)
            Kill(playerID, WeaponType.none);
    }


    /// <summary>
    /// Decreases the player's health by health and kills if at 0 (must call from server)
    /// </summary>
    /// <param name="health"> Amount to decrease health by </param>
    /// <param name="attackerID"> ID of the attacker </param>
    /// <param name="weaponType"> Type of weapon that attacked </param>
    public void DecreaseHealth(float health, int attackerID, WeaponType weaponType)
    {
        // Decreases health and prevents invalid values
        currHealth = Mathf.Min(Mathf.Max(currHealth - health, 0), maxHealth);

        // Sets last attacked ID to the attacker as long as it's not the player or an external object
        if (attackerID != playerID && attackerID != -1)
        {
            lastAttackedID = attackerID;
            lastAttackedType = weaponType;
        }

        if (uiManager) uiManager.ui_Players[playerID].UpdateHealthBar(currHealth / maxHealth);

        // If the Player runs out of health, kill them
        if (currHealth <= 0 && matchType != MatchType.Online)
            Kill(attackerID, weaponType);
    }


    public void SetCurrHealth(float newHealth)
    {
        currHealth = newHealth;
    }

    /// <summary>
    /// Retrieves the player's current health
    /// </summary>
    /// <returns> currHealth float </returns>
    public float GetCurrHealth()
    {
        return currHealth;
    }



    // ===========================================================
    //                      DEATH AND REBIRTH
    // ===========================================================


    /// <summary>
    /// Kills this instance of Player on every client
    /// </summary>
    /// <param name="killerID"> ID of the Player that killed them </param>
    /// <param name="weaponType"> Type of weapon that killed them </param>
    public bool Kill(int killerID, WeaponType weaponType)
    {
        // Prevent double kill
        if (!dying)
        {
            dying = true;

            // If life still exists, snuff it out
            if (currHealth != 0)
            {
                currHealth = 0;
                if (uiManager) uiManager.ui_Players[playerID].UpdateHealthBar(currHealth / maxHealth);
            }

            // Activate the GameManager's respawn process
            GameManager.Instance().Respawn(gameObject);

            // If the player isn't online, track the death locally
            if (matchType != MatchType.Online)
            {
                // Updates KDR based on who killed; if a self-kill, gives kill to last attacker
                if (killerID == -1 || killerID == playerID)
                    TrackDeath(lastAttackedID, playerID, lastAttackedType);
                else
                    TrackDeath(killerID, playerID, weaponType);
            }
            
            // Deactivates the GameObject
            gameObject.SetActive(false);

            return true;
        }
        return false;
    }


    /// <summary>
    /// Tells the server's GameManager to track the death
    /// </summary>
    /// <param name="killerID"> ID of the killer </param>
    /// <param name="playerID"> ID of the killed </param>
    /// /// <param name="weaponType"> Type of weapon that killed them </param>
    void TrackDeath(int killerID, int playerID, WeaponType weaponType)
    {
        GameManager.Instance().TrackDeath(killerID, playerID, weaponType);

        // Resets the last attacker and type
        lastAttackedID = playerID;
        lastAttackedType = WeaponType.none;
    }


    /// <summary>
    /// Respawns the player at the specified location
    /// </summary>
    /// <param name="spawnLocation"> Location to respawn the player at </param>
    public void Respawn(Vector3 spawnLocation)
    {
        IncreaseHealth(maxHealth);
        rb.velocity = Vector3.zero;
        transform.position = spawnLocation;

        foreach (Arm arm in arms)
        {
            arm.FullReload();
        }

        dying = false;
    }


    // ===========================================================
    //                   START-UP AND CONNECTING
    // ===========================================================

    /// <summary>
    /// Tells the server that the Player is connected
    /// </summary>
    public void PlayerConnected()
    {
        Debug.Log("Player Connected executed");

        // Retrieves an ID from GameManager
        int newID = GameManager.Instance().ClientConnected();
        GameManager.Instance().localPlayers[newID] = this;

        this.playerID = newID;
        lastAttackedID = this.playerID;
    }


    protected void Start()
    {
        // If not an online player
        if (matchType != MatchType.Online)
        {
            // Tells the server that the Player is connected
            PlayerConnected();

            uiManager.ui_Players[playerID].UpdateWeaponIcons(playerID);
            // If UI exists (only local player), connect health bar and weapon UI
            //if (uiManager && matchType != MatchType.Local)
            //{
            //    GameManager.Instance().localPlayer = this;
            //    uiManager.ui_Players[0].UpdateHealthBar(currHealth / maxHealth);
            //    uiManager.ui_Players[0].UpdateWeaponIcons();
            //}
        }
    }

    public void Activate()
    {
        // Tells the server that the Player is connected
        // Debug.LogError("STARTING AI");
        PlayerConnected();
    }

    protected void Awake()
    {
        // Start with full health
        currHealth = maxHealth;

        // Initiate variables
        rb = gameObject.GetComponent<Rigidbody>();
        arms = gameObject.GetComponentsInChildren<Arm>();
    }

    void Update()
    {
        //if (matchType != MatchType.Online)
            // Constantly update health bar (kinda nasty but works better than event-based)
            //if (uiManager) uiManager.ui_Players[playerID].UpdateHealthBar(currHealth / maxHealth);
    }


    /// <summary>
    /// Tells the server to update the appearance of both Arms
    /// </summary>
    public void UpdateAppearance()
    {
        if (matchType == MatchType.Online)
        {
            arms[0].onlineArm.CmdSwitchAppearance(arms[0].GetEquippedWeapon().mesh.name, arms[0].GetEquippedWeapon().material.name);
            arms[1].onlineArm.CmdSwitchAppearance(arms[1].GetEquippedWeapon().mesh.name, arms[1].GetEquippedWeapon().material.name);
        }
        else
        {
            if (arms != null)
            {
                arms[0].SwitchAppearance(arms[0].GetEquippedWeapon().mesh.name, arms[0].GetEquippedWeapon().material.name);
                arms[1].SwitchAppearance(arms[1].GetEquippedWeapon().mesh.name, arms[1].GetEquippedWeapon().material.name);
            }
        }
    }


    /// <summary>
    /// Returns references to the Arm objects of the player
    /// </summary>
    /// <returns> Player's Arm objects </returns>
    public Arm[] GetArms()
    {
        return arms;
    }


    public void SetPlayerID(int newID)
    {
        playerID = newID;
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public void SetLastAttackedID(int newID)
    {
        lastAttackedID = newID;
    }

    public int GetLastAttackedID()
    {
        return lastAttackedID;
    }

    public void SetLastAttackedType(WeaponType type)
    {
        lastAttackedType = type;
    }

    public WeaponType GetLastAttackedType()
    {
        return lastAttackedType;
    }

    public UIManager GetUIManager()
    {
        return uiManager;
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
        float capsuleHeight = gameObject.GetComponent<CapsuleCollider>().bounds.extents.y;
        return Physics.Raycast(gameObject.transform.position, -Vector3.up, capsuleHeight + 0.05f);
    }


    /// <summary>
    /// Adds the specified force to the player's RigidBody (used from local calls)
    /// </summary>
    /// <param name="force"> Force to add to the player </param>
    public void EnactForce(Vector3 force)
    {
        rb.AddForce(force);
    }


    public void FreezeConstraints()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }


}
