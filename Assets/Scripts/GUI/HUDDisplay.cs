using UnityEngine;
using System.Collections;

public class HUDDisplay : MonoBehaviour {
	
	public Texture HUDTexture;
	public bool canShow = true;
	
	void OnGUI() {
		if(canShow) {
			GUI.DrawTexture(ScaleManager.GetScaledRect(new Rect(10, 10, HUDTexture.width, HUDTexture.height)), HUDTexture);		
		}
	}
}
