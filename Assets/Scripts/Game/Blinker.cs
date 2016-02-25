using UnityEngine;
using System.Collections;
using System;

public class Blinker : MonoBehaviour {
	
	public float totalDuration = 2.0f;
	public float intervalDuration = 0.25f;
	public Renderer characterRenderer;
	
	private bool _isEnabled;
	private bool _isOn = true;
	private bool _isExecuting;
	private float _timer = 0;
	
	#region MonoBehavior methods
	
	void Awake() {

	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_isEnabled) {
			_isEnabled = false;
			_isExecuting = true;
			_timer = 0;
			StartCoroutine(Blink());
		}
	}
	
	#endregion
	
	#region Events
	
	public delegate void BlinkFinishedHandler();
	public event BlinkFinishedHandler OnBlinkFinished;
	
	#endregion
	
	
	public void Execute() {
		if(!_isExecuting) {
			_isEnabled = true;
		}
	}


	private IEnumerator Blink() {
		while(_timer < totalDuration) {
			_isOn = !_isOn;
			characterRenderer.enabled = _isOn;
			yield return(new WaitForSeconds(intervalDuration));
			_timer += intervalDuration;
		}
		characterRenderer.enabled = true;
		_isExecuting = false;
		if(OnBlinkFinished != null) {
			OnBlinkFinished();	
		}
	}
	
	
}
