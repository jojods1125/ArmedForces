using System.Collections.Generic;
using UnityEngine;
using Mirror;

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


public class Arm : NetworkBehaviour
{
    [Header("Local References")]

    [Tooltip("The arm GameObject")]
    public GameObject arm;
    [Tooltip("Tip of the arm, where shots emit from")]
    public GameObject barrel;
    [Tooltip("Player character reference")]
    public Player player;


    [Header("Arm Data")]

    [Tooltip("Whether the arm uses the player's Front Arm or Back Arm weapons")]
    public ArmType armType;
    [Tooltip("Bullet prefab reference")]
    public LineRenderer bullet;


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
    /// <summary> Dictionary used for knowing which weapons are in which slots </summary>
    private readonly Dictionary<Weapon, char> weaponLetters = new Dictionary<Weapon, char>();
    /// <summary> UIManager, as retrieved from GameManager </summary>
    protected UIManager uiManager;



    // ===========================================================
    //                      ATTACKS AND DAMAGE
    // ===========================================================

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
            ///Debug.DrawRay(barrel.transform.position, bulletPath * 1000f, Color.green, 1);


            // Raycasts bullet path
            if (Physics.Raycast(barrel.transform.position, bulletPath, out RaycastHit hit))
            {
                CmdDrawBullet(barrel.transform.position, hit.point);

                ///Debug.Log("HIT " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (hit.collider.gameObject.GetComponent<Player>() != null)
                        CmdAttack(hit.collider.gameObject.GetComponent<Player>(), auto.bulletDamage, player.playerID, bulletPath.normalized * auto.bulletPushback, WeaponType.auto);
                    else if (hit.collider.gameObject.GetComponent<Player_AI>() != null)
                        Attack_AI(hit.collider.gameObject.GetComponent<Player_AI>(), auto.bulletDamage, player.playerID, bulletPath.normalized * auto.bulletPushback, WeaponType.auto);
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
                    ///Debug.DrawRay(barrel.transform.position, bulletPath * 1000f, Color.red, 1);

                    // Raycasts bullet path
                    if (Physics.Raycast(barrel.transform.position, bulletPath, out RaycastHit hit))
                    {
                        CmdDrawBullet(barrel.transform.position, hit.point);

                        ///Debug.Log("HIT " + hit.collider.gameObject.name);
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            if (hit.collider.gameObject.GetComponent<Player>() != null)
                                CmdAttack(hit.collider.gameObject.GetComponent<Player>(), semi.bulletDamage, player.playerID, bulletPath.normalized * semi.bulletPushback, WeaponType.semi);
                            else if (hit.collider.gameObject.GetComponent<Player_AI>() != null)
                                Attack_AI(hit.collider.gameObject.GetComponent<Player_AI>(), semi.bulletDamage, player.playerID, bulletPath.normalized * semi.bulletPushback, WeaponType.semi);
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

                // Spawns the projectile in the server
                CmdSpawnProjectile(launcher.projectilePrefab.name, projectilePath, launcher.projectilePower, launcher.explosionRadius, launcher.coreDamage, launcher.corePushback, launcher.rocketPowered);

                // Pushes player
                player.EnactForce(barrel.transform.up.normalized * -launcher.pushback);
            }
        }

    }

    /// <summary>
    /// Instantiates a projectile prefab on the server and tells the clients to spawn it
    /// </summary>
    /// <param name="projectilePrefabName"> String of the prefab's name, which gets loaded from Resources/Projectiles </param>
    /// <param name="projectilePath"> Location of barrel </param>
    /// <param name="projectilePower"> Power of the projectile </param>
    /// <param name="explosionRadius"> Radius of the explosion </param>
    /// <param name="coreDamage"> Damage at the impact point </param>
    /// <param name="corePushback"> Pushback at the impact point </param>
    /// <param name="rocketPowered"> Whether to use gravity or not </param>
    [Command]
    void CmdSpawnProjectile(string projectilePrefabName, Vector3 projectilePath, float projectilePower, float explosionRadius, float coreDamage, float corePushback, bool rocketPowered)
    {
        if (!isServer)
            return;

        // Instantiates a projectile prefab from the Resources/Projectiles folder
        GameObject projectile = (GameObject)Instantiate(Resources.Load("Projectiles/" + projectilePrefabName), projectilePath + (barrel.transform.up * 0.5f), Quaternion.identity);

        // Spawns projectile across all clients
        NetworkServer.Spawn(projectile);

        // Initializes the projectile with the appropriate launcher values across all clients
        projectile.GetComponent<Projectile>().RpcInitialize(barrel.transform.up, projectilePower, explosionRadius,
                                coreDamage, corePushback, rocketPowered, player.playerID);
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
            ///Debug.DrawRay(barrel.transform.position, bulletPath * sprayer.sprayDistance, Color.yellow, 1);
            CmdDrawBullet(barrel.transform.position, barrel.transform.position + (bulletPath * sprayer.sprayDistance));

            // Raycasts bullet path
            if (Physics.Raycast(barrel.transform.position, bulletPath, out RaycastHit hit, sprayer.sprayDistance))
            {
                ///Debug.Log("HIT " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (hit.collider.gameObject.GetComponent<Player>() != null)
                        CmdAttack(hit.collider.gameObject.GetComponent<Player>(), sprayer.bulletDamage, player.playerID, bulletPath.normalized * sprayer.bulletPushback, WeaponType.sprayer);
                    else if (hit.collider.gameObject.GetComponent<Player_AI>() != null)
                        Attack_AI(hit.collider.gameObject.GetComponent<Player_AI>(), sprayer.bulletDamage, player.playerID, bulletPath.normalized * sprayer.bulletPushback, WeaponType.sprayer);
                }
            }

            // Pushes player
            player.EnactForce(bulletPath.normalized * -sprayer.pushback);
        }
    }


    /// <summary>
    /// Deals damage and force to a Player
    /// </summary>
    /// <param name="recipient"> Player receiving the damage/force </param>
    /// <param name="damage"> Amount of damage dealt </param>
    /// <param name="attackerID"> Player ID dealing the damage </param>
    /// <param name="pushback"> Pushback force </param>
    /// <param name="weaponType"> Type of weapon </param>
    [Command]
    void CmdAttack(Player recipient, float damage, int attackerID, Vector3 pushback, WeaponType weaponType)
    {
        if (!isServer)
            return;

        recipient.DecreaseHealth(damage, attackerID, weaponType);
        recipient.RpcEnactForce(pushback);
    }


    void Attack_AI(Player_AI recipient, float damage, int attackerID, Vector3 pushback, WeaponType weaponType)
    {
        recipient.DecreaseHealth(damage, attackerID, weaponType);
        recipient.RpcEnactForce(pushback);
    }

    /// <summary>
    /// Tells the server where to draw a bullet path
    /// </summary>
    /// <param name="start"> Start point of path </param>
    /// <param name="end"> End point of path </param>
    [Command]
    void CmdDrawBullet(Vector3 start, Vector3 end)
    {
        if (!isServer)
            return;

        RpcDrawBullet(start, end);
    }


    /// <summary>
    /// Draws the bullet path on every client
    /// </summary>
    /// <param name="start"> Start point of path </param>
    /// <param name="end"> End point of path </param>
    [ClientRpc]
    void RpcDrawBullet(Vector3 start, Vector3 end)
    {
        LineRenderer path = Instantiate(bullet);
        Destroy(path.gameObject, 0.2f);
        path.SetPosition(0, start);
        path.SetPosition(1, end);
    }



    void FixedUpdate()
    {
        // If the player is using the firing input, call the appropriate weapon firing function
        if (firing)
        {
            if (!Application.isFocused)
                return;

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
    //                  ARM CONTROLS  AND VISUALS
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

        // Initializes ammoRemaining dictionary for each weapon
        if (!ammoRemaining.ContainsKey(weaponA))
            if (weaponA is W_Shootable gunA) ammoRemaining.Add(weaponA, gunA.ammoCapacity);
        if (!ammoRemaining.ContainsKey(weaponB))
            if (weaponB is W_Shootable gunB) ammoRemaining.Add(weaponB, gunB.ammoCapacity);
        if (!ammoRemaining.ContainsKey(weaponC))
            if (weaponC is W_Shootable gunC) ammoRemaining.Add(weaponC, gunC.ammoCapacity);
        if (!ammoRemaining.ContainsKey(weaponD))
            if (weaponD is W_Shootable gunD) ammoRemaining.Add(weaponD, gunD.ammoCapacity);
        
        // Starts the match with full ammo in each weapon
        FullReload();
    }


    // Aims the arm based on joystick angle
    public void Aim(Vector3 aimVal)
    {
        if (!Application.isFocused)
            return;

        float degrees = Vector3.Angle(Vector3.up, arm.transform.position + (aimVal * 1000));
        Vector3 eulerRotation = arm.transform.rotation.eulerAngles;
        if (aimVal.x > 0)
            degrees = -degrees;
        if (aimVal.sqrMagnitude > 0.5)
            arm.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, degrees);
    }

    

    // Switches the arm's currently equipped weapon
    public void Switch(Weapon weapon)
    {
        if (!Application.isFocused)
            return;

        if (weapon)
        {
            // Change currently equipped weapon
            equippedWeapon = weapon;

            // Change equipped weapon visuals
            CmdSwitchAppearance(weapon.mesh.name, weapon.material.name);

            // Update UI
            if (uiManager) uiManager.UpdateSelectedUI(armType, weaponLetters[weapon]);
        }
    }


    /// <summary>
    /// Tells the server to refresh the appearance of the Arm
    /// </summary>
    /// <param name="meshName"> Mesh file name, loaded from Resources/Meshes </param>
    /// <param name="materialName"> Material file name, loaded from Resources/Materials </param>
    [Command]
    public void CmdSwitchAppearance(string meshName, string materialName)
    {
        if (!isServer)
            return;

        RpcSwitchAppearance(meshName, materialName);
    }


    /// <summary>
    /// Every client refreshes the appearance of their Arm
    /// </summary>
    /// <param name="meshName"> Mesh file name, loaded from Resources/Meshes </param>
    /// <param name="materialName"> Material file name, loaded from Resources/Materials </param>
    [ClientRpc]
    private void RpcSwitchAppearance(string meshName, string materialName)
    {
        arm.GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load("Meshes/" + meshName);
        arm.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Materials/" + materialName);
    }


    /// <summary>
    /// Gets the equippedWeapon
    /// </summary>
    /// <returns> Weapon currently equipped </returns>
    public Weapon GetEquippedWeapon()
    {
        return equippedWeapon;
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

    //Get weaponA
    public Weapon getWeaponA(){
        return weaponA;
    }
    //Get weaponB
    public Weapon getWeaponB(){
        return weaponB;
    }
    //Get weaponC
    public Weapon getWeaponC(){
        return weaponC;
    }
    //Get weaponD
    public Weapon getWeaponD(){
        return weaponD;
    }
    public int getAmmo(Weapon weapon){
        return ammoRemaining[weapon];
    }
    //Set firing
    public void SetFiring(bool firing){
        this.firing = firing;
    }
    public bool getFiring(){
        return firing;
    }
    //A way for the AI to "release trigger" and reset shooting
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
