using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Which side of the player the arm is on; determines weapon loadout
/// </summary>
public enum ArmType
{
    /// <summary> Closer to camera </summary>
    Front,
    /// <summary> Behind player </summary>
    Back
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Arm : MonoBehaviour
{
    [Tooltip("Tip of the arm, where shots emit from")]
    public GameObject barrel;
    [Tooltip("Player character reference")]
    public Player player;
    [Tooltip("Whether the arm uses the player's Front Arm or Back Arm weapons")]
    public ArmType armType;


    /// <summary> Whether or not the player is pressing the fire trigger </summary>
    protected bool firing = false;
    /// <summary> Previous time the player fired </summary>
    private float fireRateTimeStamp = 0;
    /// <summary> Checks if a weapon that requires a trigger release has been released </summary>
    private bool singleShotFired = false;


    /// <summary> Currently equipped weapon </summary>
    protected Weapon equippedWeapon;
    /// <summary> Weapon attached to top face button, weapons[0] </summary>
    protected Weapon weaponA;
    /// <summary> Weapon attached to right face button, weapons[1] </summary>
    protected Weapon weaponB;
    /// <summary> Weapon attached to bottom face button, weapons[2] </summary>
    protected Weapon weaponC;
    /// <summary> Weapon attached to left face button, weapons[3] </summary>
    protected Weapon weaponD;


    /// <summary> Amount of ammo remaining in each weapon </summary>
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
                ///Debug.Log("HIT " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    hit.collider.gameObject.GetComponent<Player>().EnactForce(bulletPath.normalized * auto.bulletPushback);
                    hit.collider.gameObject.GetComponent<Player>().DecreaseHealth(auto.bulletDamage);
                }
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
        if (!singleShotFired && Time.time > fireRateTimeStamp)
        {
            // Burst loop
            for (int i = 0; i < semi.burstCount; i++)
            {
                // Ammo check
                if (ammoRemaining[semi] > 0)
                {
                    // Updates trigger, fire rate, and ammo
                    singleShotFired = true;
                    fireRateTimeStamp = Time.time + semi.fireRate;
                    ReduceAmmo(1);

                    // Calculates bullet path and draws ray
                    Vector3 bulletPath = barrel.transform.up + new Vector3(Random.Range(-semi.spreadRange, semi.spreadRange), Random.Range(-semi.spreadRange, semi.spreadRange));
                    Debug.DrawRay(barrel.transform.position, bulletPath * 1000f, Color.red, 1);

                    // Raycasts bullet path
                    if (Physics.Raycast(barrel.transform.position, bulletPath, out RaycastHit hit))
                    {
                        ///Debug.Log("HIT " + hit.collider.gameObject.name);
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            hit.collider.gameObject.GetComponent<Player>().EnactForce(bulletPath.normalized * semi.bulletPushback);
                            hit.collider.gameObject.GetComponent<Player>().DecreaseHealth(semi.bulletDamage);
                        }
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
        if (!singleShotFired && Time.time > fireRateTimeStamp)
        {
            // Ammo check
            if (ammoRemaining[launcher] > 0)
            {
                // Updates trigger, fire rate, and ammo
                singleShotFired = true;
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
    /// Fires an Sprayer Weapon
    /// </summary>
    /// <param name="sprayer">Equipped W_AutoGun</param>
    void FireSprayer(W_Sprayer sprayer)
    {
        // Fire rate and ammo check
        if (Time.time > fireRateTimeStamp && ammoRemaining[sprayer] > 0)
        {
            // Updates fire rate and ammo
            fireRateTimeStamp = Time.time + sprayer.fireRate;
            ReduceAmmo(1);

            // Calculates bullet path and draws ray
            Vector3 bulletPath = barrel.transform.up + new Vector3(Random.Range(-sprayer.spreadRange, sprayer.spreadRange), Random.Range(-sprayer.spreadRange, sprayer.spreadRange));
            Debug.DrawRay(barrel.transform.position, bulletPath * sprayer.sprayDistance, Color.yellow, 1);

            // Raycasts bullet path
            if (Physics.Raycast(barrel.transform.position, bulletPath, out RaycastHit hit, sprayer.sprayDistance))
            {
                ///Debug.Log("HIT " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    hit.collider.gameObject.GetComponent<Player>().EnactForce(bulletPath.normalized * sprayer.bulletPushback);
                    hit.collider.gameObject.GetComponent<Player>().DecreaseHealth(sprayer.bulletDamage);
                }
            }

            // Pushes player
            player.EnactForce(bulletPath.normalized * -sprayer.pushback);
        }
    }



    void FixedUpdate()
    {
        // If the player is using the firing input, call the appropriate weapon firing function
        if (firing)
        {
            if (equippedWeapon is W_SemiGun semi)
                FireSemi(semi);

            else if (equippedWeapon is W_AutoGun auto)
                FireAuto(auto);

            else if (equippedWeapon is W_Launcher launcher)
                FireLauncher(launcher);

            else if (equippedWeapon is W_Sprayer sprayer)
                FireSprayer(sprayer);

            /// INCLUDE MORE WEAPONS HERE
        }

        // If the player is not firing, reset the variable that requires trigger releasing
        else
        {
            if (singleShotFired) singleShotFired = false;

            /// TODO: Make RegainAmmo() do 1 ammo at a time when UI is implemented
            if (player.IsGrounded() && equippedWeapon is W_Shootable weapon) RegainAmmo(weapon.ammoCapacity);
        }
    }



    // ===========================================================
    //                 INITIALIZATION AND CONTROLS
    // ===========================================================


    // Start is called before the first frame update
    void Start()
    {
        // Set up arm loadouts
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
    protected void Aim(Vector3 aimVal)
    {
        float degrees = Vector3.Angle(Vector3.up, transform.position + (aimVal * 1000));
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        if (aimVal.x > 0)
            degrees = -degrees;
        if (aimVal.sqrMagnitude > 0.5)
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, degrees);
    }


    // Switches the arm's currently equipped weapon
    protected void Switch(Weapon weapon)
    {
        if (weapon)
        {
            this.equippedWeapon = weapon;
            gameObject.GetComponent<MeshFilter>().mesh = weapon.mesh;
            gameObject.GetComponent<MeshRenderer>().material = weapon.material;
        }
    }


    // Initializes the loadout of the arm based on it being the front arm
    protected void FrontArmInitialize()
    {
        weaponA = player.frontArmWeapons[0];
        weaponB = player.frontArmWeapons[1];
        weaponC = player.frontArmWeapons[2];
        weaponD = player.frontArmWeapons[3];
    }


    // Initializes the loadout of the arm based on it being the back arm
    protected void BackArmInitialize()
    {
        weaponA = player.backArmWeapons[0];
        weaponB = player.backArmWeapons[1];
        weaponC = player.backArmWeapons[2];
        weaponD = player.backArmWeapons[3];
    }



    // ===========================================================
    //                       ALL THINGS AMMO
    // ===========================================================


    /// <summary>
    /// Reduces the ammo of the equipped weapon by a specified amount
    /// </summary>
    /// <param name="count"> Amount to reduce ammo by </param>
    public void ReduceAmmo(int count)
    {
        // Finds the equipped weapon
        // Sets the ammo of the weapon to either 0 or ammoRemaining - count, prevents negative ammo

        if (equippedWeapon.Equals(weaponA))
            ammoRemaining[weaponA] = Mathf.Max(ammoRemaining[weaponA] - count, 0);

        else if (equippedWeapon.Equals(weaponB))
            ammoRemaining[weaponB] = Mathf.Max(ammoRemaining[weaponB] - count, 0);

        else if (equippedWeapon.Equals(weaponC))
            ammoRemaining[weaponC] = Mathf.Max(ammoRemaining[weaponC] - count, 0);

        else if (equippedWeapon.Equals(weaponD))
            ammoRemaining[weaponD] = Mathf.Max(ammoRemaining[weaponD] - count, 0);


        if (this is Arm_Controlled)
        {
            GameManager.Instance().uiManager.UpdateUI();
        }
    }


    /// <summary>
    /// Increases the ammo of the equipped weapon by a specified amount
    /// </summary>
    /// <param name="count"> Amount to increase ammo by </param>
    public void RegainAmmo(int count)
    {
        // Checks if the equipped weapon should have ammo
        if (equippedWeapon is W_Shootable weapon)
        {
            // Finds the equipped weapon
            // Sets the ammo of the weapon to either max or ammoRemaining + count, prevents overloading ammo

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

    /// <summary>
    /// Fully reloads all weapons if they have ammo
    /// </summary>
    public void FullReload()
    {
        if (weaponA is W_Shootable gunA)
            ammoRemaining[weaponA] = ammoRemaining[weaponA] + gunA.ammoCapacity;
        if (weaponB is W_Shootable gunB)
            ammoRemaining[weaponB] = ammoRemaining[weaponB] + gunB.ammoCapacity;
        if (weaponC is W_Shootable gunC)
            ammoRemaining[weaponC] = ammoRemaining[weaponC] + gunC.ammoCapacity;
        if (weaponD is W_Shootable gunD)
            ammoRemaining[weaponD] = ammoRemaining[weaponD] + gunD.ammoCapacity;
    }


    public Dictionary<Weapon, int> GetAmmoRemaining()
    {
        return ammoRemaining;
    }

}
