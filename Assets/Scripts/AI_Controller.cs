using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{


    private bool follow;
    private Vector3 targetPos;

    public Player self;
    public Player enemy;
    public Arm frontArm;
    public Arm backArm;


    // Start is called before the first frame update
    void Start()
    {
        follow = true;
        targetPos = self.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(follow){
            if(enemy.transform.position.x < self.transform.position.x){
                targetPos.x = enemy.transform.position.x + 1;
            } else{
                targetPos.x = enemy.transform.position.x - 1;
            }
            targetPos.y = enemy.transform.position.y + 1;
        }

        Vector3 targetAngle = Vector3.Normalize(targetPos - self.transform.position);
        targetAngle += new Vector3(0,.1f,0);
        Vector3.Normalize(targetAngle);

        frontArm.Aim(-targetAngle);
        backArm.Aim(-targetAngle);
        frontArm.SetFiring(true);
        backArm.SetFiring(true);
    }
}
