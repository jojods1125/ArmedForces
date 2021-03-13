using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{


    private bool follow;
    private Vector3 targetPos;
    private Vector3 previousPos;
    private float previousTime;

    public Player self;
    public Player enemy;
    public Arm frontArm;
    public Arm backArm;

    public float followDist;
    public float stoppingDist;
    public float stuckDist;

    public float mapWidth;
    public float mapHeight;
    public float safeSpeed;
    

    // Start is called before the first frame update
    void Start()
    {
        follow = true;
        targetPos = self.transform.position;
        previousPos = self.transform.position;
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't fly of the Right
        if(self.transform.position.x > mapWidth && self.GetComponent<Rigidbody>().velocity.x > safeSpeed){
            posAdjust(new Vector3(1,0,0));
            return;
        }
        //Don't fly of the Left
        if(self.transform.position.x < -mapWidth && self.GetComponent<Rigidbody>().velocity.x < safeSpeed){
            posAdjust(new Vector3(-1,0,0));
            return;
        }
        //Don't fly off the Top
        if(self.transform.position.y > mapHeight && self.GetComponent<Rigidbody>().velocity.y > safeSpeed){
            posAdjust(new Vector3(0,1,0));
            return;
        }
        //Don't fly off the bottom
        if(self.transform.position.y < -mapHeight + 1){
            posAdjust(new Vector3(0,-1,0));
            return;
        }


//Check if you are stuck
            if(follow && Time.time > previousTime + 1){       
                if(Vector3.Magnitude(self.transform.position - previousPos) < stuckDist){
                    Vector3 stuckAngle = Vector3.Normalize(targetPos - self.transform.position);
                    if(Mathf.Abs(stuckAngle.x) > Mathf.Abs(stuckAngle.y)){
                        if(self.transform.position.y < 0){
                            posAdjust(new Vector3(0,-1,0));
                        }else{
                            posAdjust(new Vector3(0,1,0));
                        }        
                    }else{
                        if(self.transform.position.x < 0){
                            posAdjust(new Vector3(-1,0,0));
                        }else{
                            posAdjust(new Vector3(1,0,0));
                        }
                    }
                    return;
                }
                previousTime = Time.time;
                previousPos = self.transform.position;
            }



        //Check if you need to chase
        if( Vector3.Magnitude(enemy.transform.position - self.transform.position) > stoppingDist + followDist){
            follow = true;
        }       


        if(follow){
            
            backArm.Switch(backArm.getWeaponA());
            frontArm.Switch(frontArm.getWeaponA());
            if(enemy.transform.position.x < self.transform.position.x){
                targetPos.x = enemy.transform.position.x + followDist;
            } else{
                targetPos.x = enemy.transform.position.x - followDist;
            }
            targetPos.y = enemy.transform.position.y + 1;
            Vector3 targetAngle = Vector3.Normalize(targetPos - self.transform.position);
            targetAngle += new Vector3(0,.1f,0);
            Vector3.Normalize(targetAngle);

            frontArm.Aim(-targetAngle);
            backArm.Aim(-targetAngle);
            frontArm.SetFiring(true);
            backArm.SetFiring(true);
            if( Vector3.Magnitude(targetPos - self.transform.position) < stoppingDist){
                follow = false;
                frontArm.SetFiring(false);
                backArm.SetFiring(false);
            }
        } 
    }

    private void posAdjust(Vector3 direction){
        print("1");
            backArm.Switch(backArm.getWeaponB());
            backArm.Aim(direction);
            backArm.SetFiring(true);
            backArm.releaseTrigger();

            frontArm.Switch(frontArm.getWeaponB());
            frontArm.Aim(direction);
            frontArm.SetFiring(true);
            frontArm.releaseTrigger();

    }
}

