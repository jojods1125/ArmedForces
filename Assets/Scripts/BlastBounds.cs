using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastBounds : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Player>().RpcKill(-1);
        }

    }
}
