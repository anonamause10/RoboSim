using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public WheelCollider wheel;
    public WheelHit hit;
    public float forceMult = 10;
    public float brakeForce = 1f;
    public float force;
    protected float targetForce;
    public float encoderVal;
    public Rigidbody robotBody;

    // Start is called before the first frame update
    public virtual void Start()
    {
        wheel = GetComponent<WheelCollider>();
        hit = new WheelHit();
    }

    // Update is called once per frame
    public virtual void LateUpdate()
    {
        encoderVal+=wheel.rpm*Time.deltaTime;
        bool hitGround = wheel.GetGroundHit(out hit);
        //print(hit.forwardSlip);
        if(targetForce==0){
            wheel.brakeTorque= brakeForce;
        }else{
            wheel.brakeTorque = 0;
        }
        
        
        force = Mathf.Lerp(force,targetForce,50*Time.deltaTime);
        
        wheel.motorTorque = Mathf.Lerp(2*forceMult*targetForce,0,Mathf.Abs(wheel.rpm)/600);
          
    }

    public virtual void SetForce(float target){
        targetForce = target;
    }

    public virtual bool IsGrounded(){
        return wheel.isGrounded;
    }

    public void resetEncoder(){
        encoderVal = 0;
    }

}
