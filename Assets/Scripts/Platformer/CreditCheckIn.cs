using UnityEngine;
using System.Collections;
using System;
public class CreditCheckIn : MonoBehaviour {
	
	public string LevelName;
	public Texture clearedCoin;
	
	// Use this for initialization
	void Start () {
		if(CharacterStats.LevelCredits != null) {
			if(CharacterStats.LevelCredits.ContainsKey(LevelName)) GetComponent<Renderer>().material.mainTexture = clearedCoin;
		}
	}
}
