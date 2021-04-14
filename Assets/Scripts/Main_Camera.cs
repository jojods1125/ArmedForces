using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Camera : MonoBehaviour
{


public List<Transform> targets;
public Vector3 offset;
private Vector3 velocity;
private Camera cam;
public float smoothTime = .5f;
public float minZoom = 40f;
public float maxZoom = 10f;

public float zoomLimiter = 50f;


private void LateUpdate() {
    if(targets.Count == 0){
        return;
    }
    Move();
    Zoom();
}

void Move(){
    Vector3 centerPoint = GetCenterPoint();
    Vector3 newPosition = centerPoint + offset;
    transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
}

void Zoom(){
    float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
}

float GetGreatestDistance(){
    var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++){
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
}

Vector3 GetCenterPoint(){
    if(targets.Count == 1){
        return targets[0].position;
    }else{
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++){
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}

    // Use this for initialization
    //TAKEN FROM http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
    //Controls aspect ratio to always be the same on any monitor
    void Start () 
    {
        cam = GetComponent<Camera>();

        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {  
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            
            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }


}
