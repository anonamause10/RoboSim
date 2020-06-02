using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public WheelController fRWheel, bRWheel, fLWheel, bLWheel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float power = Input.GetAxis("Vertical");
        fRWheel.SetForce(power);
        fLWheel.SetForce(power);
        bLWheel.SetForce(power);
        bRWheel.SetForce(power);

    }
}
