using UnityEngine;
using System.Collections;
using System;
using Mirror;

public class Cauldron : NetworkBehaviour
{
    // Start is called before the first frame update

    public int pourStart;
    public int pourInterval;

    void Start()
    {
        gameObject.SetActive(false);
        if(isServer){
            InvokeRepeating("Pour", pourStart, pourInterval);
        }
        
    }

    // Update is called once per frame
    [Command]
    void Pour(){
        RPCpour();
    }

    [ClientRpc]
    void RPCpour(){
if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }else{
            gameObject.SetActive(true);
        }
    }
}
