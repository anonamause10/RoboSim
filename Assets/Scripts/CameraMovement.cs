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
	private Vector3 prevZoomOutPos;
	private Vector3 zoomRot;
	private Vector3 zoomRotDamp = Vector3.zero;
	private bool transitioning = false;
	private Terrain terrain;
	
	
	public float minTurnAngle = -90.0f;
	public float maxTurnAngle = 0.0f;
	private float rotX;
	private float y;

	private int[,] blocksArray;
	private GameObject cursor;
	private Vector3 realCursorPos;

	public GameObject[] spawnObjects;
	
	private bool zoom; // true is normal, false is for viewing full thing

	void Start ()
	{
	    targetDistance = Vector3.Distance(transform.position, target.transform.position);
		targetDistanceInitial = targetDistance;
		zoomRot = new Vector3(90,0,0);
		zoom = true;
		terrain = GameObject.Find("Floor").GetComponent<Terrain>();
		zoomedOutPos = new Vector3(terrain.terrainData.size.x/2,Mathf.Max(terrain.terrainData.size.x,terrain.terrainData.size.z),terrain.terrainData.size.z/2);
		prevZoomOutPos = zoomedOutPos;
		blocksArray = new int[((int)terrain.terrainData.size.x-1),((int)terrain.terrainData.size.z-1)];
		blocksArray = fillArray(blocksArray,-1);
	}
	
	void LateUpdate ()
	{
		if(Input.GetKeyDown(KeyCode.Space)&&!transitioning){
			zoom = !zoom;
			transitioning = true;
			if(!zoom){
				cursor = Instantiate(spawnObjects[0],new Vector3(terrain.terrainData.size.x/2,0.5f,terrain.terrainData.size.z/2),Quaternion.identity);
				Color col = cursor.GetComponent<Renderer>().material.color;
				col.a = 0.5f;
				cursor.GetComponent<Renderer>().material.color = col;
				cursor.GetComponent<Rigidbody>().isKinematic = false;
				cursor.GetComponent<Rigidbody>().useGravity = false;
				cursor.GetComponent<Collider>().isTrigger = true;
				realCursorPos = cursor.transform.position;
			}else{
				prevZoomOutPos = transform.position;
				Destroy(cursor);
			}
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
			zoomedOutPos = prevZoomOutPos;
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
		transform.position = Vector3.SmoothDamp(transform.position, zoomedOutPos, ref zoomPosDamp, 0.1f);
		transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, zoomRot, ref zoomRotDamp, 0.1f);
		Vector3 delVec = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
		zoomedOutPos += Mathf.Max(terrain.terrainData.size.x,terrain.terrainData.size.z)/5*delVec*Time.deltaTime;
		zoomedOutPos += -1*Vector3.up*Input.GetAxis("Mouse ScrollWheel")*20;
		realCursorPos += new Vector3(Input.GetAxis("Mouse X"),0,Input.GetAxis("Mouse Y"));
		if(intedVector(realCursorPos)!=intedVector(cursor.transform.position)){
			cursor.transform.position = intedVector(realCursorPos)+Vector3.up*0.5f*cursor.transform.localScale.y;
		}
		if(Input.GetMouseButton(0)){
			if(blocksArray[(int)cursor.transform.position.x,(int)cursor.transform.position.z]==-1){
				blocksArray[(int)cursor.transform.position.x,(int)cursor.transform.position.z] = 0;
				Instantiate(spawnObjects[0],cursor.transform.position,Quaternion.identity);
			}
		}
	}

	Vector3 intedVector(Vector3 v){
		return new Vector3((int)v.x,(int)v.y,(int)v.z);
	}

	int[,] fillArray(int[,] arr, int val){
		int[,] temp = new int[arr.GetLength(0),arr.GetLength(1)];
		for (int i = 0; i < arr.GetLength(0); i++)
		{
			for (int j = 0; j < arr.GetLength(1); j++)
			{
				temp[i,j] = val;
			}
		}
		return temp;
	}



}