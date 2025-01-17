using UnityEngine;
using System.Collections;

public class FadeOutScript : MonoBehaviour {
	
	public Texture fadeTexture;
	
	public enum FadeDirection {
		FadeIn,	
		FadeOut
	}
	
	#region Events
	public delegate void FadeStartedHandler();
	public event FadeStartedHandler OnFadeStarted;
	
	public delegate void FadeFinishedHandler();
	public event FadeFinishedHandler OnFadeFinished;
	#endregion
	
	
	public float totalTime = 1;
	public FadeDirection fadeDirection = FadeDirection.FadeIn;
	public int drawDepth = -1000;
	
	private float direction = 1f;
	private bool inAction = false;
	private float alpha;
	private float _timer = 0;
	private float _initialValue;
	private float _finalValue;
	
	// Use this for initialization
	void Start () {
	}
	
	void OnGUI() {
		if(inAction) {
			_timer += Time.deltaTime;
			
			alpha = Mathf.Lerp(_initialValue, _finalValue, Mathf.Clamp01(_timer/totalTime));
		    
			//Color tempColor = GUI.color;
			//tempColor.a = alpha;
			//GUI.color = tempColor;
		    
		    //GUI.depth = drawDepth;
		    
		    //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
			
			if((direction > 0 && alpha == 1) || (direction < 0 && alpha == 0)) {
				if(direction < 0)
					inAction = false;
				if(OnFadeFinished != null) {
					OnFadeFinished();
				}
			}
		}
	}
	
	public void Fade() {
		//Init color
		if(fadeDirection == FadeDirection.FadeIn) {
			direction = -1f;
			_initialValue = 1f;
			_finalValue = 0f;
		} else {
			direction = 1f;
			_initialValue = 0f;
			_finalValue = 1f;
		}
		
		_timer = 0;

		inAction = true;
		if(OnFadeStarted != null) {
			OnFadeStarted();	
		}
	}
		
	void FadeIn(){
		direction = -1f;
		inAction = true;
	}
	
	void FadeOut(){
		direction = 1f;
		inAction = true;
	}
	

}
