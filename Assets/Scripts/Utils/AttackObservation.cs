using UnityEngine;
using System.Collections;
using System;

public class AttackObservation : MonoBehaviour {

	//In order for this script to work, enemy objects must be in the "Enemy" layer.
	
	public GameObject hero;
	public bool debugMode = false;
	
	public float distanceX0 = 10;
	public float distanceX1 = 8;
	public float distanceX2 = 6;
	public float distanceX3 = 4;
	
	void Awake() {
		if(hero == null)
			throw new NullReferenceException("Hero not found");
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(debugMode){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
		}
	}
	
	void OnEnable(){
		//Gesture.onMultiTapE += OnTap;
		Gesture.onMouse1E += OnTap;
		Gesture.onTouchE += OnTap;		
	}
	
	void OnDisable(){
		//Gesture.onMultiTapE -= OnTap;
		Gesture.onMouse1E -= OnTap;
		Gesture.onTouchE -= OnTap;
	}
	
	void OnTap(Vector2 pos){  //(Tap tap){
		//Ray ray = Camera.main.ScreenPointToRay(tap.pos);
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		
		int layerMask = 1 << LayerMask.NameToLayer("Enemy");
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
			var distance = Vector3.Distance(hit.collider.transform.position, hero.transform.position);
			
			if(debugMode){
				Debug.Log("Distancia entre hero y enemigo:" + distance);
			}
			
			if(distance > distanceX0){
				Debug.Log("Ataque nulo");
			}else if((distanceX0 > distance) && (distance > distanceX1)){
				Debug.Log("Ataque A");
			}else if((distanceX1 > distance) && (distance > distanceX2)){
				Debug.Log("Ataque B");
			}else if((distanceX2 > distance) && (distance > distanceX3)){
				Debug.Log("Ataque C");
			}else if(distanceX3 > distance){
				Debug.Log("Ataque nulo");
			}
		}
	}
}
