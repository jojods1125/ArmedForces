using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Arm_Controlled : Arm
{

    public void OnArmAim(InputAction.CallbackContext context)
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlineArm.isLocalPlayer))
            return;

        Aim(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlineArm.isLocalPlayer))
            return;

        if (context.performed)
            firing = true;
        else if (context.canceled)
            firing = false;
    }

    public void OnArm1(InputAction.CallbackContext context)
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlineArm.isLocalPlayer))
            return;

        if (matchType != MatchType.Online)
            Switch(weaponA);
        else
            onlineArm.Switch(weaponA);
    }

    public void OnArm2(InputAction.CallbackContext context)
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlineArm.isLocalPlayer))
            return;

        if (matchType != MatchType.Online)
            Switch(weaponB);
        else
            onlineArm.Switch(weaponB);
    }

    public void OnArm3(InputAction.CallbackContext context)
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlineArm.isLocalPlayer))
            return;

        if (matchType != MatchType.Online)
            Switch(weaponC);
        else
            onlineArm.Switch(weaponC);
    }

    public void OnArm4(InputAction.CallbackContext context)
    {
        if (!Application.isFocused || (matchType == MatchType.Online && !onlineArm.isLocalPlayer))
            return;

        if (matchType != MatchType.Online)
            Switch(weaponD);
        else
            onlineArm.Switch(weaponD);
    }



    new void Start()
    {
        if ((matchType == MatchType.Online && onlineArm.isLocalPlayer) || matchType != MatchType.Online)
        {
            // Retrieve UIManager
            uiManager = GameManager.Instance().uiManager;
        }

        base.Start();
    }

}
