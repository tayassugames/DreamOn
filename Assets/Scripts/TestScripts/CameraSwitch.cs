using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {
	
	public GameObject TwoDCamera;
	public GameObject IsometricCamera;
	public GameObject TopDownCamera;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if(TwoDCamera != null && IsometricCamera != null && TopDownCamera != null) {
		
			if (GUI.Button(new Rect(10, 10, 150, 30), "2D")) {
	            print("You clicked 2D");
				IsometricCamera.SetActive(false);
				TopDownCamera.SetActive(false);
				TwoDCamera.SetActive(true);
			}
	
			if (GUI.Button(new Rect(170, 10, 150, 30), "Isometric")) {
	            print("You clicked Isometric");
				IsometricCamera.SetActive(true);
				TopDownCamera.SetActive(false);
				TwoDCamera.SetActive(false);
			}
	
			if (GUI.Button(new Rect(330, 10, 150, 30), "Top down")) {
	            print("You clicked Top down");
				IsometricCamera.SetActive(false);
				TopDownCamera.SetActive(true);
				TwoDCamera.SetActive(false);
			}
		}

	}
}
