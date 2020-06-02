using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public float speed = 30;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = Vector3.ProjectOnPlane(Camera.main.transform.forward,Vector3.up).normalized;
        transform.Translate(speed*moveDir*Input.GetAxis("Vertical")*Time.deltaTime,Space.World);
    }
}
