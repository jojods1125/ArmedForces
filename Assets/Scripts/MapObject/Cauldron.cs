using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    // Start is called before the first frame update

    public int pourStart;
    public int pourInterval;

    void Start()
    {
        gameObject.SetActive(false);
        InvokeRepeating("Pour", pourStart, pourInterval);
    }

    // Update is called once per frame
    void Pour(){
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }else{
            gameObject.SetActive(true);
        }
    }
}
