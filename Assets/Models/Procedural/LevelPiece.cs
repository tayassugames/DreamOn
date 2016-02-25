using UnityEngine;
using System.Collections;

public class LevelPiece : MonoBehaviour {
	public string UniquePieceName;
	
	public Transform inPoint;
	public Transform outPoint;
	public ProduralPlatformerConnector trigger;
	
	public LevelDifficulty difficulty = LevelDifficulty.VeryEasy; 
	public LevelPieceTypes pieceType = LevelPieceTypes.Two_Ways;
	
	private bool connectedIn;
	private bool connectedOut;
	
	private Vector3 distanceInPoint;
	private Vector3 distanceOuPoint;
	
	
	void Start () {
		connectedIn = false;
		connectedOut = false;
		
		distanceInPoint = this.transform.position - inPoint.transform.position;
		distanceOuPoint = this.transform.position - outPoint.transform.position;
		
		if(ProduralPlatformer.instance != null) {
			ProduralPlatformer.instance.AddPiece(UniquePieceName, this.gameObject.GetComponent("LevelPiece") as LevelPiece);
		}
		else {
			Debug.LogWarning("Piece not registered in singleton");
		}
	}
	
	public LevelDifficulty GetDifficulty() {
		return difficulty;
	}
	
	public void PlacePiece(Vector3 target, bool forward) {
		if(forward) {
			this.transform.position = target + distanceInPoint;
			trigger.ResetTrigger();
			
		}
		else {
			this.transform.position = target + distanceOuPoint;
			trigger.ResetTrigger();
		}
	}
	
	public void Conected(bool forward) {
		if(forward) {
			connectedIn = true;
		}
		else {
			connectedOut = true;
		}
	}
	
	public bool IsConnected(bool forward) {
		if(forward) {
			return connectedOut;
		}
		else {
			return connectedIn;
		}
	}
}
