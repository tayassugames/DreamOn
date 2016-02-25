using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AnimationController2 : MonoBehaviour {
	
	public static AnimationController2 Instance;
	
	public enum ANIMATION {
		idle = 0,
		run = 1
	}
	
	
	#region Events
	
	public delegate void AnimationFinishedHandler(string clipName);
	public event AnimationFinishedHandler OnAnimationFinished;
	
	#endregion
	
	public Animation animatedCharacter;
	public AnimationEventHandler animationEventHandler;
	public List<string> queuedAnimations;
	
	//public bool inputOk = false;
	private string _currentAnimationName;
	
	private ANIMATION _currentAnimaiton = ANIMATION.idle;
	//private bool _isQueued = false;
	
	public void setAnimation(int run) {
		if(run == 0) {
			_currentAnimaiton = ANIMATION.idle;
		}
		else if (run == 1) {
			_currentAnimaiton = ANIMATION.run;
		}
	}
	
	#region MonoBehavior methods
	
	void Awake() {
		Instance = this;
		
		if(animatedCharacter == null)	
			throw new NullReferenceException("Animated character not assigned");
	}
	
	void OnEnable() {
		//Attach to events
		//CharacterStats.OnStateChange += HandleCharacterStatsOnStateChange;
		//if(animationEventHandler != null)
		//	animationEventHandler.OnAnimationEnded += HandleOnAnimationFinished;
	}

	void OnDisable() {
		//Dettach to events
		//CharacterStats.OnStateChange -= HandleCharacterStatsOnStateChange;
		//if(animationEventHandler != null)
		//	animationEventHandler.OnAnimationEnded -= HandleOnAnimationFinished;
	}
	
	// Use this for initialization
	void Start () {
		animatedCharacter.wrapMode = WrapMode.Loop;
		//queuedAnimations = new List<string>();
		
		_currentAnimationName = "idle";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_currentAnimaiton == ANIMATION.idle)
		{
			_currentAnimationName = "idle";
		} else
		{
			_currentAnimationName = "run_cycle";
		}	

		animatedCharacter.CrossFade(_currentAnimationName);
		/*
		if(_currentAnimaiton == ANIMATION.idle)
		{
			_currentAnimationName = "idle";
			
		} else
		{
			_currentAnimationName = "run_cycle";
			
		}	
		
		
		
		*/
		
	}

	#endregion
	
	
	#region Event Handlers
	private void HandleCharacterStatsOnStateChange(States previousState, States newState)
	{
		Debug.Log (string.Format("State change: {0} -> {1}", previousState.ToString(), newState.ToString()));
		
		if(previousState == newState)
			return;
		
		//Implement animation transition logic here
		
		//Temporary logic
		switch(newState) 
		{
			case States.Run:
				animatedCharacter.wrapMode = WrapMode.Loop;
				
				_currentAnimationName = "run_cycle";
				break;
			
			case States.Idle:
				animatedCharacter.wrapMode = WrapMode.Loop;
				
				_currentAnimationName = "idle";
				break;
		}
	}
	
	private void HandleOnAnimationFinished(string clipName) {
		if(queuedAnimations != null && queuedAnimations.Count > 0) {
			_currentAnimationName = queuedAnimations[0];
			queuedAnimations.RemoveAt(0);
		}
		
		
		if(OnAnimationFinished != null) {
			OnAnimationFinished(clipName);
		}
	}
	
	#endregion
	
	public void SetAnimation(ANIMATION animationSetting) {
		_currentAnimaiton = animationSetting;
	}
	
	/*
	public void PlayFall1() {
		if(inputOk) {
			animation.Play("Jump1");
		} else {
			animation.Play("Fall1");
		}
	}
	*/
}
