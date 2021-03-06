using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarInteligenceScript : MonoBehaviour {
	
	public GameObject firstCheck;
	private Vector3 nextPosition;
	
	public bool isSplash = false;
	
	private LapController lapController;
	
	public List<Vector3> arrayListPath;
	public int maxPoints = 100;
	public int currentStep = 0;
	public float speedFactor = 1;
	
	// Use this for initialization
	void Start () {
		
		moveToCheckPoint(firstCheck.transform.position, 2);
		
	}

    void FixedUpdate() {
		if(!isSplash && GetLapController().HasFinished()) {
			return;
		}
		
		if(!isSplash && !GetLapController().hasStarted()) {
			return;
		}
		
		
		if(currentStep < arrayListPath.Count){
			if(currentStep + 1 < arrayListPath.Count){
				Vector3 newVectorDirection = arrayListPath[currentStep + 1] - transform.position;
				//print(Vector3.Angle(newVectorDirection,transform.position.normalized));

				transform.LookAt(newVectorDirection.normalized + transform.position);
			}
			
			//transform.position = arrayListPath[currentStep++];
			rigidbody.MovePosition(arrayListPath[currentStep++]);
			
		}
    }
	
	public void moveToCheckPoint(Vector3 nextCheckPosition, int velocity) {
		float distance = Vector3.Distance(nextPosition, nextCheckPosition)/100;
		nextPosition = nextCheckPosition;
		maxPoints = (int) (distance * velocity * speedFactor);
		currentStep = 0;
		//Vector3 directionVector = nextPosition - transform.position;
		arrayListPath = new List<Vector3>();
		for(int i = 0 ; i < maxPoints ; i ++){
			arrayListPath.Add(Vector3.Lerp(transform.position, nextPosition, (float)(i+1) / (float)maxPoints));
			//GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//	sphere.transform.position = newVectorDirection.normalized + transform.position;
		}
	}
	
	 void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
			rigidbody.AddForce(contact.normal);
			//print (contact.normal);
        }
            
        
    }

	private LapController GetLapController(){
		if(lapController == null) {
			lapController = (LapController) GameObject.FindGameObjectWithTag("LapController").GetComponent(typeof(LapController));
		}
		
		return lapController;
	}
}