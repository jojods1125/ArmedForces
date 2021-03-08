using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controlled : Player
{
    private PlayerControls inputActions;
    private float currWalkMomentum = 0;
    private bool movingL = false;
    private bool movingR = false;


    // ===========================================================
    //                  INPUTS AND INSTANTIATION
    // ===========================================================

    // Start is called before the first frame update
    void Start()
    {
        playerID = GameManager.Instance().getID();    // Shouldn't need a new playerID here because Player_Controlled is a Player
        // What is my ID
        // Debug.Log("Player_Controlled: " + playerID);
        inputActions = new PlayerControls();

        inputActions.GunGuy.MoveLeft.performed += ctx => MoveLeft(true);
        inputActions.GunGuy.MoveLeft.canceled += ctx => MoveLeft(false);
        inputActions.GunGuy.MoveLeft.Enable();

        inputActions.GunGuy.MoveRight.performed += ctx => MoveRight(true);
        inputActions.GunGuy.MoveRight.canceled += ctx => MoveRight(false);
        inputActions.GunGuy.MoveRight.Enable();

        uiManager = GameManager.Instance().uiManager;
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


}
