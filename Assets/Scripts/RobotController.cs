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
        print(body.velocity);
        float power = Input.GetAxis("Vertical");
        if(power==0){
            body.drag = 25;
        }else{
            body.drag = 0;
        }
        fRWheel.SetForce(power);
        fLWheel.SetForce(power);
        bLWheel.SetForce(power);
        bRWheel.SetForce(power);

    }
}
