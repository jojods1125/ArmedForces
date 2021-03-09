using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm_Controlled : Arm
{
    /// <summary> Player input controls </summary>
    private PlayerControls inputActions;


    private void Awake()
    {
        
    }

    new void Start()
    {
        if (isLocalPlayer)
        {
            inputActions = new PlayerControls();

            // Set up arm input controls
            if (armType == ArmType.Front) FrontArmInitializeInputs();
            else BackArmInitializeInputs();

            // Retrieve UIManager
            uiManager = GameManager.Instance().uiManager;
        }

        base.Start();
    }


    // Initializes the controls of the arm based on it being the front arm
    void FrontArmInitializeInputs()
    {
        inputActions.GunGuy.ArmAimFront.performed += ctx => Aim(ctx.ReadValue<Vector2>());
        inputActions.GunGuy.ArmAimFront.Enable();

        inputActions.GunGuy.ShootFront.performed += ctx => firing = true;
        inputActions.GunGuy.ShootFront.canceled += ctx => firing = false;
        inputActions.GunGuy.ShootFront.Enable();

        inputActions.GunGuy.FrontArm1.performed += ctx => Switch(weaponA);
        inputActions.GunGuy.FrontArm1.Enable();

        inputActions.GunGuy.FrontArm2.performed += ctx => Switch(weaponB);
        inputActions.GunGuy.FrontArm2.Enable();

        inputActions.GunGuy.FrontArm3.performed += ctx => Switch(weaponC);
        inputActions.GunGuy.FrontArm3.Enable();

        inputActions.GunGuy.FrontArm4.performed += ctx => Switch(weaponD);
        inputActions.GunGuy.FrontArm4.Enable();
    }


    // Initializes the controls of the arm based on it being the back arm
    void BackArmInitializeInputs()
    {
        inputActions.GunGuy.ArmAimBack.performed += ctx => Aim(ctx.ReadValue<Vector2>());
        inputActions.GunGuy.ArmAimBack.Enable();

        inputActions.GunGuy.ShootBack.performed += ctx => firing = true;
        inputActions.GunGuy.ShootBack.canceled += ctx => firing = false;
        inputActions.GunGuy.ShootBack.Enable();

        inputActions.GunGuy.BackArm1.performed += ctx => Switch(weaponA);
        inputActions.GunGuy.BackArm1.Enable();

        inputActions.GunGuy.BackArm2.performed += ctx => Switch(weaponB);
        inputActions.GunGuy.BackArm2.Enable();

        inputActions.GunGuy.BackArm3.performed += ctx => Switch(weaponC);
        inputActions.GunGuy.BackArm3.Enable();

        inputActions.GunGuy.BackArm4.performed += ctx => Switch(weaponD);
        inputActions.GunGuy.BackArm4.Enable();
    }

}
