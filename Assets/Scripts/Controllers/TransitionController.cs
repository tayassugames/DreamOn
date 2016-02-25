using UnityEngine;
using System.Collections;

public class TransitionController : MonoBehaviour {

	public FadeOutScript fader;

	void OnEnable() {
		fader.OnFadeFinished += FadeHandler;	
	}
	
	void OnDisable() {
		fader.OnFadeFinished -= FadeHandler;	
	}
	
	
	void OnGUI() {
		
		if(GUI.Button (new Rect(10,10, 50, 30), "In")) {
			fader.gameObject.SetActive(true);
			fader.fadeDirection = FadeOutScript.FadeDirection.FadeIn;
			fader.Fade();	
		}

		if(GUI.Button (new Rect(10,40, 50, 30), "Out")) {
			fader.gameObject.SetActive(true);
			fader.fadeDirection = FadeOutScript.FadeDirection.FadeOut;
			fader.Fade();	
		}
		
	}
	
	private void FadeHandler() {
		
		fader.gameObject.SetActive(false);	
	}
	
}
