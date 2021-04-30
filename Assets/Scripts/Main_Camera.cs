using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Camera : MonoBehaviour
{

//List of objects to follow
public List<GameObject> targets;
//Offset from center between objects
public Vector3 offset;
//Speed camera is movie
private Vector3 velocity;
//Camera to be moved
private Camera cam;
//USed to make movemenet smooth
public float smoothTime;
//Minimum zoom, bigger is more zoomed out
public float minZoom;
//Max zoom, smaller is more zoomed in
public float maxZoom;
//This is important I just don't know why
public float zoomLimiter;

//Used for limiting how far the screen can move
private float minX;
private float maxX;
private float minY;
private float maxY;

//Bounds of the map
private float mapLeft;
private float mapRight;
private float mapTop;
private float mapBottom;

//Offset from the killzone to limit camera
public int killZoneVerticalOffset;
public int killZoneHorzOffset;

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

//Returns the center point between all objects being tracked
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



    void Start () 
    {
        //Set the camera
        cam = GetComponent<Camera>();

        //Setup bounds of the map
        mapRight = GameManager.Instance().rightBound.GetComponent<Transform>().position.x - killZoneHorzOffset;
        Debug.Log(mapRight);
        mapLeft = GameManager.Instance().leftBound.GetComponent<Transform>().position.x + killZoneHorzOffset;
        mapBottom = GameManager.Instance().bottomBound.GetComponent<Transform>().position.y + killZoneVerticalOffset + 1;
        mapTop = GameManager.Instance().topBound.GetComponent<Transform>().position.y - killZoneVerticalOffset;

        var vertExtent = cam.orthographicSize;    
        var horzExtent = vertExtent * Screen.width / Screen.height;
        maxX = mapRight - horzExtent;
        minX = mapLeft + horzExtent;
        maxY = mapTop - vertExtent;
        minY = mapBottom + vertExtent;
        

    //TAKEN FROM http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
    //Controls aspect ratio to always be the same on any monitor
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

        // if scaled height is less than current height, add letterbox (black bars across top and bottom)
        if (scaleheight < 1.0f)
        {  
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            
            camera.rect = rect;
        }
        else // add pillarbox (black bars on left and right)
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
