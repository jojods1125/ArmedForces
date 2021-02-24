using System.Collections.Generic;
using UnityEngine;

public enum ArmType
{
    Front,
    Back
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Arm : MonoBehaviour
{
    public GameObject barrel;
    public Player player;
    public PlayerControls.GunGuyActions gunActions;
    public ArmType armType;


    private PlayerControls inputActions;

    private bool firing = false;
    private float fireRateTimeStamp = 0;
    private bool semiFired = false;
    private bool launcherFired = false;

    private Weapon equippedWeapon;
    private Weapon weaponA;
    private Weapon weaponB;
    private Weapon weaponC;
    private Weapon weaponD;

    private readonly Dictionary<Weapon, int> ammoRemaining = new Dictionary<Weapon, int>();


    /// <summary>
    /// Fires an AutoGun Weapon
    /// </summary>
    /// <param name="auto">Equipped W_AutoGun</param>
    void FireAuto(W_AutoGun auto)
    {
        // Fire rate and ammo check
        if (Time.time > fireRateTimeStamp && ammoRemaining[auto] > 0)
        {
            // Updates fire rate and ammo
            fireRateTimeStamp = Time.time + auto.fireRate;
            ReduceAmmo(1);

            // Calculates bullet path and draws ray
            Vector3 bulletPath = barrel.transform.up + new Vector3(Random.Range(-auto.spreadRange, auto.spreadRange), Random.Range(-auto.spreadRange, auto.spreadRange));
            Debug.DrawRay(barrel.transform.position, bulletPath * 1000f, Color.green, 1);

            // Raycasts bullet path
            if (Physics.Raycast(barrel.transform.position, bulletPath, out RaycastHit hit))
            {
                ///Debug.Log("HIT");
            }

            // Pushes player
            player.EnactForce(bulletPath.normalized * -auto.pushback);
        }
    }

    /// <summary>
    /// Fires a SemiGun Weapon
    /// </summary>
    /// <param name="semi">Equipped W_SemiGun</param>
    void FireSemi(W_SemiGun semi)
    {
        // Trigger and fire rate check
        if (!semiFired && Time.time > fireRateTimeStamp)
        {
            // Burst loop
            for (int i = 0; i < semi.burstCount; i++)
            {
                // Ammo check
                if (ammoRemaining[semi] > 0)
                {
                    // Updates trigger, fire rate, and ammo
                    semiFired = true;
                    fireRateTimeStamp = Time.time + semi.fireRate;
                    ReduceAmmo(1);

                    // Calculates bullet path and draws ray
                    Vector3 bulletPath = barrel.transform.up + new Vector3(Random.Range(-semi.spreadRange, semi.spreadRange), Random.Range(-semi.spreadRange, semi.spreadRange));
                    Debug.DrawRay(barrel.transform.position, bulletPath * 1000f, Color.red, 1);

                    // Raycasts bullet path
                    if (Physics.Raycast(barrel.transform.position, bulletPath, out RaycastHit hit))
                    {
                        ///Debug.Log("HIT");
                    }

                    // Pushes player
                    player.EnactForce(bulletPath.normalized * -semi.pushback);
                }
            }
        }

    }

    /// <summary>
    /// Fires a Launcher Weapon
    /// </summary>
    /// <param name="launcher">Equipped W_Launcher</param>
    void FireLauncher(W_Launcher launcher)
    {
        // Trigger and fire rate check
        if (!launcherFired && Time.time > fireRateTimeStamp)
        {
            // Ammo check
            if (ammoRemaining[launcher] > 0)
            {
                // Updates trigger, fire rate, and ammo
                launcherFired = true;
                fireRateTimeStamp = Time.time + launcher.fireRate;
                ReduceAmmo(1);

                // Creates projectile and spawns it at the correct location
                Vector3 projectilePath = new Vector3(barrel.transform.position.x, barrel.transform.position.y);
                GameObject projectile = Instantiate(launcher.projectilePrefab, projectilePath + (barrel.transform.up * 0.5f), Quaternion.identity);

                // Initializes the projectile prefab with the appropriate launcher values
                projectile.GetComponent<Projectile>().Initialize(barrel.transform.up, launcher.projectilePower, launcher.explosionRadius,
                                        launcher.coreDamage, launcher.corePushback, launcher.rocketPowered);

                // Pushes player
                player.EnactForce(barrel.transform.up.normalized * -launcher.pushback);
            }
        }

    }

    /// <summary>
    /// Reduces the ammo of the equipped weapon by a specified amount
    /// </summary>
    /// <param name="count"> Amount to reduce ammo by </param>
    void ReduceAmmo(int count)
    {
        if (equippedWeapon.Equals(weaponA))
            ammoRemaining[weaponA] = Mathf.Max(ammoRemaining[weaponA] - count, 0);

        else if (equippedWeapon.Equals(weaponB))
            ammoRemaining[weaponB] = Mathf.Max(ammoRemaining[weaponB] - count, 0);

        else if (equippedWeapon.Equals(weaponC))
            ammoRemaining[weaponC] = Mathf.Max(ammoRemaining[weaponC] - count, 0);

        else if (equippedWeapon.Equals(weaponD))
            ammoRemaining[weaponD] = Mathf.Max(ammoRemaining[weaponD] - count, 0);
    }

    /// <summary>
    /// Increases the ammo of the equipped weapon by a specified amount
    /// </summary>
    /// <param name="count"> Amount to increase ammo by </param>
    void RegainAmmo(int count)
    {
        if (equippedWeapon is W_Shootable weapon)
        {
            if (equippedWeapon.Equals(weaponA))
                ammoRemaining[weaponA] = Mathf.Min(ammoRemaining[weaponA] + count, weapon.ammoCapacity);

            else if (equippedWeapon.Equals(weaponB))
                ammoRemaining[weaponB] = Mathf.Min(ammoRemaining[weaponB] + count, weapon.ammoCapacity);

            else if (equippedWeapon.Equals(weaponC))
                ammoRemaining[weaponC] = Mathf.Min(ammoRemaining[weaponC] + count, weapon.ammoCapacity);

            else if (equippedWeapon.Equals(weaponD))
                ammoRemaining[weaponD] = Mathf.Min(ammoRemaining[weaponD] + count, weapon.ammoCapacity);
        }
    }

    void FixedUpdate()
    {
        if (firing)
        {
            if (equippedWeapon is W_SemiGun semi)
                FireSemi(semi);

            else if (equippedWeapon is W_AutoGun auto)
                FireAuto(auto);

            else if (equippedWeapon is W_Launcher launcher)
                FireLauncher(launcher);

            /// INCLUDE MORE WEAPONS HERE
        }
        else
        {
            if (semiFired) semiFired = false;
            if (launcherFired) launcherFired = false;

            /// INCLUDE MORE SINGLE-PRESS WEAPONS HERE

            /// TODO: Make RegainAmmo() do 1 ammo at a time when UI is implemented
            if (player.IsGrounded() && equippedWeapon is W_Shootable weapon) RegainAmmo(weapon.ammoCapacity);
        }
    }



    // ===========================================================
    //              IGNORE EVERYTHING PAST THIS POINT
    // ===========================================================


    // Start is called before the first frame update
    void Start()
    {
        // Set up input actions
        inputActions = new PlayerControls();
        if (armType == ArmType.Front) FrontArmInitialize();
        else BackArmInitialize();

        // Initialize equippedWeapon
        if (weaponA) equippedWeapon = weaponA;
        else if (weaponB) equippedWeapon = weaponA;
        else if (weaponC) equippedWeapon = weaponC;
        else equippedWeapon = weaponD;

        // Render equippedWeapon
        if (equippedWeapon)
        {
            gameObject.GetComponent<MeshFilter>().mesh = equippedWeapon.mesh;
            gameObject.GetComponent<MeshRenderer>().material = equippedWeapon.material;
        }

        // Initializes ammoRemaining dictionary for each weapon
        if (weaponA is W_Shootable gunA) ammoRemaining.Add(weaponA, gunA.ammoCapacity);
        if (weaponB is W_Shootable gunB) ammoRemaining.Add(weaponB, gunB.ammoCapacity);
        if (weaponC is W_Shootable gunC) ammoRemaining.Add(weaponC, gunC.ammoCapacity);
        if (weaponD is W_Shootable gunD) ammoRemaining.Add(weaponD, gunD.ammoCapacity);
    }


    // Aims the arm based on joystick angle
    void Aim(Vector3 aimVal)
    {
        float degrees = Vector3.Angle(Vector3.up, transform.position + (aimVal * 1000));
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        if (aimVal.x > 0)
            degrees = -degrees;
        if (aimVal.sqrMagnitude > 0.5)
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, degrees);
    }


    // Switches the arm's currently equipped weapon
    void Switch(Weapon weapon)
    {
        if (weapon)
        {
            this.equippedWeapon = weapon;
            gameObject.GetComponent<MeshFilter>().mesh = weapon.mesh;
            gameObject.GetComponent<MeshRenderer>().material = weapon.material;
        }
    }


    // Initializes the controls of the arm based on it being the front arm
    void FrontArmInitialize()
    {
        weaponA = player.frontArmWeapons[0];
        weaponB = player.frontArmWeapons[1];
        weaponC = player.frontArmWeapons[2];
        weaponD = player.frontArmWeapons[3];

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
    void BackArmInitialize()
    {
        weaponA = player.backArmWeapons[0];
        weaponB = player.backArmWeapons[1];
        weaponC = player.backArmWeapons[2];
        weaponD = player.backArmWeapons[3];

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
