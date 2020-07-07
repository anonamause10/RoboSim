using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@angryArugula knows what he's doing
public class TeleController : MonoBehaviour
{
    // Start is called before the first frame update
    public RoboController robot;
    public bool controlLock = false;
    private CameraMovement camScript;

    void Start()
    {
        robot  = GameObject.Find("Robot").GetComponent<RoboController>();
        controlLock = false;
        camScript = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if(!camScript.zoom||robot.isOnSide()){
            controlLock = true;
        }else{
            controlLock = false;
        }

        if(controlLock){
            return;
        }

        //robot.setTurnVel((Input.GetKey(KeyCode.Q)?-1:(Input.GetKey(KeyCode.E)?1:0)));
        robot.setTurnVel(Input.GetAxis("Horizontal"));
        robot.setForwardVel(Input.GetAxis("Vertical"));
        
        //print(robot.getForwardDist());
    }

    public void changeLock(){
        controlLock = !controlLock;
    }
}
