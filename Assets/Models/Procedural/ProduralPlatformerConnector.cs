using UnityEngine;
using System.Collections;

public class ProduralPlatformerConnector : MonoBehaviour {
	
	public LevelPiece currentPiece;
	public bool forward = true;
	
	private bool triggered;
	
	void Start() {
		triggered = false;
	}
	
	void OnTriggerEnter (Collider other) {
		if(other.gameObject.tag == "Player" && !triggered) {
			triggered = true;
			ProduralPlatformer.instance.PlaceAnoterPiece(currentPiece.UniquePieceName, forward);
		}	
	}
	
	public void ResetTrigger() {
		triggered = false;
	}
	
}
