/* GameStar Project 2012
 * Temporary pause menu. Written by Anthony Najjar
 * Last Updated: June 16, 2012
 */

using UnityEngine;
using System.Collections;
using System;

public class PauseMenu : MonoBehaviour {
	
	
	
	/* Will be used for textured buttons instead of the default ones
	public Texture2D pauseTex;
	public Texture2D resumeTex;
	public Texture2D restartTex;
	public Texture2D exitTex;
	*/
	public Camera perspectiveCam;
	public Camera orthographicCam;
	
	public platformerControl platCon;
	public string exitLevel;
	
	private bool _paused;
	private bool _pauseButton;
	private int _cameraSwitch;
	//private int _gravitySwitch;
	
	void Awake() {
		if(string.IsNullOrEmpty(exitLevel)) {
			//throw new NullReferenceException("exit level is not defined");
		}
	}
	
	void Start () {
		
		_paused = false;
		_pauseButton = false;
		SetCamera(0);
		//_gravitySwitch = 0; // Set initial gravity to earth magnitude
	
	}
	
	/// <summary>
	/// Sets the active camera.
	/// </summary>
	/// <param name='mode'>
	/// Camera mode: 0 = Perspective, 1 = Orthogonal
	/// </param>
	private void SetCamera(int mode) {
		if (mode == 1){
			perspectiveCam.enabled = false;
			orthographicCam.enabled = true;
			_cameraSwitch = 1;
		}
		else{
			perspectiveCam.enabled = true;
			orthographicCam.enabled = false;
			_cameraSwitch = 0;
		}
	}
	
	void OnGUI(){
		
		//Demo UI buttons for testing some features
		if(_pauseButton){
			if(GUI.Button(new Rect((Screen.width / 6) * 3,(Screen.height / 12) * 1,(Screen.width / 7),(Screen.height / 10)),"2d/3d Camera")){
				if (_cameraSwitch == 0){
					SetCamera(1);
				} else {
					SetCamera(0);
				}
				
			}
		}

		//Pause Button display
		if(_pauseButton){
			if(GUI.Button(new Rect((Screen.width / 6) * 5,(Screen.height / 12) * 1,(Screen.width / 7),(Screen.height / 10)),"Pause")){
				_pauseButton = false;
				_paused = true;
			}
		}
		
		// If game has been paused display in-game menu
		if(_paused){
			
			if(GUI.Button(new Rect((Screen.width / 5) * 2,(Screen.height / 8) * 2,(Screen.width / 5),(Screen.height / 10)),"Resume")){
				_paused = false;
				_pauseButton = true;
				Time.timeScale = 1.0f;
			}
			
			if(GUI.Button(new Rect((Screen.width / 5) * 2,(Screen.height / 8) * 3,(Screen.width / 5),(Screen.height / 10)),"Restart Level")){
				_paused = false;
				_pauseButton = true;
				Time.timeScale = 1.0f;
				Application.LoadLevel(Application.loadedLevelName); // change "0" integer for the correct scene index
			}
			
			if(GUI.Button(new Rect((Screen.width / 5) * 2,(Screen.height / 8) * 4,(Screen.width / 5),(Screen.height / 10)),"Exit Level")){
				_paused = false;
				Time.timeScale = 1.0f;
				_pauseButton = true;
				//print ("Exit this level");
				Application.LoadLevel(exitLevel);
			}
		}
	}
	
	void Update () {
		if (_paused == true) {
			Time.timeScale = 0.0f;
		}
	}
}
