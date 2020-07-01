using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RobotDataHolder
{
    //can be set
    private float maxforwardVel;
    private float maxSideVel;
    private float maxTurnVel;
    private float forwardVel;
    private float sideVel;
    private float turnVel;
    
    //cannot be set
    private float actualTurnVel;
    private Vector3 actualVelVec;
    private float gyroAngle;
    private float forwardDist;
    private float backDist;
    private float leftDist;
    private float rightDist;
    private RoboController controller;

    public string dataText;

    public RobotDataHolder(RoboController cont){
        controller = cont;
        collectValsFromBot();
        dataText = System.IO.File.ReadAllText(@"‪C:\Users\Arjun Pal\Downloads\buffer.txt");
    }

    public void collectValsFromBot(){
        maxforwardVel = controller.getMaxForwardVel();
        maxSideVel = controller.getMaxSideVel();
        maxTurnVel = controller.getMaxTurnVel();
        forwardVel = controller.getForwardVel();
        sideVel = controller.getSideVel();
        turnVel = controller.getTurnVel();
        actualTurnVel = controller.getTrueAngularVelocity();
        actualVelVec = controller.getTrueVelocity();
        gyroAngle = controller.getGyroAngle();
        forwardDist = controller.getForwardDist();
        backDist = controller.getBackDist();
        leftDist = controller.getLeftDist();
        rightDist = controller.getRightDist();
    }

    public void setRobotVals(){
        controller.setMaxForwardVel(5);
        controller.setMaxSideVel(5);
        controller.setMaxTurnVel(90);
        controller.setForwardVel(1);
        controller.setSideVel(1);
        controller.setTurnVel(1);
    }


}
