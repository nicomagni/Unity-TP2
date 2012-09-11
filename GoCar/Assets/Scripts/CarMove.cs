using UnityEngine;
using System.Collections.Generic;

public class CarMove : MonoBehaviour {

	public GameObject shield;
	public int rotDrag = 10;
	public WheelCollider wheelColliderLeftRear;
	public WheelCollider wheelColliderRightRear;
	public WheelCollider wheelColliderLeftFront;
	public WheelCollider wheelColliderRightFront;
	
	public Transform wheelLeftRear;
	public Transform wheelRightRear;
	public Transform wheelLeftFront;
	public Transform wheelRightFront;
	
	
	public float steer_max = 20F;
	public float motor_max = 10F;
	public float brake_max = 100F;
	
	public int levelsRecordLimit = 1;
	public float deltaMiliSecondsRecord = 0.01F;
	public bool shouldSavePath = false;
	public bool shouldReplay = false;
	private float lastPathMilisencods = 0F;
	
	private float steer = 0F;
	private float motor = 0F;
	private float brake = 0F;
	private float back = 0F;
	private float forward = 0F;
	private float speed = 0F;
	private bool reverse;
	private float localEulerAngles;
	Vector3 aux;
	public int nextPoint = 0;
	
	
	private List<Transform> levelsPahtList;
	private List<Vector3> currentPositionPathList;
	private List<Quaternion> currentRotatePathList;
	
	void Start() {
		levelsPahtList = new List<Transform>();
		currentPositionPathList = new List<Vector3>();
		currentRotatePathList = new List<Quaternion>();
		lastPathMilisencods = Time.time;
		rigidbody.centerOfMass = new Vector3(0,-0.5F,0);
	}
	
	void FixedUpdate () {
		//	updateMovement();
		speed = rigidbody.velocity.sqrMagnitude;
			
		steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
		back = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
		if(Input.GetKeyDown(KeyCode.G)){
			shouldSavePath = !shouldSavePath;
		}
		if(Input.GetKeyDown(KeyCode.R)){
			shouldReplay = !shouldReplay;
		}
	 	if(shouldReplay){
			if(nextPoint < currentPositionPathList.Count){
				Vector3 currentPosition = currentPositionPathList[nextPoint++];
				Quaternion currentRotation = currentRotatePathList[nextPoint++];
				
				transform.position = currentPosition;
				transform.rotation = currentRotation;
			}
		}else{
			if(speed < 0.1) {
				if(back > 0) { reverse = true; }
				if(forward > 0) { reverse = false; }
			}
			
			if(reverse) {
				motor = -1 * back;
				brake = forward;
			} else {
				motor = forward;
				brake = back;
			}
			
			wheelColliderLeftRear.motorTorque = motor_max * motor;
			wheelColliderRightRear.motorTorque = motor_max * motor;
			wheelColliderLeftRear.brakeTorque = brake_max * brake;
			wheelColliderRightRear.brakeTorque = brake_max * brake;
			
			wheelColliderLeftFront.steerAngle = steer_max * steer;
			wheelColliderRightFront.steerAngle = steer_max * steer;	
			
			
			wheelRightRear.Rotate(wheelColliderRightRear.rpm * -6F * Time.deltaTime,0,0);
			wheelLeftRear.Rotate(wheelColliderLeftRear.rpm * 6F * Time.deltaTime,0,0);
			
			localEulerAngles = steer_max * steer;
			aux = new Vector3(0,localEulerAngles,0);
			//		wheelRightFront.localEulerAngles = aux;
			
			localEulerAngles= steer_max * steer;
			aux = new Vector3(0,180 + localEulerAngles,0);
			//		wheelLeftFront.localEulerAngles = aux;
			wheelRightFront.Rotate(wheelColliderRightFront.rpm * -6F * Time.deltaTime, 0, 0);
			wheelLeftFront.Rotate(wheelColliderLeftFront.rpm * 6F * Time.deltaTime,0,0);
			
			if(shouldSavePath){
				saveTrasnformation();
			}
			
		}
		
	}
	
	public void saveTrasnformation(){
	
		//print(deltaMiliSecondsRecord);
		float timepased = lastPathMilisencods + deltaMiliSecondsRecord;
		print(timepased);
		if( timepased < Time.time){
			lastPathMilisencods = Time.time;
			currentPositionPathList.Add(transform.position);
			currentRotatePathList.Add(transform.rotation);
		}
	}
	
}
