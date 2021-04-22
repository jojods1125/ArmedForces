using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Camera : MonoBehaviour
{


public List<GameObject> targets;
public Vector3 offset;
private Vector3 velocity;
private Camera cam;
public float smoothTime = .5f;
public float minZoom = 40f;
public float maxZoom = 10f;
public float zoomLimiter = 50f;
public float mapX;
public float mapY;
private float minX;
private float maxX;
private float minY;
private float maxY;


private void LateUpdate() {
    if(targets.Count == 0){
        return;
    }
    Move();
    Zoom();
}

void Move(){
    Vector3 centerPoint = GetCenterPoint();
    //TAKEN FROM https://answers.unity.com/questions/501893/calculating-2d-camera-bounds.html
        var vertExtent = cam.orthographicSize;    
        var horzExtent = vertExtent * Screen.width / Screen.height;
        maxX = horzExtent - mapX / 2.0f;
        minX = mapX / 2.0f - horzExtent;
        maxY = vertExtent - mapY / 2.0f;
        minY = mapY / 2.0f - vertExtent;
    centerPoint.x = Mathf.Clamp(centerPoint.x, minX, maxX);
    centerPoint.y = Mathf.Clamp(centerPoint.y, minY, maxY);
    Vector3 newPosition = centerPoint + offset;
    
    transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
}

void Zoom(){
    float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
}

float GetGreatestDistance(){
    int count = 0;
    for(int i = 0; i < targets.Count; i++){
        if(!targets[i].GetComponent<Player>().dying){
            count++;
        }
    }
    if(count == 1){
        return maxZoom;
    }
    var bounds = new Bounds(targets[0].GetComponent<Transform>().position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++){
            if(targets[i].GetComponent<Player>().dying ){
                //bounds.Encapsulate(Vector3.zero);
            }else{
                bounds.Encapsulate(targets[i].GetComponent<Transform>().position);
            }
            
        }
        return bounds.size.x;
}

Vector3 GetCenterPoint(){
    int count = 0;
    for(int i = 0; i < targets.Count; i++){
        if(!targets[i].GetComponent<Player>().dying){
            count++;
        }
    }
    if(count == 1){
        for(int i = 0; i < targets.Count; i++){
            if(!targets[i].GetComponent<Player>().dying){
                return targets[i].GetComponent<Transform>().position;
            }
        }
        return targets[0].GetComponent<Transform>().position;
    } else{
        var bounds = new Bounds(targets[0].GetComponent<Transform>().position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++){
            if(targets[i].GetComponent<Player>().dying){
                //bounds.Encapsulate(Vector3.zero);
            }else{
                bounds.Encapsulate(targets[i].GetComponent<Transform>().position);
            }
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
