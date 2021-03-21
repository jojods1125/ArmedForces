using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{


    private bool follow; //Is the current state follow?
    private Vector3 targetPos; //Current place AI is trying to go
    private Vector3 previousPos; //Where the AI was a second ago, used to check if stuck
    private float previousTime; //Used to check if a second has passed

    public Player self; //Reference to self
    public Player enemy; //Reference to target
    public Arm frontArm; //Reference to front arm
    public Arm backArm; //Reference to back arm

    public float followDist; //How close the AI follows
    public float stoppingDist; //What range the AI should stop following
    public float stuckDist; //Distance AI must move in a second before decided its stuck

    public float mapWidth; //Width AI tries to stay in
    public float mapHeight; //Height AI tries to stay in
    public float safeSpeed; //AI cannot move too quickly on edges
    

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
        //Don't fly off the Right
        if(self.transform.position.x > mapWidth && self.GetComponent<Rigidbody>().velocity.x > safeSpeed){
            posAdjust(new Vector3(1,0,0));
            return;
        }
        //Don't fly off the Left
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
            //If you haven't move enough in previous second, adjust position with shotgun and try again
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


        //Chase the target
        if(follow){
            //Prepare flamethrowers
            backArm.Switch(backArm.getWeaponA());
            frontArm.Switch(frontArm.getWeaponA());
            //Set target position, left or right of enemy
            if(enemy.transform.position.x < self.transform.position.x){
                targetPos.x = enemy.transform.position.x + followDist;
            } else{
                targetPos.x = enemy.transform.position.x - followDist;
            }
            targetPos.y = enemy.transform.position.y + 1;
            //Find angle to shoot at, slightly up to account for gravity
            Vector3 targetAngle = Vector3.Normalize(targetPos - self.transform.position);
            targetAngle += new Vector3(0,.1f,0);
            Vector3.Normalize(targetAngle);
            //Aim at target and fire!
            frontArm.Aim(-targetAngle);
            backArm.Aim(-targetAngle);
            frontArm.SetFiring(true);
            backArm.SetFiring(true);
            //If target is within attacking range, stop following
            if( Vector3.Magnitude(targetPos - self.transform.position) < stoppingDist){
                follow = false;
                frontArm.SetFiring(false);
                backArm.SetFiring(false);
            }
        } 
    }

    //Shoot the shotgun in the direction you need to go
    private void posAdjust(Vector3 direction){
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

