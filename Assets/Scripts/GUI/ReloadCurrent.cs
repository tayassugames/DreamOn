using UnityEngine;
using System.Collections;

public class ReloadCurrent : MonoBehaviour {

	void OnGUI() {
		if(GUI.Button(new Rect(5,5,100,25), "Reload")) {
			Application.LoadLevel (Application.loadedLevelName);	
			
		}
	}
}
