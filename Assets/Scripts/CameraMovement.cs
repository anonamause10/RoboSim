using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public class CameraMovement : MonoBehaviour {

	public float turnSpeed = 4.0f;
 
	public GameObject target;
	private float targetDistance;
	float targetDistanceInitial;
	float speedSmoothVelocity = 1f;
	public Vector3 zoomedOutPos;
	private Vector3 zoomPosDamp = Vector3.zero;
	private Vector3 prevZoomOutPos;
	public Vector3 zoomRot;
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
	public bool mouseOverButton = false;
	public bool clickedToCursorMode = false;
	public Button modifyFieldButton;
	public Dropdown cameraModeChooser;
	
	public bool zoom; // true is normal, false is for viewing full thing

	public enum CameraMode
	{
		FirstPerson, Free, LockToRobotForward, ZoomedOut, TopDown
	}

	public CameraMode camMode = CameraMode.Free;

	void Start ()
	{
	    targetDistance = Vector3.Distance(transform.position, target.transform.position);
		targetDistanceInitial = targetDistance;
		zoomRot = new Vector3(90,0,0);
		zoom = true;
		terrain = GameObject.Find("Floor").GetComponent<Terrain>();
		zoomedOutPos = new Vector3(terrain.terrainData.size.x/2,Mathf.Max(terrain.terrainData.size.x,terrain.terrainData.size.z),terrain.terrainData.size.z/2);
		prevZoomOutPos = zoomedOutPos;
		blocksArray = new int[((int)terrain.terrainData.size.x),((int)terrain.terrainData.size.z)];
		blocksArray = fillArray(blocksArray,-1);
		mouseOverButton = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		clickedToCursorMode = false;
		camMode = CameraMode.Free;
	}
	
	void LateUpdate ()
	{
		transform.eulerAngles = modEulers(transform.eulerAngles);
		//zoomRot = modEulers(zoomRot);
		if(transitioning){
			transition();
			return;
		}
		if(zoom&&camMode!=CameraMode.ZoomedOut){
			Move();
		}else{
			zoomOutCam();
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if(Input.GetMouseButtonDown(0)&&!mouseOverButton){
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

	}

	void Move(){
	    // get the mouse inputs
		if(Cursor.lockState == CursorLockMode.None){
			switch (camMode)
			{
				case CameraMode.FirstPerson:
					transform.eulerAngles = target.transform.eulerAngles;
					transform.position = target.transform.position+target.transform.forward;
					break;
				case CameraMode.Free:
					transform.position = (target.transform.position) - (transform.forward * targetDistance);
					break;
				case CameraMode.LockToRobotForward:
					transform.position = (target.transform.position) - (transform.forward * targetDistance);
					break;
				case CameraMode.ZoomedOut:
					break;
				case CameraMode.TopDown:
					transform.position = target.transform.position + Vector3.up*5;
					transform.eulerAngles = new Vector3(90,0,0);
					break;
				default:
					transform.position = (target.transform.position) - (transform.forward * targetDistance);
					break;
			}

			return;
		}
	    y = Input.GetAxis("Mouse X") * turnSpeed;
	    rotX += Input.GetAxis("Mouse Y") * turnSpeed;
	
	    // clamp the vertical rotation
	    rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
		targetDistance = Mathf.SmoothDamp (targetDistance, targetDistanceInitial, ref speedSmoothVelocity, 0.1f);
	
	    switch (camMode)
		{
			case CameraMode.FirstPerson:
				transform.eulerAngles = target.transform.eulerAngles;
				transform.position = target.transform.position+target.transform.forward;
				break;
			case CameraMode.Free:
				transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
				transform.position = (target.transform.position) - (transform.forward * targetDistance);
				break;
			case CameraMode.LockToRobotForward:
				transform.eulerAngles = new Vector3(-rotX, target.transform.eulerAngles.y, 0);
				transform.position = (target.transform.position) - (transform.forward * targetDistance);
				break;
			case CameraMode.ZoomedOut:
				break;
			case CameraMode.TopDown:
				transform.position = target.transform.position + Vector3.up*5;
				transform.eulerAngles = new Vector3(90,0,0);
				break;
			default:
				transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
				transform.position = (target.transform.position) - (transform.forward * targetDistance);
				break;
		}

	}

	void transition(){
		transitioning = true;
		switch (camMode)
		{
			case CameraMode.FirstPerson:
				zoomRot = target.transform.eulerAngles;
				zoomedOutPos = target.transform.position+target.transform.forward;
				break;
			case CameraMode.Free:
				zoomedOutPos = target.transform.position+(Vector3.forward*targetDistanceInitial);
				zoomRot = new Vector3(0,180,0);
				break;
			case CameraMode.LockToRobotForward:
				zoomRot = target.transform.eulerAngles;
				zoomedOutPos = target.transform.position-target.transform.forward*targetDistanceInitial;
				break;
			case CameraMode.ZoomedOut:
				zoomedOutPos = prevZoomOutPos;
				zoomRot = new Vector3(90,0,0);
				break;
			case CameraMode.TopDown:
				zoomedOutPos = target.transform.position + Vector3.up*5;
				zoomRot = new Vector3(90,0,0);
				break;
			default:
				zoomedOutPos = target.transform.position+(Vector3.forward*targetDistanceInitial);
				zoomRot = new Vector3(0,180,0);
				break;
		}
		transform.position = Vector3.SmoothDamp(transform.position, zoomedOutPos, ref zoomPosDamp, 0.3f);
		transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, zoomRot, ref zoomRotDamp, 0.3f);
		if((transform.position - zoomedOutPos).magnitude<0.004f){
			transitioning = false;
			if(camMode == CameraMode.Free||camMode == CameraMode.LockToRobotForward){
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
		zoomedOutPos += Vector3.up*(Input.GetKey(KeyCode.Q)?-1:(Input.GetKey(KeyCode.E)?1:0))*20*Time.deltaTime;
		if(zoomedOutPos.y<0.5f){
			zoomedOutPos += 1*Vector3.up*Input.GetAxis("Mouse ScrollWheel")*20;
			zoomedOutPos -= Vector3.up*(Input.GetKey(KeyCode.Q)?-1:(Input.GetKey(KeyCode.E)?1:0))*20*Time.deltaTime;
		}
		if(intedVector(realCursorPos)!=intedVector(cursor.transform.position)){
			cursor.transform.position = intedVector(realCursorPos)+Vector3.up*0.5f*cursor.transform.localScale.y;
		}
		if(Input.GetMouseButtonDown(0)&&(Cursor.lockState == CursorLockMode.None)){
			clickedToCursorMode = true;
		}
		if(Input.GetMouseButtonUp(0)&&(Cursor.lockState == CursorLockMode.Locked)){
			clickedToCursorMode = false;
		}
		if(Cursor.lockState == CursorLockMode.None){
			return;
		}
		realCursorPos += new Vector3(Input.GetAxis("Mouse X"),0,Input.GetAxis("Mouse Y"));
		if((int)realCursorPos.x<1||(int)realCursorPos.x>terrain.terrainData.size.x-1||(int)realCursorPos.z<1||(int)realCursorPos.z>terrain.terrainData.size.z-1){
			realCursorPos -= new Vector3(Input.GetAxis("Mouse X"),0,Input.GetAxis("Mouse Y"));
		}
		if(Input.GetMouseButton(0)&&!mouseOverButton&&!clickedToCursorMode){
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

	public void clearBlocks(){
		GameObject[] obstacles;
 
 		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		for (int i = 0; i < obstacles.Length; i++)
		{
			Destroy(obstacles[i]);
		}
	}

	public void switchCamPos(){
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
			cursor.tag = "Cursor";
			realCursorPos = cursor.transform.position;
			modifyFieldButton.transform.Find("Text").gameObject.GetComponent<Text>().text = "Back to robot";
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			camMode = CameraMode.ZoomedOut;
		}else{
			prevZoomOutPos = transform.position;
			Destroy(cursor);
			modifyFieldButton.transform.Find("Text").gameObject.GetComponent<Text>().text = "Place Obstacles";
			setCameraMode(0);
		}
	}

	public void setCameraMode(string mode){
		transitioning = true;
		CameraMode cameraMode = CameraMode.Free;
		switch (mode)
		{
			case "Free":
				cameraMode = CameraMode.Free;
				break;
			case "FirstPerson":
				cameraMode = CameraMode.FirstPerson;
				break;
			case "LockToRobotForward":
				cameraMode = CameraMode.LockToRobotForward;
				break;
			case "ZoomedOut":
				cameraMode = CameraMode.ZoomedOut;
				break;
			case "TopDown":
				cameraMode = CameraMode.TopDown;
				break;
			default:
				break;
		}
		camMode = cameraMode;
	}

	public void setCameraMode(int mode){
		if(!zoom){
			return;
		}
		transitioning = true;
		mode = cameraModeChooser.value;
		CameraMode cameraMode = camMode;
		switch (mode)
		{
			case 0:
				cameraMode = CameraMode.Free;
				break;
			case 2:
				cameraMode = CameraMode.FirstPerson;
				break;
			case 1:
				cameraMode = CameraMode.LockToRobotForward;
				break;
			case 3:
				cameraMode = CameraMode.TopDown;
				break;
			default:
				break;
		}
		camMode = cameraMode;
	}

	public void setMouseOverTrue(){
		mouseOverButton = true;
	}

	public void setMouseOverFalse(){
		mouseOverButton = false;
	}

	public static Vector3 modEulers(Vector3 v){
		return new Vector3(v.x%360,v.y%360,v.z%360);
	}



}