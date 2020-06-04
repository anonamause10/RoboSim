using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanumWheelController : WheelController
{

    public float initAngle = 45;
    // Start is called before the first frame update
    public override void Start()
    {
        wheel = GetComponent<WheelCollider>();
        hit = new WheelHit();
    }

    // Update is called once per frame
    public override void LateUpdate()
    {
        wheel.steerAngle = initAngle;
        bool hitGround = wheel.GetGroundHit(out hit);
        //print(wheel.rpm);
        if(targetForce==0){
            wheel.brakeTorque= brakeForce;
        }else{
            wheel.brakeTorque = 0;
        }
        
        
        force = Mathf.Lerp(force,targetForce,50*Time.deltaTime);
        
        wheel.motorTorque = Mathf.Lerp(2*forceMult*targetForce,0,Mathf.Abs(wheel.rpm)/600);
          
    }

    public override void SetForce(float target){
        targetForce = target;
    }

}
