using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboController : MonoBehaviour
{
    // Start is called before the first frame update

    private float maxforwardVel;
    private float maxSideVel;
    private float maxTurnVel;
    private float forwardVel;
    private float sideVel;
    private float turnVel;

    private Vector3 velVec;

    void Start()
    {
        maxforwardVel = 5;
        maxSideVel = 4;
        maxTurnVel = 90;
    }

    // Update is called once per frame
    void Update()
    {
        velVec.Set(sideVel, 0, forwardVel);
        transform.Translate(velVec * Time.deltaTime);
        transform.Rotate(Vector3.up*turnVel*Time.deltaTime);
    }

    public float getMaxforwardVel()
    {
        return this.maxforwardVel;
    }

    public void setMaxforwardVel(float maxforwardVel)
    {
        this.maxforwardVel = maxforwardVel;
    }

    public float getMaxSideVel()
    {
        return this.maxSideVel;
    }

    public void setMaxSideVel(float maxSideVel)
    {
        this.maxSideVel = maxSideVel;
    }

    public float getMaxTurnVel()
    {
        return this.maxTurnVel;
    }

    public void setMaxTurnVel(float maxTurnVel)
    {
        this.maxTurnVel = maxTurnVel;
    }

    public float getForwardVel()
    {
        return this.forwardVel;
    }

    public void setForwardVel(float forwardVel)
    {
        forwardVel = Mathf.Clamp(forwardVel, -1, 1);
        this.forwardVel = forwardVel*maxforwardVel;
    }

    public float getSideVel()
    {
        return this.sideVel;
    }

    public void setSideVel(float sideVel)
    {
        sideVel = Mathf.Clamp(sideVel, -1, 1);
        this.sideVel = sideVel * maxSideVel;
    }

    public float getTurnVel()
    {
        return this.turnVel;
    }

    public void setTurnVel(float turnVel)
    {
        turnVel = Mathf.Clamp(turnVel, -1,1);
        this.turnVel = turnVel*maxTurnVel;
    }


    
}
