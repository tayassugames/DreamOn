using UnityEngine;
using System.Collections;
using System;

public class SplashImage : MonoBehaviour {
	
	public Texture backgroundTexture;
	
	void Awake() {
		if(backgroundTexture == null) {
			throw new NullReferenceException("Texture not defined");	
		}
		
	}
	
	// Use this for initialization
	void OnGUI() {
		GUI.DrawTexture (new Rect(0,0, Screen.width, Screen.height), backgroundTexture);
	}
}
