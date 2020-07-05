using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@angryArugula knows what he's doing
public class TeleController : MonoBehaviour
{
    // Start is called before the first frame update
    public RoboController robot;
    public bool controlLock = false;

    void Start()
    {
        robot  = GameObject.Find("Robot").GetComponent<RoboController>();
        controlLock = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(controlLock){
            return;
        }

        robot.setTurnVel((Input.GetKey(KeyCode.J)?-1:(Input.GetKey(KeyCode.L)?1:0)));
        robot.setSideVel(Input.GetAxis("Horizontal"));
        robot.setForwardVel(Input.GetAxis("Vertical"));
        
        //print(robot.getForwardDist());
    }

    public void changeLock(){
        controlLock = !controlLock;
    }
}
