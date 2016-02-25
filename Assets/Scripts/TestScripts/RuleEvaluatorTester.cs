using UnityEngine;
using System.Collections;
using System;

public class RuleEvaluatorTester : MonoBehaviour {
	
	private string message = string.Empty;
	
	public RuleEvaluator evaluator;
	
	void Awake() {
		if(evaluator == null)
			throw new NullReferenceException("Evaluator not assigned");
		
	}
	
	void Start() {
		message = string.Empty;	
	}
	
	void OnEnable() {
		evaluator.OnGameEvent += HandleEvaluatorOnGameEvent;	
	}

	void OnDisable() {
		evaluator.OnGameEvent -= HandleEvaluatorOnGameEvent;	
	}	
	
	void HandleEvaluatorOnGameEvent (GameEvents eventType)
	{
		if(eventType == GameEvents.Win) {
			message = "You won! :P";
		} else {
			message = "You DIED!!";
		}
	}
	
	
	// Use this for initialization
	void OnGUI() {
		if(GUI.Button(new Rect(10, 10, 100, 50), "Win")) {
			EventContext.AddEvent ("Win");
		}
		
		if(GUI.Button(new Rect(10, 60, 100, 50), "Die")) {
			EventContext.AddEvent ("Dead");
		}

		
		if(GUI.Button(new Rect(10, 120, 100, 50), "Reload")) {
			Application.LoadLevel("EmptyTest");
		}
		
		GUI.TextArea (new Rect(10, 180, 100, 50), message);
		
	}
}
