using UnityEngine;
using System.Collections;

public class LoadSceneTimerEvent : TimerEventBase {

	public string sceneToLoad;
	
	private bool _enabled;
	
	void Start() {
		_enabled = false;	
	}
	
	// Update is called once per frame
	void Update () {
		if(_enabled) {
			if(Application.CanStreamedLevelBeLoaded(sceneToLoad)) {
				if(CharacterStats.fromAndy != 1){ // If we are not coming from the Andy in game menu
				Application.LoadLevel(sceneToLoad);	 //load the profiles normal scene
				}
				else
				{
				Application.LoadLevel("TitleScreenFromAndy"); // load the andy title scene
				}
			}
		}
	}
	
	public override void Execute() {
		_enabled = true;
	}
}
