using UnityEngine;
using System.Collections;

public class MinigameScoreTest : MonoBehaviour {
	
	//public PointsRepresentation globalScore;
	//public PointsRepresentation levelScore;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//globalScore.SetPoints(CharacterStats.GlobalScore);
		//levelScore.SetPoints(CharacterStats.CurrentLevelScore);
	}
	
	void OnGUI() {
		
		if(GUI.Button(new Rect(40, 50, 150, 50), "10 Puntos ganados")) {
			CharacterStats.AddPointsToLevel(10);
		}
		
		if(GUI.Button(new Rect(40, 110, 150, 50), "Nivel fallado")) {
			CharacterStats.ClearLevelScore();
		}

		if(GUI.Button(new Rect(40, 170, 150, 50), "Nivel ganado")) {
			CharacterStats.ApplyLevelScore();
		}

		
	}
}
