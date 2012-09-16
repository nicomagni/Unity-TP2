using UnityEngine;
using System.Collections;

public class CheckPointScript : MonoBehaviour {
	
	public GameObject nextCheckPoint;
	public int velocity = 100;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		CarInteligenceScript carScript = (CarInteligenceScript) other.gameObject.GetComponent(typeof(CarInteligenceScript));
		float randomPosition = Random.Range(-5.0F,5.0F);
		Vector3 newPosition = new Vector3(randomPosition,0,0);
		carScript.moveToCheckPoint(nextCheckPoint.transform.position + newPosition, Random.Range(20, 40)); 
	}
}