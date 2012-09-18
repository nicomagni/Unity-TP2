using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LapController : MonoBehaviour {
	
	public GUISkin mySkin;
	
	public static int cars = 4;
	public static int waypoints = 21;
	public static int max_laps = 4;
	
	private int secondsToStart = 3;
	private float timeFromStart;
	
	private bool started;
	
	
	private int[] laps = new int[cars];
	private int[] wps_total = new int[cars];
	private List<List<bool>> wps_track = new List<List<bool>>();
	
	private int winner = -1;
	
	private CarMove carMove;

	void Start () {
		timeFromStart = Time.time+secondsToStart;
		started = false;
	}

	// init the data structure
	public LapController(){
		for( int i = 0 ; i < cars ; i++ ) {
			wps_track.Add(new List<bool>());
			for(int j = 0 ; j < waypoints ; j++) wps_track[i].Add(false);
		}
	}
	
	public void TrackWaypoint(int waypoint, int car){
			if(!wps_track[car][waypoint]) wps_total[car]++;
			wps_track[car][waypoint] = true;
	}
	
	public void TrackEnd(int car){
		if(wps_total[car] > waypoints/2) laps[car]++;
		
		// check if the car is the winner
		if(laps[car] >= max_laps && winner == -1) winner = car;
		
		// remove waypoint counter and flags
		wps_total[car] = 0;
		for( int i = 0 ; i < waypoints ; i++ ){
			wps_track[car][i] = false;
		}
	}
	
	public bool HasFinished(){
		return winner != -1;
	}
	
	public bool hasStarted() {
		return started;
	}
	
	public int GetWinner(){
		return winner;
	}
	
	public int GetLap(int car){
		return laps[car];
	}
	
	public int GetPosition(int car){
		var position = 1;
		var currentLap = laps[car];
		
		for(int other = 0 ; other < cars ; other++){
			if(car == other) continue;
			
			if( laps[other] > currentLap          ) position++;
			if( laps[other] == currentLap && 
				wps_total[other] > wps_total[car] ) position++;
		}
		
		return position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!started) {
			started = (Time.time - timeFromStart) > 0;
		}
	}

	void OnGUI () {
		GUIStyle style = new GUIStyle();
		style.fontSize = 18;
		style.font = mySkin.font;
		style.normal.textColor = Color.white;

		if(!started) {
			int seconds = (int) (timeFromStart - Time.time) + 1;
			style.fontSize = 100;
			GUI.Label(new Rect((Screen.width-70)/2, (Screen.height-100)/2, 150, 100), " " + seconds, style);
		} else {
			GUI.Label(new Rect(20, 20, 200, 40), "Lap: " + GetLap(0) +"/"+max_laps, style);
			GUI.Label(new Rect(20, 40, 200, 40), "Position: " + GetPosition(0), style);		
			
			float currSpeed = GetCarMove().getCurrentSpeed()/80;
			if(currSpeed < 5F)
				currSpeed = 0;
			
			GUI.Label(new Rect(20, 60, 200, 40), "Speed: " + System.Math.Floor(currSpeed), style);
			
			if(HasFinished()) {
				string bigMsj = "You win!";
				string smallMsj = "play next level";
				if(winner!=-1) {
					bigMsj = "Player " + winner + " wins";
					smallMsj = "play again";
				}
				
				style.alignment = TextAnchor.MiddleCenter;
			
				style.fontSize = 60;	
				GUI.Label(new Rect((Screen.width-500)/2, (Screen.height+100)/2, 600, 100), bigMsj, style);
				style.fontSize = 25;
				style.normal.textColor = Color.yellow;
				style.alignment = TextAnchor.MiddleCenter;
				GUI.Label(new Rect((Screen.width-700)/2, (Screen.height+300)/2, 700, 100), "Press any key to " + smallMsj, style);
				if(Input.anyKeyDown){
	//				Application.LoadLevel("playScene");
				}
			}
		}
	}
	
	
	private CarMove GetCarMove(){
		if(carMove == null) {
			carMove = (CarMove) GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(CarMove));
		}
		
		return carMove;
	}
}
