using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public GameObject prefab_Explosion;

    private Rigidbody proj_rb;
    private float explosionRadius;
    private float coreDamage;
    private float corePushback;

    /// <summary>
    /// Fills in the projectile's values based on the W_Launcher's values
    /// </summary>
    /// <param name="direction"> Direction the projectile will fire at </param>
    /// <param name="projectilePower"> Speed of the projectile </param>
    /// <param name="explosionRadius"> Radius of the explosion if projectile has explosionRadius </param>
    /// <param name="coreDamage"> Damage at the core of the explosion if projectile has explosionRadius </param>
    /// <param name="corePushback"> Pushback at the core of the explosion if projectile has explosionRadius </param>
    /// <param name="rocketPowered"> True if gravity does not affect, false if gravity affects </param>
    public void Initialize(Vector3 direction, float projectilePower, float explosionRadius, float coreDamage, float corePushback, bool rocketPowered)
    {
        this.explosionRadius = explosionRadius;
        this.coreDamage = coreDamage;
        this.corePushback = corePushback;

        proj_rb.useGravity = !rocketPowered;
        proj_rb.AddForce(direction.normalized * projectilePower);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Creates explosion if there is an explosion radius
        if (explosionRadius > 0)
        {
            // Spawn explosion prefab
            GameObject projectile = Instantiate(prefab_Explosion, transform.position, Quaternion.identity);
            projectile.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, explosionRadius * 2);
            Destroy(projectile, 0.5f);

            // Bit shift the index of the layer (8) to get a bit mask for solid ground.
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            // Checks if explosion hits any objects not in the solid ground layer
            Collider[] exploded = Physics.OverlapSphere(gameObject.transform.position, explosionRadius, layerMask);

            // For each object hit
            for (int i = 0; i < exploded.Length; i++)
            {
                /// TODO: Make players in explosion take damage

                // Add explosion force to rigidbody if exists
                Rigidbody rb = exploded[i].GetComponent<Rigidbody>();
                if (rb != null) { rb.AddExplosionForce(corePushback, gameObject.transform.position, explosionRadius); }
            }

            Destroy(gameObject);
        }
        // Does not create an explosion if there is no explosion radius
        else
        {
            // Inflict pushback on collided object if it has a RigidBody
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = other.gameObject.transform.position - gameObject.transform.position;
                rb.AddForce(direction.normalized * corePushback);
            }

            /// TODO: Make player take damage

            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        proj_rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, 10);
    }
}
