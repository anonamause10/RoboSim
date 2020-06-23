using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleController : MonoBehaviour
{
    // Start is called before the first frame update
    public RoboController robot;

    void Start()
    {
        robot  = GameObject.Find("Robot").GetComponent<RoboController>();
    }

    // Update is called once per frame
    void Update()
    {
        robot.setTurnVel(-0.2f);
        robot.setForwardVel(1);
    }
}
