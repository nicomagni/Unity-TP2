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
		float randomPosition = Random.Range(-3.0F,3.0F);
		Vector3 newPosition = new Vector3(randomPosition,0,randomPosition);
		carScript.moveToCheckPoint(nextCheckPoint.transform.position + newPosition, Random.Range(30, 35)); 
	}
}