using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanumWheelController : WheelController
{

    public float initAngle = 45;
    // Start is called before the first frame update

    // Update is called once per frame
    public override void LateUpdate()
    {
        wheel.steerAngle = initAngle;
        base.LateUpdate();
          
    }


}
