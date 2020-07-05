using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMover : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    private Vector3 targetPos;
    private Vector3 moveDamp;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        targetPos = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref moveDamp, 0.2f);
    }

    public void switchTargetPos(){
        targetPos = targetPos==startPos?endPos:startPos;
    }
}
