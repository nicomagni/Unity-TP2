using UnityEngine;
using System.Collections;
using System;

public class TerrainLevel1Script : MonoBehaviour {
	private float drag;
	private float anularDrag;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		CarMove carScript = (CarMove) other.gameObject.GetComponent(typeof(CarMove));
		
		if(carScript != null) {
			drag = other.gameObject.rigidbody.drag;
			anularDrag = other.gameObject.rigidbody.angularDrag;
			other.gameObject.rigidbody.drag = 5;
			other.gameObject.rigidbody.angularDrag = 3;
		}

	}
	
	void OnTriggerExit(Collider other) {
		CarMove carScript = (CarMove) other.gameObject.GetComponent(typeof(CarMove));
		int car = Int32.Parse(other.gameObject.name.Split('_')[1]);
		
		if(carScript != null) {
			other.gameObject.rigidbody.drag = drag;
			other.gameObject.rigidbody.angularDrag = anularDrag;
		}

	}
}
