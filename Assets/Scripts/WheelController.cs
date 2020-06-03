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
    private float targetForce;

    // Start is called before the first frame update
    void Start()
    {
        wheel = GetComponent<WheelCollider>();
        hit = new WheelHit();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bool hitGround = wheel.GetGroundHit(out hit);
        print(hit.forwardSlip);
        if(targetForce==0){
            wheel.brakeTorque= brakeForce;
        }else{
            wheel.brakeTorque = 0;
        }
        
        force = Mathf.Lerp(force,targetForce,50*Time.deltaTime);
        
        wheel.motorTorque = Mathf.Lerp(2*forceMult*targetForce,0,Mathf.Abs(wheel.rpm)/600);
          
    }

    public void SetForce(float target){
        targetForce = target;
    }

}
