using UnityEngine;
using System.Collections;

public class PlanetMover : TimerEventBase {
	public float verticalMovementDistance = 1.0f;
	public int maxCounter = 20;
	
	private Transform _transform;
	private int counter = 0;
	
	void Awake() {
		_transform = transform;	
	}
	
	void Start() {
	}
	
	public override void Execute() {
		if(counter < maxCounter) {
			_transform.Translate(Vector3.up * verticalMovementDistance);
			counter++;
		}
	}	

}
