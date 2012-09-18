using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LapController : MonoBehaviour {

	public static int cars = 3;
	public static int waypoints = 21;
	public static int max_laps = 2;
	
	private int[] laps = new int[cars];
	private int[] wps_total = new int[cars];
	private List<List<bool>> wps_track = new List<List<bool>>();
	
	private int winner = -1;
	
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
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(this.HasFinished()) print ("Ganador Auto: " + winner);
		else {
			print ("Vuelta: " + laps[0] + " - Pos: " + GetPosition(0));
		}
	}
}
