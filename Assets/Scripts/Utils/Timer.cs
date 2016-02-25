using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	
	public float TimeInSeconds;
	public bool TimerEnabled;
	public bool Loop;
	public TimerEventBase TimerEvent;
	
	private float _currentTimeSpan;
	
	// Use this for initialization
	void Start () {
		_currentTimeSpan = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(TimerEnabled) {
			if(_currentTimeSpan >= TimeInSeconds) {
				if(!Loop) {
					TimerEnabled = false;	
				} else {
					_currentTimeSpan = 0;	
				}
				TimerEvent.Execute();
			} else {
				_currentTimeSpan += Time.deltaTime;	
			}
		}
	}
	
	public void StartTimer() {
		_currentTimeSpan = 0;
		TimerEnabled = true;
	}
}
