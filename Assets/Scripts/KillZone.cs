using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.gameObject.GetComponent<Player>() != null)
                other.gameObject.GetComponent<Player>().CmdKill(-1, WeaponType.none);
            else if (other.gameObject.GetComponent<Player_AI>() != null)
                other.gameObject.GetComponent<Player_AI>().CmdKill(-1, WeaponType.none);
        }

    }
}
