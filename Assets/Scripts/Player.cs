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
    private float currHealth = 100;
    private bool dying = false;



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
    }


    /// <summary>
    /// Decreases the player's health by health and kills if at 0
    /// </summary>
    /// <param name="health"> Amount to decrease health by </param>
    public void DecreaseHealth(float health)
    {
        currHealth = Mathf.Max(currHealth - health, 0);
        
        if (currHealth == 0)
            Kill();
    }


    /// <summary>
    /// Instantly sets the player's health to 0 and kills them
    /// </summary>
    public void Kill()
    {
        if (!dying)
        {
            dying = true;
            if (currHealth != 0) currHealth = 0;

            GameManager.Instance().Respawn(gameObject);

            gameObject.SetActive(false);
        }
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


    void Awake()
    {
        currHealth = maxHealth;

        rb = gameObject.GetComponent<Rigidbody>();
        arms = gameObject.GetComponentsInChildren<Arm>();
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
    public void EnactForce(Vector3 force)
    {
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


}
