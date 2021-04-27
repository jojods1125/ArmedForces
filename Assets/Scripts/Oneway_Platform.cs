using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oneway_Platform : MonoBehaviour
{
    public GameObject platform;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 9){
            Physics.IgnoreCollision(other.GetComponent<Collider>(), platform.GetComponent<Collider>(), true);
        }
        
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 9){
            Physics.IgnoreCollision(other.GetComponent<Collider>(), platform.GetComponent<Collider>(), false);
        }
    }
}
