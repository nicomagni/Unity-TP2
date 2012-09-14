using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarInteligenceScript : MonoBehaviour {
	
	public GameObject firstCheck;
	private Vector3 nextPosition;
	
	public List<Vector3> arrayListPath;
	public int maxPoints = 100;
	public int currentStep = 0;
	
	// Use this for initialization
	void Start () {
		
		moveToCheckPoint(firstCheck.transform.position, 100);
		
	}

    void FixedUpdate() {
		
		if(currentStep < arrayListPath.Count){
			if(currentStep + 1 < arrayListPath.Count){
				Vector3 newVectorDirection = arrayListPath[currentStep + 1] - transform.position;
				print(Vector3.Angle(newVectorDirection,transform.position.normalized));

				transform.LookAt(newVectorDirection.normalized + transform.position);
			}
			
			transform.position = arrayListPath[currentStep++];
		}
    }
	
	public void moveToCheckPoint(Vector3 nextCheckPosition, int velocity) {
		nextPosition = nextCheckPosition;
		maxPoints = velocity;
		currentStep = 0;
		//Vector3 directionVector = nextPosition - transform.position;
		arrayListPath = new List<Vector3>();
		for(int i = 0 ; i < maxPoints ; i ++){
			arrayListPath.Add(Vector3.Lerp(transform.position, nextPosition, (float)(i+1) / (float)maxPoints));
			//GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//	sphere.transform.position = newVectorDirection.normalized + transform.position;
		}
	}
	
	
}