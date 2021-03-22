using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{



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
    public int lowSprayerAmmo;
    public int lowShotgunAmmo;
    public int lowAutoAmmo;
    public int maxSprayerAmmo;
    public int maxShotgunAmmo;
    public int maxAutoAmmo;
    
    
    enum State{
        follow,
        reload,
        attack,
        test
    }


    private State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.follow;
        targetPos = self.transform.position;
        previousPos = self.transform.position;
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
    //**
    //Use the shotgun to avoid flying off edges
    //**
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


    //**
    //Check if you are trying to move and have not
    //Adjust with shotgun and try again
    //**
        if(frontArm.getFiring() && Time.time > previousTime + 1){       
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

    //**
    //Check if the other player is dead
    //If so, find safe ground and wait
    //**
            else if(!enemy.isActiveAndEnabled){
                state = State.follow;
                targetPos = GameManager.Instance().getCloseRespawnPoint(self.transform.position);
                targetPos.y -= 1;
                if(Vector3.Magnitude(targetPos - self.transform.position) < stoppingDist){
                    frontArm.SetFiring(false);
                    backArm.SetFiring(false);
                    return;
                }
            }

    //**
    //Reload State
    //**
        if(state == State.reload){
            if(Vector3.Magnitude(targetPos - self.transform.position) > stoppingDist){
                followTarget();
                return;
            } else{
                frontArm.SetFiring(false);
                backArm.SetFiring(false);
            }
            backArm.Switch(backArm.getWeaponA());
            frontArm.Switch(frontArm.getWeaponA());
            if(backArm.getAmmo(backArm.getWeaponA()) >= maxSprayerAmmo){
                backArm.Switch(backArm.getWeaponB());
                frontArm.Switch(frontArm.getWeaponB());
                backArm.releaseTrigger();
                frontArm.releaseTrigger();
                if(backArm.getAmmo(backArm.getWeaponB()) >= maxShotgunAmmo){
                    backArm.Switch(backArm.getWeaponC());
                    frontArm.Switch(frontArm.getWeaponC());
                    backArm.releaseTrigger();
                    frontArm.releaseTrigger();
                    if(backArm.getAmmo(backArm.getWeaponC()) >= maxAutoAmmo){
                        state = State.follow;
                        return;
                    }
                    
                }
                
            }
    //**
    //Follow State
    //**  
        } else if(state == State.follow){
            followTarget();
            //Check if you need to reload
            if(backArm.getAmmo(backArm.getWeaponA()) <= lowSprayerAmmo || backArm.getAmmo(backArm.getWeaponB()) <= lowShotgunAmmo || backArm.getAmmo(backArm.getWeaponC()) <= lowAutoAmmo){
                targetPos = GameManager.Instance().getCloseRespawnPoint(self.transform.position);
                targetPos.y -= 1;
                state = State.reload;
                return;
            }
            
            //Check if you need to follow
            else if( Vector3.Magnitude(enemy.transform.position - self.transform.position) > stoppingDist + followDist){
                if(enemy.transform.position.x < self.transform.position.x){
                    targetPos.x = enemy.transform.position.x + followDist;
                } else{
                    targetPos.x = enemy.transform.position.x - followDist;
                }
                targetPos.y = enemy.transform.position.y + 1;
            }
            //Check if you need to attack
            else if( Vector3.Magnitude(targetPos - self.transform.position) < stoppingDist){
                state = State.attack;
                return;     
            }
        }
    //**
    //Attack State
    //** 
        else if(state == State.attack){ 
            //Check if you need to reload
            if(backArm.getAmmo(backArm.getWeaponA()) <= lowSprayerAmmo || backArm.getAmmo(backArm.getWeaponB()) <= lowShotgunAmmo || backArm.getAmmo(backArm.getWeaponC()) <= lowAutoAmmo){
                targetPos = GameManager.Instance().getCloseRespawnPoint(self.transform.position);
                targetPos.y -= 1;
                state = State.reload;
                return;
            }
            frontArm.SetFiring(true);
            backArm.SetFiring(true);
            frontArm.Switch(frontArm.getWeaponC());
            backArm.Switch(backArm.getWeaponC());
            Vector3 targetAngle = Vector3.Normalize(enemy.transform.position - self.transform.position);
            targetAngle += new Vector3(0,0,0);
            Vector3.Normalize(targetAngle);
            frontArm.Aim(targetAngle);
            backArm.Aim(targetAngle);
            if( Vector3.Magnitude(enemy.transform.position - self.transform.position) > stoppingDist + followDist){
                state = State.follow;
                return;
            }
        }              
    }

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

    private void followTarget(){
            backArm.Switch(backArm.getWeaponA());
            frontArm.Switch(frontArm.getWeaponA());
            
            Vector3 targetAngle = Vector3.Normalize(targetPos - self.transform.position);
            targetAngle += new Vector3(0,.1f,0);
            Vector3.Normalize(targetAngle);

            frontArm.Aim(-targetAngle);
            backArm.Aim(-targetAngle);
            frontArm.SetFiring(true);
            backArm.SetFiring(true);
    }
}

