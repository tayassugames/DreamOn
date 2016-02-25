using UnityEngine;
using System.Collections;

public class AnimationEventHandler : MonoBehaviour {
	
	public delegate void AnimationEndedHandler(string animationName);
	public event AnimationEndedHandler OnAnimationEnded;
	
	public void AnimationEnded(string animationName) {
		if(OnAnimationEnded != null) {
			OnAnimationEnded(animationName);	
		}
	}
}
