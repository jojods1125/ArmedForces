using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public GameObject prefab_Explosion;

    private Rigidbody proj_rb;
    private Vector3 direction;
    private float projectilePower;
    private float explosionRadius;
    private float coreDamage;
    private float corePushback;
    private bool explosive;
    private bool rocketPowered;

    public void ExplosiveProjectile(Vector3 direction, float projectilePower, float explosionRadius, float coreDamage, float corePushback, bool rocketPowered)
    {
        this.direction = direction;
        this.projectilePower = projectilePower;
        this.explosionRadius = explosionRadius;
        this.coreDamage = coreDamage;
        this.corePushback = corePushback;
        this.rocketPowered = rocketPowered;
        explosive = true;

        proj_rb.useGravity = !rocketPowered;
        proj_rb.AddForce(direction * projectilePower);
    }

    public void NonExplosiveProjectile(Vector3 direction, float power, float coreDamage, float corePushback, bool rocketPowered)
    {
        this.direction = direction;
        this.projectilePower = power;
        this.coreDamage = coreDamage;
        this.corePushback = corePushback;
        this.rocketPowered = rocketPowered;
        explosionRadius = 0;
        //edgeDamage = 0;
        //edgePushback = 0;
        explosive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (explosive)
        {
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
                // Add explosion force to rigidbody if exists
                Rigidbody rb = exploded[i].GetComponent<Rigidbody>();
                if (rb != null) { rb.AddExplosionForce(corePushback, gameObject.transform.position, explosionRadius); }
            }

            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        proj_rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, 10);
    }
}
