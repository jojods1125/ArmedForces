using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(MeshRenderer))]
public class Projectile : MonoBehaviour
{
    public GameObject prefab_Explosion;
    public bool spinning;

    private Rigidbody proj_rb;
    private Collider proj_collide;
    private MeshRenderer proj_rend;
    private float explosionRadius;
    private float coreDamage;
    private float corePushback;

    //The if of the player that shot the projectile
    private int playerID;

    /// <summary>
    /// Fills in the projectile's values based on the W_Launcher's values
    /// </summary>
    /// <param name="direction"> Direction the projectile will fire at </param>
    /// <param name="projectilePower"> Speed of the projectile </param>
    /// <param name="explosionRadius"> Radius of the explosion if projectile has explosionRadius </param>
    /// <param name="coreDamage"> Damage at the core of the explosion if projectile has explosionRadius </param>
    /// <param name="corePushback"> Pushback at the core of the explosion if projectile has explosionRadius </param>
    /// <param name="rocketPowered"> True if gravity does not affect, false if gravity affects </param>
    public void Initialize(Vector3 direction, float projectilePower, float explosionRadius, float coreDamage, float corePushback, bool rocketPowered, int playerID)
    {
        this.explosionRadius = explosionRadius;
        this.coreDamage = coreDamage;
        this.corePushback = corePushback;
        this.playerID = playerID;

        Quaternion rotation = new Quaternion();
        rotation.SetLookRotation(direction, Vector3.up);

        transform.localRotation = rotation;
        proj_rb.useGravity = !rocketPowered;
        proj_rb.AddForce(direction.normalized * projectilePower);
    }

    void Update()
    {
        if (spinning)
        {
            transform.Rotate(new Vector3(0, 0, 1), 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // THIS HAPPENS CLIENT-SIDE, CAUSING A SLIGHT OFFSET FROM WHAT THE SERVER HAS

        if (other.gameObject.layer != LayerMask.NameToLayer("Bound") && other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
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
                    // Players in explosion take damage
                    if (exploded[i].gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        float damageMultiplier = 1 / (explosionRadius / (explosionRadius - Vector3.Distance(exploded[i].ClosestPoint(gameObject.transform.position), gameObject.transform.position)));
                        damageMultiplier = Mathf.Min(Mathf.Max(damageMultiplier, 0), 1);

                        if (exploded[i].gameObject.GetComponent<Player_Networked>() != null)
                            exploded[i].gameObject.GetComponent<Player_Networked>().DecreaseHealth(coreDamage * damageMultiplier, playerID, WeaponType.launcher);
                        else if (exploded[i].gameObject.GetComponent<Player>() != null)
                            exploded[i].gameObject.GetComponent<Player>().DecreaseHealth(coreDamage * damageMultiplier, playerID, WeaponType.launcher);
                        //else if (exploded[i].gameObject.GetComponent<Player_AI>() != null)
                        //    exploded[i].gameObject.GetComponent<Player_AI>().DecreaseHealth(coreDamage * damageMultiplier, playerID, WeaponType.launcher);
                    }

                    // Add explosion force to rigidbody if exists
                    Rigidbody rb = exploded[i].GetComponent<Rigidbody>();
                    if (rb != null) { rb.AddExplosionForce(corePushback, gameObject.transform.position, explosionRadius); }
                }

                // Give clients 0.5 seconds to catch up to server, then destroy
                proj_rb.constraints = RigidbodyConstraints.FreezeAll;
                proj_collide.enabled = false;
                proj_rend.enabled = false;
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
                Destroy(gameObject, .5f);
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

                // Player take damage
                if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (other.gameObject.GetComponent<Player_Networked>() != null)
                        other.gameObject.GetComponent<Player_Networked>().DecreaseHealth(coreDamage, playerID, WeaponType.launcher);
                    else if (other.gameObject.GetComponent<Player>() != null)
                        other.gameObject.GetComponent<Player>().DecreaseHealth(coreDamage, playerID, WeaponType.launcher);
                    //else if (other.gameObject.GetComponent<Player_AI>() != null)
                    //    other.gameObject.GetComponent<Player_AI>().DecreaseHealth(coreDamage, playerID, WeaponType.launcher);
                }

                // Give clients 0.5 seconds to catch up to server, then destroy
                proj_rb.constraints = RigidbodyConstraints.FreezeAll;
                proj_collide.enabled = false;
                proj_rend.enabled = false;
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
                Destroy(gameObject, .5f);
            }
        }
    }


    private void Awake()
    {
        proj_rb = gameObject.GetComponent<Rigidbody>();
        proj_collide = gameObject.GetComponent<Collider>();
        proj_rend = gameObject.GetComponent<MeshRenderer>();
        Destroy(gameObject, 10);
    }
}
