using UnityEngine;
using System.Collections;
using System;

public static class AnimationExtensions {
	/*
	public static bool Play(this Animation animation, Action<string> onComplete) {
		if(animation.Play()) {
		
			while(animation.isPlaying){}
			
			if(onComplete != null) {
				onComplete(animation.clip.name);
			}
		}		
		return true;
	}
	
	public static IEnumerator Play(this Animation animation, string clipName, Action<string> onComplete) {
		if(animation.Play(clipName)) {
			
			while(animation.isPlaying)
				yield return null;
			
			if(onComplete != null) {
				onComplete(clipName);
			}
		}
	}

	public static IEnumerator CrossFade(this Animation animation, string clipName, Action<string> onComplete) {
		return CrossFade(animation, clipName, onComplete, 0.3f, PlayMode.StopSameLayer);
	}
	
	public static IEnumerator CrossFade(this Animation animation, string clipName, Action<string> onComplete, float fadeLength, PlayMode mode) {
		animation.CrossFade(clipName, fadeLength, mode);
		
		while(animation.isPlaying)
			yield return null;
		
		if(onComplete != null) {
			onComplete(clipName);
		}

	}
	*/
	
}
