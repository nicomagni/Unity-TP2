using System;
using UnityEngine;
using System.Collections;

public class CheckPointScript : MonoBehaviour {
	
	public GameObject nextCheckPoint;
	public int velocity = 100;
	
	public bool isSplash = false;
	
	private LapController lapController;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if(!isSplash) {
			int wp  = Int32.Parse(gameObject.name.Split('_')[1]);
			int car = Int32.Parse(other.gameObject.name.Split('_')[1]);
			
			
			GetLapController().TrackWaypoint(wp, car);
			if(wp == 0) GetLapController().TrackEnd(car);
		}
		CarInteligenceScript carScript = (CarInteligenceScript) other.gameObject.GetComponent(typeof(CarInteligenceScript));
		if(carScript != null) {
			float randLimit = 0.3F;
			
			Vector3 newPosition = new Vector3(UnityEngine.Random.Range(-randLimit,randLimit),0,0);
			print ("New Relative " + newPosition);
			Vector3 newPos = nextCheckPoint.transform.TransformPoint( newPosition);
			print ("New Global " + newPos);
			carScript.moveToCheckPoint(newPos, 30);	
		}

		//Random.Range(30, 40)
	}
	
	private LapController GetLapController(){
		if(lapController == null) {
			lapController = (LapController) GameObject.FindGameObjectWithTag("LapController").GetComponent(typeof(LapController));
		}
		
		return lapController;
	}
	
}