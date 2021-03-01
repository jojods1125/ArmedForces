using UnityEngine;
using System.Collections;

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
    public int jumpForce = 50;

    [Header("Weapon Loadouts")]
    public Weapon[] backArmWeapons = new Weapon[4];
    public Weapon[] frontArmWeapons = new Weapon[4];


    private PlayerControls inputActions;
    private Rigidbody rb;
    private Arm[] arms;
    private float currWalkMomentum = 0;
    private bool movingL = false;
    private bool movingR = false;
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
    //                  INPUTS AND INSTANTIATION
    // ===========================================================

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;

        inputActions = new PlayerControls();
        rb = gameObject.GetComponent<Rigidbody>();
        arms = gameObject.GetComponentsInChildren<Arm>();

        inputActions.GunGuy.MoveLeft.performed += ctx => MoveLeft(true);
        inputActions.GunGuy.MoveLeft.canceled += ctx => MoveLeft(false);
        inputActions.GunGuy.MoveLeft.Enable();

        inputActions.GunGuy.MoveRight.performed += ctx => MoveRight(true);
        inputActions.GunGuy.MoveRight.canceled += ctx => MoveRight(false);
        inputActions.GunGuy.MoveRight.Enable();
    }


    /// <summary>
    /// Sets variables that, on FixedUpdate, will make player movement reflect left-moving input
    /// </summary>
    /// <param name="pressed"> Whether the player is pressing the movement button or not </param>
    void MoveLeft(bool pressed)
    {
        if (pressed)
        {
            currWalkMomentum += -walkMomentum;
            movingL = true;
        }
        else
        {
            currWalkMomentum += walkMomentum;
            movingL = false;
        }
    }


    /// <summary>
    /// Sets variables that, on FixedUpdate, will make player movement reflect right-moving input
    /// </summary>
    /// <param name="pressed"> Whether the player is pressing the movement button or not </param>
    void MoveRight(bool pressed)
    {
        if (pressed)
        {
            currWalkMomentum += walkMomentum;
            movingR = true;
        }
        else
        {
            currWalkMomentum += -walkMomentum;
            movingR = false;
        }
    }


    // Called every fixed tic
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxWalkSpeed)
            rb.AddForce(currWalkMomentum, 0, 0);

        if (movingL && movingR)
            if (IsGrounded())
                rb.AddForce(currWalkMomentum * 2, jumpForce, 0);
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


    


}
