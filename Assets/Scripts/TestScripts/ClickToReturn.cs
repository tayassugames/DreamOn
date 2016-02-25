using UnityEngine;
using System.Collections;

public class ClickToReturn : MonoBehaviour {

	// Use this for initialization
	void OnGUI() {
		if(GUI.Button(new Rect(10, 10, 200, 30), "click to return"	)) {
			Application.LoadLevel("SphericWorld");	
		}
		
	}
}
