using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

public class CarMove : MonoBehaviour {

	private LapController lapController;
	
	public GameObject shield;
	public int rotDrag = 10;
	public WheelCollider wheelColliderLeftRear;
	public WheelCollider wheelColliderRightRear;
	public WheelCollider wheelColliderLeftFront;
	public WheelCollider wheelColliderRightFront;
	public GameObject mass_center;
	public Transform wheelLeftRear;
	public Transform wheelRightRear;
	public Transform wheelLeftFront;
	public Transform wheelRightFront;
	
	
	public float steer_max = 20F;
	public float motor_max = 10F;
	public float brake_max = 100F;
	
	public int levelsRecordLimit = 1;
	public float deltaMiliSecondsRecord = 3F;
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
	public int choque= 2;
	public int nextPoint = 0;
	public int step = 1;
	
	public Transform lastTransform;
	
	public List<Transform> levelsPahtList;
	private List<Vector3> currentPositionPathList;
	private List<Quaternion> currentRotatePathList;
	
	void Start() {
		levelsPahtList = new List<Transform>();
		currentPositionPathList = new List<Vector3>();
		currentRotatePathList = new List<Quaternion>();
		lastPathMilisencods = Time.time;
		rigidbody.centerOfMass = new Vector3(0,-0.1F,0.3F);
	}
	
	void FixedUpdate () {
		if(GetLapController().HasFinished() || !GetLapController().hasStarted()) {
			wheelColliderLeftRear.motorTorque = 0;
			wheelColliderRightRear.motorTorque = 0;
			
			return;
		}
		
		//	updateMovement();
		speed = rigidbody.velocity.sqrMagnitude;
			
		steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
		back = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
		
	//	if(Input.GetKeyDown(KeyCode.G)){
	//		shouldSavePath = !shouldSavePath;
	//	}
		
		if(Input.GetKeyUp(KeyCode.Space)){
			spawnCarAtLastPosition();
		}

		if(Input.GetKeyDown(KeyCode.R)){
			shouldReplay = !shouldReplay;
		}
	 	if(shouldReplay){
			if(nextPoint < currentPositionPathList.Count - step){
				nextPoint += step;
				Vector3 currentPosition = currentPositionPathList[nextPoint];
				Quaternion currentRotation = currentRotatePathList[nextPoint];
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
			aux = new Vector3(localEulerAngles,localEulerAngles,0);
			wheelLeftFront.localEulerAngles = aux;
			
			
			//wheelRightFront.Rotate(wheelColliderRightFront.rpm * -6F * Time.deltaTime, 0, 0);
			//wheelRightFront.Rotate(wheelColliderRightFront.rpm * -6F * Time.deltaTime, localEulerAngles-wheelRightFront.transform.rotation.y, 0);
			localEulerAngles= steer_max * steer;
			aux = new Vector3(0,180 + localEulerAngles,0);
			wheelRightFront.localEulerAngles = aux;
			
			//wheelLeftFront.Rotate(wheelColliderLeftFront.rpm * 6F * Time.deltaTime,0,0);
			//wheelLeftFront.Rotate(wheelColliderLeftFront.rpm * 6F * Time.deltaTime,180 + localEulerAngles,0);
			
			
			saveTrasnformation();
			
			
		}
		
	}
	
	public void saveTrasnformation(){
	
		//print(deltaMiliSecondsRecord);
		float timepased = lastPathMilisencods + deltaMiliSecondsRecord;
		
		if( timepased < Time.time){
			
			lastPathMilisencods = Time.time;
			currentPositionPathList.Add(transform.position);
			currentRotatePathList.Add(transform.rotation);
			
			
		}
	}
	
	public void spawnCarAtLastPosition(){
		if(currentRotatePathList.Count == 0){
				transform.localPosition=new Vector3(0,0,0);
			}else{
				Vector3 currentPosition = currentPositionPathList[currentPositionPathList.Count - 1];
				Quaternion currentRotation = currentRotatePathList[currentRotatePathList.Count - 1];
				transform.position = currentPosition;
				transform.rotation = currentRotation;
			}
	}

	public float getCurrentSpeed() {
		return rigidbody.velocity.sqrMagnitude;
	}

	void OnCollisionEnter(Collision collision) {
		CarInteligenceScript carScript = (CarInteligenceScript) collision.collider.gameObject.GetComponent(typeof(CarInteligenceScript));
		if(carScript){
			foreach (ContactPoint contact in collision.contacts) {
				//   Debug.DrawRay(contact.point, contact.normal, Color.red);
				rigidbody.AddForce(contact.normal * 0.1F * rigidbody.velocity.sqrMagnitude,ForceMode.Impulse);
				
			}
		}
        
        
    }

	private LapController GetLapController(){
		if(lapController == null) {
			lapController = (LapController) GameObject.FindGameObjectWithTag("LapController").GetComponent(typeof(LapController));
		}
		
		return lapController;
	}
	
/*	
	public void save() {
		string path = Application.persistentDataPath + "/trasnformPath.txt";
		Stream stream = File.Open(path, FileMode.Create);
		XMLSerializer bformatter = new XmlSerializer();
	            
		print("Writing Employee Information " +  path);
		
		Quaternion t = new Quaternion(1,2,3,4);	
		bformatter.Serialize(stream, t);
		stream.Close();
		print("ANDTES" + currentRotatePathList.Count);
	}
	
	public void load(){
		string path = Application.persistentDataPath + "/trasnformPath.txt";
		Stream stream = File.Open(path, FileMode.Open);
		BinaryFormatter bformatter = new BinaryFormatter();

		//List<Quaternion> mp = (List<Quaternion>)bformatter.Deserialize(stream);
		Quaternion mp = (Quaternion)bformatter.Deserialize(stream);
		stream.Close();
            
		print("Dspuyes" + mp);
	}
	 */
}
