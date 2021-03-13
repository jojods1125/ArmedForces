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
    /// <summary> Previous time the weapon reloaded </summary>
    private float reloadRateTimeStamp = 0;


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

    private readonly Dictionary<Weapon, char> weaponLetters = new Dictionary<Weapon, char>();

    protected UIManager uiManager;


    /// <summary>
    /// Fires an AutoGun Weapon
    /// </summary>
    /// <param name="auto">Equipped W_AutoGun</param>
    void FireAuto(W_AutoGun auto)
    {
        // Fire rate and ammo check
        if (Time.time > fireRateTimeStamp + auto.fireRate && ammoRemaining[auto] > 0)
        {
            // Updates fire rate and ammo
            fireRateTimeStamp = Time.time;
            reloadRateTimeStamp = Time.time;
            ReduceAmmo(1);
            AchievementManager.Instance().OnEvent(AchievementType.shotsFired, 1, WeaponType.auto);

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
                    hit.collider.gameObject.GetComponent<Player>().DecreaseHealth(auto.bulletDamage, player.playerID);
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
        if (!singleShotFired && Time.time > fireRateTimeStamp + semi.fireRate)
        {
            // Ammo check
            if (ammoRemaining[semi] > 0)
            {
                // Updates trigger, fire rate, and ammo
                singleShotFired = true;
                fireRateTimeStamp = Time.time;
                reloadRateTimeStamp = Time.time;
                ReduceAmmo(1);
                AchievementManager.Instance().OnEvent(AchievementType.shotsFired, 1, WeaponType.semi);

                // Burst loop
                for (int i = 0; i < semi.burstCount; i++)
                {
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
                            hit.collider.gameObject.GetComponent<Player>().DecreaseHealth(semi.bulletDamage, player.playerID);
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
        if (!singleShotFired && Time.time > fireRateTimeStamp + launcher.fireRate)
        {
            // Ammo check
            if (ammoRemaining[launcher] > 0)
            {
                // Updates trigger, fire rate, and ammo
                singleShotFired = true;
                fireRateTimeStamp = Time.time;
                reloadRateTimeStamp = Time.time;
                ReduceAmmo(1);
                AchievementManager.Instance().OnEvent(AchievementType.shotsFired, 1, WeaponType.launcher);

                // Creates projectile and spawns it at the correct location
                Vector3 projectilePath = new Vector3(barrel.transform.position.x, barrel.transform.position.y);
                GameObject projectile = Instantiate(launcher.projectilePrefab, projectilePath + (barrel.transform.up * 0.5f), Quaternion.identity);

                // Initializes the projectile prefab with the appropriate launcher values
                projectile.GetComponent<Projectile>().Initialize(barrel.transform.up, launcher.projectilePower, launcher.explosionRadius,
                                        launcher.coreDamage, launcher.corePushback, launcher.rocketPowered, player.playerID);

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
        if (Time.time > fireRateTimeStamp + sprayer.fireRate && ammoRemaining[sprayer] > 0)
        {
            // Updates fire rate and ammo
            fireRateTimeStamp = Time.time;
            reloadRateTimeStamp = Time.time;
            ReduceAmmo(1);
            AchievementManager.Instance().OnEvent(AchievementType.shotsFired, 1, WeaponType.sprayer);

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
                    hit.collider.gameObject.GetComponent<Player>().DecreaseHealth(sprayer.bulletDamage, player.playerID);
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
            {
                FireSemi(semi);
                /*AchievementManager.Instance().OnEvent(AchievementType.shotsFired, 1, WeaponType.semi);*/
            }
            else if (equippedWeapon is W_AutoGun auto)
            {
                FireAuto(auto);
                /*AchievementManager.Instance().OnEvent(AchievementType.shotsFired, 1, WeaponType.auto);*/
            }
            else if (equippedWeapon is W_Launcher launcher)
            {
                FireLauncher(launcher);
                /*AchievementManager.Instance().OnEvent(AchievementType.shotsFired, 1, WeaponType.launcher);*/
            }
            else if (equippedWeapon is W_Sprayer sprayer)
            {
                FireSprayer(sprayer);
                /*AchievementManager.Instance().OnEvent(AchievementType.shotsFired, 1, WeaponType.sprayer);*/
            }
            /// INCLUDE MORE WEAPONS HERE
        }

        // If the player is not firing, reset the variable that requires trigger releasing
        else
        {
            if (singleShotFired) singleShotFired = false;

            // Currently equipped weapon regains ammo if player is grounded
            if (player.IsGrounded() && equippedWeapon is W_Shootable weapon)
            {
                if (Time.time > reloadRateTimeStamp + weapon.reloadRate && ammoRemaining[equippedWeapon] != weapon.ammoCapacity)
                {
                    RegainAmmo(1);
                    reloadRateTimeStamp = Time.time;
                }
            }
        }

    }



    // ===========================================================
    //                 INITIALIZATION AND CONTROLS
    // ===========================================================


    // Start is called before the first frame update
    protected void Start()
    {
        // Set up arm loadouts
        if (armType == ArmType.Front) FrontArmInitialize();
        else BackArmInitialize();

        // Fills in the dictionary of weapons to letters for checking which they are
        weaponLetters[weaponA] = 'A';
        weaponLetters[weaponB] = 'B';
        weaponLetters[weaponC] = 'C';
        weaponLetters[weaponD] = 'D';

        // Initialize equippedWeapon
        if (weaponA) Switch(weaponA);
        else if (weaponB) Switch(weaponB);
        else if (weaponC) Switch(weaponC);
        else Switch(weaponD);

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
        
        // Starts the match with full ammo in each weapon
        FullReload();
    }


    // Aims the arm based on joystick angle
    public void Aim(Vector3 aimVal)
    {
        float degrees = Vector3.Angle(Vector3.up, transform.position + (aimVal * 1000));
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        if (aimVal.x > 0)
            degrees = -degrees;
        if (aimVal.sqrMagnitude > 0.5)
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, degrees);
    }

    

    // Switches the arm's currently equipped weapon
    public void Switch(Weapon weapon)
    {
        if (weapon)
        {
            // Change currently equipped weapon
            equippedWeapon = weapon;

            // Change equipped weapon visuals
            gameObject.GetComponent<MeshFilter>().mesh = weapon.mesh;
            gameObject.GetComponent<MeshRenderer>().material = weapon.material;

            // Update UI
            if (uiManager) uiManager.UpdateSelectedUI(armType, weaponLetters[weapon]);
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

    public Weapon getWeaponA(){
        return weaponA;
    }
    public Weapon getWeaponB(){
        return weaponB;
    }
    public Weapon getWeaponC(){
        return weaponC;
    }
    public Weapon getWeaponD(){
        return weaponD;
    }
    public void SetFiring(bool firing){
        this.firing = firing;
    }
    public void releaseTrigger(){
        singleShotFired = false;
        
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
        // Checks if the equipped weapon should have ammo
        if (equippedWeapon is W_Shootable weapon)
        {
            // Finds the equipped weapon
                // Sets the ammo of the weapon to either 0 or ammoRemaining - count, prevents negative ammo
                // Updates UI

            if (equippedWeapon.Equals(weaponA))
            {
                ammoRemaining[weaponA] = Mathf.Max(ammoRemaining[weaponA] - count, 0);
                if (uiManager) uiManager.UpdateAmmoUI(armType, 'A', ammoRemaining[weaponA], weapon.ammoCapacity);
            }

            else if (equippedWeapon.Equals(weaponB))
            {
                ammoRemaining[weaponB] = Mathf.Max(ammoRemaining[weaponB] - count, 0);
                if (uiManager) uiManager.UpdateAmmoUI(armType, 'B', ammoRemaining[weaponB], weapon.ammoCapacity);
            }

            else if (equippedWeapon.Equals(weaponC))
            {
                ammoRemaining[weaponC] = Mathf.Max(ammoRemaining[weaponC] - count, 0);
                if (uiManager) uiManager.UpdateAmmoUI(armType, 'C', ammoRemaining[weaponC], weapon.ammoCapacity);
            }

            else if (equippedWeapon.Equals(weaponD))
            {
                ammoRemaining[weaponD] = Mathf.Max(ammoRemaining[weaponD] - count, 0);
                if (uiManager) uiManager.UpdateAmmoUI(armType, 'D', ammoRemaining[weaponD], weapon.ammoCapacity);
            }

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
                // Sets the ammo of the weapon to either max capacity or ammoRemaining + count, prevents overloading ammo
                // Updates UI

            if (equippedWeapon.Equals(weaponA))
            {
                ammoRemaining[weaponA] = Mathf.Min(ammoRemaining[weaponA] + count, weapon.ammoCapacity);
                if (uiManager) uiManager.UpdateAmmoUI(armType, 'A', ammoRemaining[weaponA], weapon.ammoCapacity);
            }

            else if (equippedWeapon.Equals(weaponB))
            {
                ammoRemaining[weaponB] = Mathf.Min(ammoRemaining[weaponB] + count, weapon.ammoCapacity);
                if (uiManager) uiManager.UpdateAmmoUI(armType, 'B', ammoRemaining[weaponB], weapon.ammoCapacity);
            }

            else if (equippedWeapon.Equals(weaponC))
            {
                ammoRemaining[weaponC] = Mathf.Min(ammoRemaining[weaponC] + count, weapon.ammoCapacity);
                if (uiManager) uiManager.UpdateAmmoUI(armType, 'C', ammoRemaining[weaponC], weapon.ammoCapacity);
            }

            else if (equippedWeapon.Equals(weaponD))
            {
                ammoRemaining[weaponD] = Mathf.Min(ammoRemaining[weaponD] + count, weapon.ammoCapacity);
                if (uiManager) uiManager.UpdateAmmoUI(armType, 'D', ammoRemaining[weaponD], weapon.ammoCapacity);
            }

        }
    }


    /// <summary>
    /// Fully reloads all weapons if they have ammo
    /// </summary>
    public void FullReload()
    {
        // Checks if each weapon should have ammo
            // Fully reloads weapon
            // Updates UI

        if (weaponA is W_Shootable gunA)
        {
            ammoRemaining[weaponA] = gunA.ammoCapacity;
            if (uiManager) uiManager.UpdateAmmoUI(armType, 'A', ammoRemaining[weaponA], gunA.ammoCapacity);
        }

        if (weaponB is W_Shootable gunB)
        {
            ammoRemaining[weaponB] = gunB.ammoCapacity;
            if (uiManager) uiManager.UpdateAmmoUI(armType, 'B', ammoRemaining[weaponB], gunB.ammoCapacity);
        }
            
        if (weaponC is W_Shootable gunC)
        {
            ammoRemaining[weaponC] = gunC.ammoCapacity;
            if (uiManager) uiManager.UpdateAmmoUI(armType, 'C', ammoRemaining[weaponC], gunC.ammoCapacity);
        }
            
        if (weaponD is W_Shootable gunD)
        {
            ammoRemaining[weaponD] = gunD.ammoCapacity;
            if (uiManager) uiManager.UpdateAmmoUI(armType, 'D', ammoRemaining[weaponD], gunD.ammoCapacity);
        }
            
    }

}
