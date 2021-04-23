using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oneway_Platform : MonoBehaviour
{
    public GameObject platform;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        Physics.IgnoreCollision(other.GetComponent<Collider>(), platform.GetComponent<Collider>(), true);
    }
    private void OnTriggerExit(Collider other) {
        Physics.IgnoreCollision(other.GetComponent<Collider>(), platform.GetComponent<Collider>(), false);
    }
}
