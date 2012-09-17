using System;

using UnityEngine;
using UnityEditor;

public class CreateCheckPoint {
	
	[MenuItem("WayPoints/Create next %g")]
	public static void createWayPoint() {
		if(Selection.gameObjects.Length == 1) {
			GameObject wp = Selection.activeGameObject;
			UnityEngine.Object prefabRoot = EditorUtility.GetPrefabParent(wp);
			GameObject newWp = (GameObject) EditorUtility.InstantiatePrefab(prefabRoot);
			newWp.transform.position = wp.transform.position + new Vector3(1, 0, 1);
			newWp.transform.rotation = wp.transform.rotation;
			CheckPointScript thisScrpt = (CheckPointScript) wp.gameObject.GetComponent(typeof(CheckPointScript));

			thisScrpt.nextCheckPoint = newWp;
			
			string[] name_comp = wp.name.Split('_');
			if(name_comp.Length > 1)
				newWp.name = name_comp[0] + "_" + (Int32.Parse(name_comp[1])+1);
			else
				newWp.name = name_comp[0] + "_0";		
			
			Selection.activeGameObject = newWp;
			EditorUtility.SetDirty(thisScrpt);
		}		
	}
}

