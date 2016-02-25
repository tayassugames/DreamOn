using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour {
	
	#region Events
	
	public delegate void AnimationFinishedHandler(string clipName);
	public event AnimationFinishedHandler OnAnimationFinished;
	
	#endregion
	
	public Animation animatedCharacter;
	public AnimationEventHandler animationEventHandler;
	public List<string> queuedAnimations;
	
	//public bool inputOk = false;
	private string _currentAnimationName;
	private bool _isImmediate;
	//private bool _isQueued = false;
	
	
	#region MonoBehavior methods
	
	void Awake() {
		if(animatedCharacter == null)	
			throw new NullReferenceException("Animated character not assigned");
	}
	
	void OnEnable() {
		//Attach to events
		CharacterStats.OnStateChange += HandleCharacterStatsOnStateChange;
		if(animationEventHandler != null)
			animationEventHandler.OnAnimationEnded += HandleOnAnimationFinished;
	}

	void OnDisable() {
		//Dettach to events
		CharacterStats.OnStateChange -= HandleCharacterStatsOnStateChange;
		if(animationEventHandler != null)
			animationEventHandler.OnAnimationEnded -= HandleOnAnimationFinished;
	}
	
	// Use this for initialization
	void Start () {
		animatedCharacter.wrapMode = WrapMode.Loop;
		queuedAnimations = new List<string>();
		_isImmediate = false;
		_currentAnimationName = "idle";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_isImmediate) {
			animatedCharacter.Play(_currentAnimationName);
		} 
		else {
			animatedCharacter.CrossFade(_currentAnimationName);
		}
		
	}

	#endregion
	
	
	#region Event Handlers
	private void HandleCharacterStatsOnStateChange(States previousState, States newState)
	{
		if(previousState == newState)
			return;
		
		//Implement animation transition logic here
		
		//Temporary logic
		switch(newState) {
			case States.Run:
				animatedCharacter.wrapMode = WrapMode.Loop;
				_isImmediate = true;
				_currentAnimationName = "run_cycle_custom";
				break;
			case States.Sprint:
				animatedCharacter.wrapMode = WrapMode.Loop;
				_isImmediate = false;
				_currentAnimationName = "sprint_custom";
				break;
			case States.Jump:
				animatedCharacter.wrapMode = WrapMode.PingPong;
				_isImmediate = false;
				_currentAnimationName = "jump_custom";
				break;
			case States.DoubleJump:
				animatedCharacter.wrapMode = WrapMode.Clamp;
				_isImmediate = true;
				_currentAnimationName = "doublejump_custom";
				break;
			case States.Fall:
				animatedCharacter.wrapMode = WrapMode.Once;
				_isImmediate = false;
				_currentAnimationName = "fall_custom";
				break;
			case States.Slide:
				animatedCharacter.wrapMode = WrapMode.Clamp;
				_isImmediate = true;
				_currentAnimationName = "slide";
				break;
			case States.Glide:
				animatedCharacter.wrapMode = WrapMode.Clamp;
				_isImmediate = false;
				_currentAnimationName = "glide_custom";
				break;
			case States.WallKick:
				animatedCharacter.wrapMode = WrapMode.Once;
				_isImmediate = true;
				_currentAnimationName = "wallkick";
				break;
			case States.Idle:
			default:
				if(previousState == States.Run || previousState == States.Sprint)
				{
					animatedCharacter.wrapMode = WrapMode.Clamp;
					_isImmediate = false;
					//_isQueued = true;
					_currentAnimationName = "idle_custom";
					//queuedAnimations.Add("idle");
				}
				else
				{
					animatedCharacter.wrapMode = WrapMode.Loop;
					_isImmediate = false;
					_currentAnimationName = "idle_custom";
				}
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
