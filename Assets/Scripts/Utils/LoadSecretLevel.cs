using UnityEngine;
using System.Collections;

public class LoadSecretLevel : MonoBehaviour {
	
	public string LevelName;

	void OnTriggerEnter() {
		Application.LoadLevel(LevelName);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
