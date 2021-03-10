using UnityEngine;
using System.Collections;
using System;
using Mirror;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : NetworkBehaviour
{
    // ===========================================================
    //                          VARIABLES
    // ===========================================================

    [Header("Player Data")]
    [Min(0)]
    public float maxHealth = 100;
    public float walkMomentum = 10;
    public float maxWalkSpeed = 5;
    [Min(0)]
    public int jumpForce = 100;

    [Header("Weapon Loadouts")]
    public Weapon[] backArmWeapons = new Weapon[4];
    public Weapon[] frontArmWeapons = new Weapon[4];

    protected Rigidbody rb;
    private Arm[] arms;
    [SyncVar]
    private float currHealth = 100;
    private bool dying = false;

    //Used for differentiating each player in Game Manager
    public int playerID;

    // UI Manager
    protected UIManager uiManager;



    // ===========================================================
    //                       HEALTH AND DEATH
    // ===========================================================


    /// <summary>
    /// Increases the player's health by health
    /// </summary>
    /// <param name="health"> Amount to increase health by </param>
    public void IncreaseHealth(float health)
    {
        currHealth = Mathf.Min(currHealth + health, maxHealth);
        if (uiManager) uiManager.UpdateHealthBar(currHealth / maxHealth);
    }


    /// <summary>
    /// Decreases the player's health by health and kills if at 0
    /// </summary>
    /// <param name="health"> Amount to decrease health by </param>
    public void DecreaseHealth(float health, int attackerID)
    {
        if (!isServer)
            return;

        currHealth = Mathf.Max(currHealth - health, 0);

        if (currHealth == 0)
            RpcKill(attackerID);
    }


    /// <summary>
    /// Instantly sets the player's health to 0 and kills them
    /// </summary>
    [ClientRpc]
    public void RpcKill(int killerID)
    {

        if (!dying)
        {
            dying = true;
            if (currHealth != 0)
            {
                currHealth = 0;
                if (uiManager) uiManager.UpdateHealthBar(currHealth / maxHealth);
            }

            GameManager.Instance().Respawn(gameObject);
            GameManager.Instance().trackDeath(killerID, playerID);

            gameObject.SetActive(false);
        }

        // if Player_Controlled, call achievement event
        if (this is Player_Controlled)
            AchievementManager.Instance().OnEvent(AchievementType.deaths);
    }


    /// <summary>
    /// Respawns the player at the specified location
    /// </summary>
    /// <param name="spawnLocation"> Location to respawn the player at </param>
    public void Respawn(Vector3 spawnLocation)
    {
        IncreaseHealth(maxHealth);
        rb.velocity = Vector3.zero;
        gameObject.transform.position = spawnLocation;

        foreach (Arm arm in arms)
        {
            arm.FullReload();
        }

        dying = false;
    }



    // ===========================================================
    //                       OTHER FUNCTIONS
    // ===========================================================


    // Will be overridden by Player_Controlled's start, so include anything in here in there
    protected void Start()
    {
        playerID = GameManager.Instance().getID();

        if (!isLocalPlayer)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (uiManager)
        {
            GameManager.Instance().mainPlayer = this;
            uiManager.UpdateHealthBar(currHealth / maxHealth);
        }
    }

    protected void Awake()
    {
        currHealth = maxHealth;

        rb = gameObject.GetComponent<Rigidbody>();
        arms = gameObject.GetComponentsInChildren<Arm>();
    }

    void Update()
    {
        if (uiManager) uiManager.UpdateHealthBar(currHealth / maxHealth);
    }


    /// <summary>
    /// Determines if the player is on the ground
    /// </summary>
    /// <returns> True if player is on ground, false if not </returns>
    public bool IsGrounded()
    {
        float capsuleHeight = gameObject.GetComponent<CapsuleCollider>().bounds.extents.y;
        return Physics.Raycast(transform.position, -Vector3.up, capsuleHeight + 0.05f);
    }


    /// <summary>
    /// Adds the specified force to the player's RigidBody
    /// </summary>
    /// <param name="force"> Force to add to the player </param>
    //[ClientRpc]
    public void EnactForce(Vector3 force)
    {
        //if (!isLocalPlayer)
        //    return;

        rb.AddForce(force);
    }


    [ClientRpc]
    public void RpcEnactForce(Vector3 force)
    {
        if (!isLocalPlayer)
            return;

        rb.AddForce(force);
    }

    /// <summary>
    /// Returns references to the Arm objects of the player
    /// </summary>
    /// <returns> Player's Arm objects </returns>
    public Arm[] GetArms()
    {
        return arms;
    }


    /// <summary>
    /// Retrieves the player's current health
    /// </summary>
    /// <returns> currHealth float </returns>
    public float GetCurrHealth()
    {
        return currHealth;
    }

}
