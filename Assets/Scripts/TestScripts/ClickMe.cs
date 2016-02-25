using UnityEngine;
using System.Collections;

public class ClickMe : MonoBehaviour {
	
	public string SceneToLoad;
	
	void OnGUI() {
		if(GUI.Button(new Rect(10, 10, 200, 30), "Click to start")) {
			if(!string.IsNullOrEmpty(SceneToLoad)) {
				Application.LoadLevel(SceneToLoad);
			}
		}
	}
}
