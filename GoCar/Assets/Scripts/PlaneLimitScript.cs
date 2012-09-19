using UnityEngine;
using System.Collections;

public class PlaneLimitScript : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		
		CarMove userCarScript = (CarMove) other.gameObject.GetComponent(typeof(CarMove));	
		if(userCarScript){
			userCarScript.spawnCarAtLastPosition();	
		}
		
	}
	
}
