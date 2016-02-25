using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ProduralPlatformer : MonoBehaviour {
	
	public static ProduralPlatformer instance;
	
	public LevelDifficulty maxDifficulty;
	public float maxPieces;
	
	
	private Dictionary<string,LevelPiece> pieces;
	private LevelPiece upcoming;
	
	void Awake() {
		instance = this;
	}
	
	void Start() {
		
		pieces = new Dictionary<string,LevelPiece>();
	}
	
	public void AddPiece(string newName, LevelPiece newPiece) {
		pieces.Add(newName, newPiece);
	}
	
	public int TotalPieces() {
		return pieces.Count;
	}
	
	public LevelPiece GetPiece(string searchName) {
		if(pieces.ContainsKey(searchName)) {
			return pieces[searchName];
		}
		else {
			return null;
		}
	}
	
	public void PlaceAnoterPiece(string name, bool forward) {
		bool continuePlacement = !false;
		int temp=0;
		
		
		if(pieces.Count > 1) {
			
			
			if(pieces.ContainsKey(name)) {
				pieces[name].Conected(forward);
				
				while(continuePlacement) { 
					temp = Random.Range(0, pieces.Count);
					
					upcoming = pieces[pieces.Keys.ElementAt(temp)];
					
					if(upcoming != pieces[name]) {
						continuePlacement = !true;
					}
					else {
						continuePlacement = !false;
					}
				}
				
				
				
				if(forward) {
					pieces[name].Conected(forward);
					upcoming.Conected(!forward);
					 upcoming.PlacePiece(pieces[name].outPoint.position,forward);
				}
				else {
					pieces[name].Conected(!forward);
					upcoming.Conected(forward);
					upcoming.PlacePiece(pieces[name].inPoint.position,forward);
				}
				
				
			}
			else {
				Debug.LogWarning("Attempting to place a unregistered level piece");
			}
		}
		else {
			Debug.LogWarning("Not enough level pieces!!!");
		}
	}
}
