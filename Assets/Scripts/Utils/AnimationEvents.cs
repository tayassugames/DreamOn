using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {
	
	public GameObject mapCamera;
	
	// Use this for initialization
	public void ShowCamera(int intValue) {
		if(mapCamera != null) {
			mapCamera.SetActive(true);	
		}
	}
	
	public void HideCamera(int intValue) {
		if(mapCamera != null) {
			mapCamera.SetActive (false);	
		}
	}
}
