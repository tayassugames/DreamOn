using UnityEngine;
using System.Collections;

public class TapRateTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
		if(GUI.Button(new Rect(10, 10, 150, 50), "Init")) {
			CharacterStats.InitProfiles();
		}
		
		if(GUI.Button(new Rect(10, 70, 150, 50), "SetProfile")) {
			CharacterStats.SetProfile("1", 0);
			CharacterStats.SetProfile("2", 1);
			CharacterStats.SetProfile("3", 2);
		}
		
		if(GUI.Button(new Rect(10, 130, 150, 50), "SetCurrentProfile")) {
			CharacterStats.SetCurrentAsLastProfile();
		}
		
		if(GUI.Button(new Rect(10, 190, 150, 50), "SaveStatsForCurrentProfile")) {
			CharacterStats.SetCurrentLevel("SpaceDefenders");
			CharacterStats.AddPointsToLevel(100);
			CharacterStats.ApplyLevelScore();
			CharacterStats.CreditCount = 100;
			CharacterStats.LastPosition = Vector3.zero;
			CharacterStats.LastRotation = Quaternion.identity;
			
			CharacterStats.SaveStats();
			
			CharacterStats.GlobalScore = 0;
			CharacterStats.CurrentLevelScore = 0;
			CharacterStats.CreditCount = 0;
			CharacterStats.LastPosition = Vector3.up;
			CharacterStats.LastRotation = Quaternion.identity;
			
		}
		
		if(GUI.Button(new Rect(10, 250, 150, 50), "LoadStatsForCurrentProfile")) {
			CharacterStats.GlobalScore = 0;
			CharacterStats.CurrentLevelScore = 0;
			CharacterStats.CreditCount = 0;
			CharacterStats.LastPosition = Vector3.up;
			CharacterStats.LastRotation = Quaternion.identity;
			
			CharacterStats.GetLastProfile();
			CharacterStats.LoadStats();
		}
		
		GUI.TextArea (new Rect(300, 10, 150, 30), CharacterStats.GlobalScore.ToString());
		
	}
}
