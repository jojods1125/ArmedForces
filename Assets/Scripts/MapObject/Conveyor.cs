using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Conveyor : MonoBehaviour
{
    public float xForce;
    public float yForce;
    public float zForce;

    // Start is called before the first frame update
    void OnTriggerStay (Collider other){
        
        if(other.gameObject.GetComponent<Player>()){
            other.gameObject.GetComponent<Rigidbody>().position = other.gameObject.GetComponent<Rigidbody>().position + (new Vector3(xForce, yForce, zForce));
            
        }
        
    }
}
