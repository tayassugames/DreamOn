using UnityEngine;
using System.Collections;
using System;

public class AudioFader : MonoBehaviour {

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
	
	
	public float speed = 1;
	public FadeDirection fadeDirection = FadeDirection.FadeIn;
	public AudioSource audioSource;
	
	private float direction = 0.01f;
	private bool inAction = false;
	private float _volume;
	
	void Awake() {
		if(audioSource == null)
			throw new NullReferenceException("audio source not found");

	}
	
	// Use this for initialization
	void Start () {
		_volume = audioSource.volume;
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		//If it is doing the fade animation.
		if(inAction){
			_volume += speed * direction;
		
			if(_volume > 1 || _volume < 0) {
				if(_volume < 0){
					_volume = 0;
					if(audioSource.isPlaying) {
						audioSource.Stop();
					}
					inAction = false;
				}
				
				if(_volume > 1){
					_volume = 1;
					inAction = false;
				}
				if(OnFadeFinished != null) {
					OnFadeFinished();	
				}
			} else {
				SetVolume(_volume);				
			}
		}
	}
	
	public void Fade() {
		//Init color
		if(fadeDirection == FadeDirection.FadeIn) {
			direction = 0.01f;
			_volume = 0.0f;
		} else {
			direction = -0.01f;
			_volume = audioSource.volume; //1.0f;
		}

		SetVolume(_volume);
		inAction = true;
		if(OnFadeStarted != null) {
			OnFadeStarted();	
		}
	}
	
	private void SetVolume(float localVolume) {
		if(!audioSource.isPlaying)
			audioSource.Play();
		audioSource.volume = localVolume;
	}
	
	
}
