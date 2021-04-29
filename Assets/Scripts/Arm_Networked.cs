using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Arm_Networked : NetworkBehaviour
{

    // ===========================================================
    //                          VARIABLES
    // ===========================================================

    public Arm arm;

    private void Awake()
    {
        arm.onlineArm = this;
    }


    // ===========================================================
    //                      ATTACKS AND DAMAGE
    // ===========================================================

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
    public void CmdSpawnProjectile(string projectilePrefabName, Vector3 projectilePath, float projectilePower, float explosionRadius, float coreDamage, float corePushback, bool rocketPowered)
    {
        if (!isServer)
            return;

        // Instantiates a projectile prefab from the Resources/Projectiles folder
        GameObject projectile = (GameObject)Instantiate(Resources.Load("Projectiles/Networked/" + projectilePrefabName), projectilePath + (arm.barrel.transform.up * 0.5f), Quaternion.identity);

        // Spawns projectile across all clients
        NetworkServer.Spawn(projectile);

        // Initializes the projectile with the appropriate launcher values across all clients
        projectile.GetComponent<Projectile_Networked>().RpcInitialize(arm.barrel.transform.up, projectilePower, explosionRadius,
                                coreDamage, corePushback, rocketPowered, arm.player.GetPlayerID());
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
    public void CmdAttack(Player_Networked recipient, float damage, int attackerID, Vector3 pushback, WeaponType weaponType)
    {
        if (!isServer)
            return;

        recipient.DecreaseHealth(damage, attackerID, weaponType);
        recipient.gameObject.GetComponent<Player_Networked>().RpcEnactForce(pushback);
    }


    //void Attack_AI(Player_AI recipient, float damage, int attackerID, Vector3 pushback, WeaponType weaponType)
    //{
    //    recipient.DecreaseHealth(damage, attackerID, weaponType);
    //    recipient.RpcEnactForce(pushback);
    //}

    /// <summary>
    /// Tells the server where to draw a bullet path
    /// </summary>
    /// <param name="start"> Start point of path </param>
    /// <param name="end"> End point of path </param>
    [Command]
    public void CmdDrawBullet(Vector3 start, Vector3 end)
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
        LineRenderer path = Instantiate(arm.bullet);
        Destroy(path.gameObject, 0.2f);
        path.SetPosition(0, start);
        path.SetPosition(1, end);
    }


    [Command]
    public void CmdActivateParticles(string prefabName)
    {
        if (!isServer)
            return;

        RpcActivateParticles(prefabName);
    }

    [ClientRpc]
    void RpcActivateParticles(string prefabName)
    {
        foreach (ParticleSystem p in arm.weaponObjs[prefabName].GetComponentsInChildren<ParticleSystem>())
        {
            if (p != null)
                p.Play();
        }
    }


    [Command]
    public void CmdDeactivateParticles(string prefabName)
    {
        if (!isServer)
            return;

        RpcDeactivateParticles(prefabName);
    }

    [ClientRpc]
    void RpcDeactivateParticles(string prefabName)
    {
        foreach (ParticleSystem p in arm.weaponObjs[prefabName].GetComponentsInChildren<ParticleSystem>())
        {
            if (p != null)
                p.Stop();
        }
    }

    // ===========================================================
    //                  ARM CONTROLS  AND VISUALS
    // ===========================================================


    // Switches the arm's currently equipped weapon
    public void Switch(Weapon weapon)
    {
        if (!Application.isFocused)
            return;

        arm.Switch(weapon);

        if (weapon)
        {
            // Change equipped weapon visuals
            CmdSwitchAppearance(weapon.prefab.name);
        }
    }


    /// <summary>
    /// Tells the server to refresh the appearance of the Arm
    /// </summary>
    /// <param name="meshName"> Mesh file name, loaded from Resources/Meshes </param>
    /// <param name="materialName"> Material file name, loaded from Resources/Materials </param>
    [Command]
    public void CmdSwitchAppearance(string prefabName)
    {
        if (!isServer)
            return;

        RpcSwitchAppearance(prefabName);
    }


    /// <summary>
    /// Every client refreshes the appearance of their Arm
    /// </summary>
    /// <param name="meshName"> Mesh file name, loaded from Resources/Meshes </param>
    /// <param name="materialName"> Material file name, loaded from Resources/Materials </param>
    [ClientRpc]
    private void RpcSwitchAppearance(string prefabName)
    {
        arm.SwitchAppearance(prefabName);

        //arm.arm.GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load("Meshes/" + meshName);
        //arm.arm.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Materials/" + materialName);
    }

}
