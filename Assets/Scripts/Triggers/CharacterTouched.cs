using UnityEngine;
using System.Collections;

public class CharacterTouched : MonoBehaviour {
	
	public DialogScript dialog;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void  OnCollisionEnter(Collision collision) {
		print("collision");	
	}
	
	void OnTriggerEnter() {
	
		//Add credit amount to Character
		print("collision");	
	}
}
