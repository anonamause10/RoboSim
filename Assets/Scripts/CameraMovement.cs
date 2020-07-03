using UnityEngine;
using System;
using System.Collections;


public class CameraMovement : MonoBehaviour {

	public float turnSpeed = 4.0f;
 
	public GameObject target;
	private float targetDistance;
	float targetDistanceInitial;
	float speedSmoothVelocity = 1f;
	private Vector3 zoomedOutPos;
	private Vector3 zoomPosDamp = Vector3.zero;
	private Vector3 zoomRot;
	private Vector3 zoomRotDamp = Vector3.zero;
	private bool transitioning = false;
	private Terrain terrain;
	
	
	public float minTurnAngle = -90.0f;
	public float maxTurnAngle = 0.0f;
	private float rotX;
	private float y;
	
	private bool zoom; // true is normal, false is for viewing full thing

	void Start ()
	{
	    targetDistance = Vector3.Distance(transform.position, target.transform.position);
		targetDistanceInitial = targetDistance;
		zoomRot = new Vector3(90,0,0);
		zoom = true;
		terrain = GameObject.Find("Floor").GetComponent<Terrain>();
		zoomedOutPos = new Vector3(terrain.terrainData.size.x/2,Mathf.Max(terrain.terrainData.size.x,terrain.terrainData.size.z),terrain.terrainData.size.z/2);
	}
	
	void LateUpdate ()
	{
		zoomedOutPos = new Vector3(terrain.terrainData.size.x/2,Mathf.Max(terrain.terrainData.size.x,terrain.terrainData.size.z),terrain.terrainData.size.z/2);
		if(Input.GetKeyDown(KeyCode.Space)&&!transitioning){
			zoom = !zoom;
			transitioning = true;
		}
		if(transitioning){
			transition();
			return;
		}
		if(zoom){
			Move();
		}else{
			zoomOutCam();
		}

	}

	void Move(){
	    // get the mouse inputs
	    y = Input.GetAxis("Mouse X") * turnSpeed;
	    rotX += Input.GetAxis("Mouse Y") * turnSpeed;
	
	    // clamp the vertical rotation
	    rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
		targetDistance = Mathf.SmoothDamp (targetDistance, targetDistanceInitial, ref speedSmoothVelocity, 0.1f);
	
	    // rotate the camera
	    transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
	
	    // move the camera position
		transform.position = (target.transform.position) - (transform.forward * targetDistance);

	}

	void transition(){
		transitioning = true;
		if(zoom){
			zoomedOutPos = target.transform.position+(Vector3.forward*targetDistanceInitial);
			zoomRot = new Vector3(0,180,0);
		}else{
			zoomedOutPos = new Vector3(terrain.terrainData.size.x/2,Mathf.Max(terrain.terrainData.size.x,terrain.terrainData.size.z),terrain.terrainData.size.z/2);
			zoomRot = new Vector3(90,0,0);
		}
		transform.position = Vector3.SmoothDamp(transform.position, zoomedOutPos, ref zoomPosDamp, 0.5f);
		transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, zoomRot, ref zoomRotDamp, 0.5f);
		if((transform.position - zoomedOutPos).magnitude<0.004f){
			transitioning = false;
			if(zoom){
				rotX = 0;
				y = 0;
			}
		}
	}

	void zoomOutCam(){
		transform.position = Vector3.SmoothDamp(transform.position, zoomedOutPos, ref zoomPosDamp, 0.5f);
		transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, zoomRot, ref zoomRotDamp, 0.5f);
	}

}