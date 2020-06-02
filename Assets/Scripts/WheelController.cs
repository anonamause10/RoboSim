using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public Rigidbody robotBody;
    public bool isGrounded = false;
    private float forceMult = 5;
    public float force;
    private float targetForce;

    // Start is called before the first frame update
    void Start()
    {
        robotBody = transform.parent.parent.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        force = Mathf.Lerp(force,targetForce,50*Time.deltaTime);
        Vector3 forceDir = Vector3.ProjectOnPlane(transform.forward,Vector3.up).normalized;
        if(isGrounded){
            robotBody.AddForceAtPosition(forceDir*force*forceMult, transform.position);
        }
        print(isGrounded);
    }

    public void SetForce(float target){
        targetForce = target;
    }

    //make sure u replace "floor" with your gameobject name.on which player is standing
    void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.name == "Floor")
        {
            isGrounded = true;
        }
    }
    
    //consider when character is jumping .. it will exit collision.
    void OnCollisionExit(Collision theCollision)
    {
        if (theCollision.gameObject.name == "Floor")
        {
            isGrounded = false;
        }
    }
}
