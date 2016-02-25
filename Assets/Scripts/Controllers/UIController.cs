using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	// Use this for initialization
	void OnGUI() {
		if(GUI.Button (new Rect(0,50,100,20), "CameraTest")) {
			Application.LoadLevel ("CameraTestScene");
		}
		if(GUI.Button (new Rect(110,50,100,20), "MapViewer")) {
			Application.LoadLevel ("SphericMap");
		}
		if(GUI.Button (new Rect(220,50,100,20), "TileSystem")) {
			Application.LoadLevel ("TileSystem");
		}

	}
}
