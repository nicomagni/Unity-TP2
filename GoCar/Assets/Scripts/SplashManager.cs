using UnityEngine;
using System.Collections;

public class SplashManager : MonoBehaviour {
	
	public GUISkin mySkin;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown){
			Application.LoadLevel("roundLevel");
		}	
	}


	void OnGUI () {
		GUIStyle style = new GUIStyle();
		style.fontSize = 60;
		style.font = mySkin.font;
		style.normal.textColor = Color.white;
		style.alignment = TextAnchor.MiddleCenter;
	
		GUI.Label(new Rect((Screen.width-600)/2, 100, 600, 100), "High Sprint", style);

		style.fontSize = 25;
		style.normal.textColor = Color.yellow;
		GUI.Label(new Rect((Screen.width-700)/2, 400, 700, 100), "Press any key to start!", style);

	}
	
}
