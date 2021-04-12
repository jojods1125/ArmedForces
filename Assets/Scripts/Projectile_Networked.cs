using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Projectile))]
public class Projectile_Networked : NetworkBehaviour
{
    public Projectile projectile;

    /// <summary>
    /// Fills in the projectile's values based on the W_Launcher's values
    /// </summary>
    /// <param name="direction"> Direction the projectile will fire at </param>
    /// <param name="projectilePower"> Speed of the projectile </param>
    /// <param name="explosionRadius"> Radius of the explosion if projectile has explosionRadius </param>
    /// <param name="coreDamage"> Damage at the core of the explosion if projectile has explosionRadius </param>
    /// <param name="corePushback"> Pushback at the core of the explosion if projectile has explosionRadius </param>
    /// <param name="rocketPowered"> True if gravity does not affect, false if gravity affects </param>
    [ClientRpc]
    public void RpcInitialize(Vector3 direction, float projectilePower, float explosionRadius, float coreDamage, float corePushback, bool rocketPowered, int playerID)
    {
        projectile.Initialize(direction, projectilePower, explosionRadius, coreDamage, corePushback, rocketPowered, playerID);
    }



}
