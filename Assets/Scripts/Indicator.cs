using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Material[] materials;
    public GameObject child;
    public Camera cameraObj;

    private int playerID;
    private GameObject player;

    private float adjustedScreenWidth;
    private float adjustedScreenHeight;
    private float adjustedScreenHeightBottom;
    private float adjustedScreenWidthBottom;

    
    private void Start() {
        adjustedScreenWidth = Screen.width * (cameraObj.rect.width + cameraObj.rect.x);
        adjustedScreenHeight = Screen.height * (cameraObj.rect.height + cameraObj.rect.y);
        adjustedScreenHeightBottom = Screen.height - adjustedScreenHeight;
        adjustedScreenWidthBottom = Screen.width - adjustedScreenWidth;
    }
    
    public void SetPlayer(int playerID, GameObject player)
    {
        this.player = player;
        this.playerID = playerID;
        gameObject.GetComponent<MeshRenderer>().material = materials[playerID];
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosOnScreen = cameraObj.WorldToScreenPoint (player.GetComponent<Transform>().position);
        if (onScreen (targetPosOnScreen) || player.GetComponent<Player>().dying) {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            child.SetActive(false);
            return;
        } else{
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            child.SetActive(true);
        }

        Vector3 center = new Vector3 (adjustedScreenWidth / 2f, adjustedScreenHeight / 2f, 0);
        float angle = Mathf.Atan2(targetPosOnScreen.y-center.y, targetPosOnScreen.x-center.x) * Mathf.Rad2Deg;
        float coef;
        if (adjustedScreenWidth > adjustedScreenHeight)
            coef = adjustedScreenWidth / adjustedScreenHeight;
        else
            coef = adjustedScreenHeight / adjustedScreenWidth;

        float degreeRange = 360f / (coef + 1);

        if(angle < 0) angle = angle + 360;
        int edgeLine;
        if(angle < degreeRange / 4f) edgeLine = 0;
        else if (angle < 180 - degreeRange / 4f) edgeLine = 1;
        else if (angle < 180 + degreeRange /  4f) edgeLine = 2;
        else if (angle < 360 - degreeRange / 4f) edgeLine = 3;
        else edgeLine = 0;

        gameObject.GetComponent<Transform>().position = cameraObj.ScreenToWorldPoint(intersect(edgeLine, center, targetPosOnScreen)+new Vector3(0,0,10));
        gameObject.GetComponent<Transform>().eulerAngles = new Vector3 (0, 0, angle);
    }

    bool onScreen(Vector2 input){
        return !(input.x > adjustedScreenWidth || input.x < adjustedScreenWidthBottom || input.y > adjustedScreenHeight || input.y < adjustedScreenHeightBottom);
    }

    Vector3 intersect(int edgeLine, Vector3 line2point1, Vector3 line2point2){
    float[] A1 = {-adjustedScreenHeight, 0, adjustedScreenHeight, 0};
    float[] B1 = {0, -adjustedScreenWidth, 0, adjustedScreenWidth};
    float[] C1 = {-adjustedScreenWidth * adjustedScreenHeight,-adjustedScreenWidth * adjustedScreenHeight,0, 0};

    float A2 = line2point2.y - line2point1.y;
    float B2 = line2point1.x - line2point2.x;
    float C2 = A2 * line2point1.x + B2 * line2point1.y;

    float det = A1[edgeLine] * B2 - A2 * B1[edgeLine];

    Vector3 ret = new Vector3 ((B2 * C1[edgeLine] - B1[edgeLine] * C2) / det, (A1[edgeLine] * C2 - A2 * C1[edgeLine]) / det, 0);
    ret.x = Mathf.Clamp(ret.x, adjustedScreenWidthBottom, adjustedScreenWidth);
    ret.y = Mathf.Clamp(ret.y, adjustedScreenHeightBottom, adjustedScreenHeight);

    return ret;
}
}
