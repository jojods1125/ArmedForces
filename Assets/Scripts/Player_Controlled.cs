using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controlled : Player
{
    private float currWalkMomentum = 0;
    private bool movingL = false;
    private bool movingR = false;


    // ===========================================================
    //                  INPUTS AND INSTANTIATION
    // ===========================================================

    /// <summary>
    /// Sets variables that, on FixedUpdate, will make player movement reflect left-moving input
    /// </summary>
    /// <param name="pressed"> Whether the player is pressing the movement button or not </param>
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlinePlayer.isLocalPlayer))
            return;

        if (context.performed)
        {
            currWalkMomentum += -walkMomentum;
            movingL = true;
            FaceLeft();
        }
        else if (context.canceled)
        {
            currWalkMomentum += walkMomentum;
            movingL = false;
        }
    }

    /// <summary>
    /// Sets variables that, on FixedUpdate, will make player movement reflect right-moving input
    /// </summary>
    /// <param name="pressed"> Whether the player is pressing the movement button or not </param>
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlinePlayer.isLocalPlayer))
            return;

        if (context.performed)
        {
            currWalkMomentum += walkMomentum;
            movingR = true;
            FaceRight();
        }
        else if (context.canceled)
        {
            currWalkMomentum += -walkMomentum;
            movingR = false;
        }
    }


    //new void Start()
    //{
        

    //    base.Start();
    //}


    // Called every fixed tic
    void FixedUpdate()
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlinePlayer.isLocalPlayer))
            return;

        if (rb.velocity.magnitude < maxWalkSpeed)
            rb.AddForce(currWalkMomentum, 0, 0);

        if (movingL && movingR)
            if (IsGrounded())
                rb.AddForce(currWalkMomentum * 2, jumpForce, 0);
    }


}
