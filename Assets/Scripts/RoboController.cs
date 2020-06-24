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
    private float actualTurnVel;

    private Vector3 velVec;
    private Vector3 actualVelVec;
    private Vector3 velDampVec = Vector3.zero;
    private Rigidbody rb;
    private float turnDampVel = 0f;

    [SerializeField] private Transform distSensorT;

    private RaycastHit forwardCast;
    private RaycastHit backCast;
    private RaycastHit leftCast;
    private RaycastHit rightCast;

    void Start()
    {
        maxforwardVel = 8;
        maxSideVel = 8;
        maxTurnVel = 90;
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        velVec.Set(sideVel, 0, forwardVel);
        actualVelVec = Vector3.SmoothDamp(actualVelVec,velVec, ref velDampVec, 0.1f);
        actualTurnVel = Mathf.SmoothDamp(actualTurnVel, turnVel, ref turnDampVel, 0.1f);

        transform.Translate(actualVelVec * Time.deltaTime);
        transform.Rotate(Vector3.up*actualTurnVel*Time.deltaTime);

        Physics.Raycast(distSensorT.position, transform.forward, out forwardCast);
        Physics.Raycast(distSensorT.position, -transform.forward, out backCast);
        Physics.Raycast(distSensorT.position, -transform.right, out leftCast);
        Physics.Raycast(distSensorT.position, transform.right, out rightCast);
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

    public Vector3 getTrueVelocity(){
        return rb.velocity;
    }

    public float getTrueAngularVelocity(){
        return rb.angularVelocity.y;
    }

    public float getGyroAngle(){
        return transform.eulerAngles.y;
    }

    public float getForwardDist(){
        return forwardCast.distance;
    }

    public float getBackDist(){
        return backCast.distance;
    }

    public float getLeftDist(){
        return leftCast.distance;
    }

    public float getRightDist(){
        return rightCast.distance;
    }
    
}
