using UnityEngine;
using System.Collections;

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
	
	private float steer = 0F;
	private float motor = 0F;
	private float brake = 0F;
	private float back = 0F;
	private float forward = 0F;
	private float speed = 0F;
	private bool reverse;
	private float localEulerAngles;
	Vector3 aux;
	
	
	public ParticleSystem mainThrust;
	public float movementForce = 12;
	
	
	private static float crashLimit = 5F;
	private float lastCrashed;
	
	void Start() {
		lastCrashed = Time.timeSinceLevelLoad;
		rigidbody.centerOfMass = new Vector3(0,-0.5F,0);
	}
	
	void FixedUpdate () {
	//	updateMovement();
	speed = rigidbody.velocity.sqrMagnitude;
		
	steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
	forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
	back = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
 
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
		
		
		

	}
	
	private void updateMovement(){
		float movement 	= Input.GetAxis("Vertical");
		float rotation 	= Input.GetAxis("Horizontal");
		//bool isBreaking = Input.GetButton("Break");
		
		if(movement > 0){
			//rigidbody.AddTorque(rigidbody.angularVelocity * (breakForce));
			rigidbody.AddRelativeForce(0,0,movementForce);
			//mainThrust.Emit(0);
		} else if(movement < 0) {
			//rigidbody.AddTorque(rigidbody.angularVelocity * (-breakForce));
			rigidbody.AddRelativeForce(0,0,-movementForce);
			//mainThrust.Emit(-1);
		}
		print(rigidbody.velocity);
		if(rotation > 0){
			//rigidbody.AddRelativeForce(movementForce,0,0);
			
			rigidbody.AddForceAtPosition(new Vector3(-rigidbody.velocity.sqrMagnitude / rotDrag,0,0),new Vector3(5,5,30),ForceMode.Force);
		}else if(rotation < 0){
			//rigidbody.AddRelativeForce(-movementForce,0,0);
			rigidbody.AddForceAtPosition(new Vector3(rigidbody.velocity.sqrMagnitude / rotDrag,0,0),new Vector3(5,5,30),ForceMode.Force);
		}
		
//		if(isBreaking){
		//	rigidbody.AddForce(rigidbody.velocity * (-breakForce));
		//	rigidbody.AddTorque(rigidbody.angularVelocity * (-breakForce));
		//}
	}
	
	

	
}
