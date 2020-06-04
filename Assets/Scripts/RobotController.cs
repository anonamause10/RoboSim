using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public WheelController fRWheel, bRWheel, fLWheel, bLWheel;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(body.velocity);
        /*
        float drive = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");
        float leftPower = Mathf.Clamp(drive + turn, -1.0f, 1.0f);
        float rightPower = Mathf.Clamp(drive - turn, -1.0f, 1.0f);
        */
        Vector2 driveVec = new Vector2(-1*Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        float robotAngle = Mathf.Atan2(driveVec.y, driveVec.x) - Mathf.PI/4;
        float driveRad = driveVec.magnitude;

        print(body.angularVelocity);
        float v1 = Mathf.Cos(robotAngle) * driveRad;
        float v2 = Mathf.Sin(robotAngle) * driveRad;
        float v3 = Mathf.Sin(robotAngle) * driveRad;
        float v4 = Mathf.Cos(robotAngle) * driveRad;
        if(driveRad==0){
            body.drag = 15;
            body.angularDrag = 15;
        }else{
            body.drag = 0;
            body.angularDrag = 0;
        }
        
        fLWheel.SetForce(v1);
        fRWheel.SetForce(v2);
        bLWheel.SetForce(v3);
        bRWheel.SetForce(v4);

    }
}
