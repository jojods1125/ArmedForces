using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{


    private Vector3 targetPos; //Current place AI is trying to go
    private Vector3 previousPos; //Where the AI was a second ago, used to check if stuck
    private float previousTime; //Used to check if a second has passed
    private float restStart;
    private float restTime;
    private bool enemyRespawn;
    private float reloadStart;
    private float reloadTime;

    public Player_AI self; //Reference to self
    public Player enemy; //Reference to target
    public Arm_AI frontArm; //Reference to front arm
    public Arm_AI backArm; //Reference to back arm

    public float attackRange; //How close the AI follows
    public float stoppingDist; //What range the AI should stop following
    public float stuckDist; //Distance AI must move in a second before decided its stuck

    public float mapWidth; //Width AI tries to stay in
    public float mapHeight; //Height AI tries to stay in
    public float safeSpeed; //AI cannot move too quickly on edges
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
        rest,
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
        enemyRespawn = false;
        reloadTime = 6;
    }

    // Update is called once per frame
    void Update()
    {
        enemy = GameManager.Instance().localPlayer;

        if (enemy == null)
            return;

    //**
    //Use the shotgun to avoid flying off edges
    //**
        //Don't fly off the Right
        if (self.transform.position.x > mapWidth && self.GetComponent<Rigidbody>().velocity.x > safeSpeed){
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
        if(self.transform.position.y < -mapHeight + 1 && self.GetComponent<Rigidbody>().velocity.y < 0){
            posAdjust(new Vector3(0,-1,0));
            return;
        }


    //**
    //Check if you are trying to move and have not
    //Adjust with shotgun and try again
    //**
        if( Time.time > previousTime + 1){       //If it has been a second
            if(frontArm.getFiring() && state != State.attack && Vector3.Magnitude(self.transform.position - previousPos) < stuckDist){
                Vector3 stuckAngle = (targetPos - self.transform.position);

                if(Mathf.Abs(stuckAngle.x) > Mathf.Abs(stuckAngle.y)){
                    if(self.transform.position.y < 0){
                        posAdjust(new Vector3(-.5f,-1,0));
                    }else{
                        posAdjust(new Vector3(.5f,1,0));
                    }        
                }else{
                    if(self.transform.position.x < 0){
                        posAdjust(new Vector3(-.5f,-1,0));
                    }else{
                        posAdjust(new Vector3(.5f,-1,0));
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
            enemyRespawn = true;
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
    //Check if the other player has respawned
    //If so, rest, then hunt
    //**
        if(enemy.isActiveAndEnabled && enemyRespawn){
            enemyRespawn = false;
            state = State.rest;
            restStart = Time.time;
            restTime = 1;
        }

        if(state == State.rest){
            frontArm.SetFiring(false);
            backArm.SetFiring(false);
            if(Time.time > restStart + restTime){
                state = State.follow;
            }
        }
    //**
    //Reload State
    //**
        if(state == State.reload){
            if(Vector3.Magnitude(targetPos - self.transform.position) > stoppingDist){
                if(Time.time > reloadStart + reloadTime){
                    targetPos = GameManager.Instance().getRandomRespawnPoint();
                    reloadStart = Time.time;
                }
                followTarget();
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
                reloadStart = Time.time;
                state = State.reload;
                return;
            }
            
            //Check where you need to follow
            else if( Vector3.Magnitude(enemy.transform.position - self.transform.position) > attackRange){

                float leftPos = enemy.transform.position.x - attackRange / 2;
                float rightPos = enemy.transform.position.x + attackRange / 2;
                if(self.transform.position.x > enemy.transform.position.x ){
                    if(rightPos < mapWidth - 1){
                        targetPos.x = rightPos;
                    }else{
                        targetPos.x = leftPos;
                    }
                }else{
                    if(leftPos > -mapWidth + 1){
                        targetPos.x = leftPos;
                    }else{
                        targetPos.x = rightPos;
                    }
                }

                /**
                if(enemy.transform.position.x < self.transform.position.x){
                    targetPos.x = enemy.transform.position.x + attackRange / 2;
                } else{
                    targetPos.x = enemy.transform.position.x - attackRange / 2;
                }
                */
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
                reloadStart = Time.time;
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
            if( Vector3.Magnitude(enemy.transform.position - self.transform.position) > attackRange){
                state = State.follow;
                return;
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

