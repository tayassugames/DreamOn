using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RuleEvaluator : MonoBehaviour {
	
	#region Events
	
	public delegate void GameEventHandler(GameEvents eventType);
	public event GameEventHandler OnGameEvent;
	
	#endregion
	
	public List<RuleBase> winRules;
	public List<RuleBase> looseRules;
	
	#region MonoBehavior methods
	
	void Awake() {
		if(winRules == null)
			throw new NullReferenceException("Win conditions are not set");
		
		if(looseRules == null)
			throw new NullReferenceException("Loose conditions are not set");
	}
	
	void Start () {
	
	}
	
	void LateUpdate () {
		EvaluateRules();	
	}
	
	#endregion
	
	#region Private methods
	
	private void EvaluateRules() {
		if(winRules != null) {
			if (EvaluateRulesCollection(winRules)) {
				FireGameEvent(GameEvents.Win);
				return;
			}
		}
		
		if(looseRules != null) {
			if (EvaluateRulesCollection(looseRules)) {
				FireGameEvent(GameEvents.Loose);
				return;
			}			
		}
	}
	
	private bool EvaluateRulesCollection(List<RuleBase> rulesCollection) {
		bool result = false;
		foreach(IRule rule in rulesCollection) {
			if(rule.Evaluate()) {
				result = true;
			}
		}
		return result;
	}
	
	private void FireGameEvent(GameEvents eventType) {
		if(OnGameEvent != null) {
			OnGameEvent(eventType);	
		}		
	}
	
	#endregion
	
}
