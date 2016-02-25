using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireTrigger : MonoBehaviour {
	
	public LayerMask layerMask;
	public Vector3 fireSphereOrigin;
	public List<Collider> ignoreCollider;
	public float reducedLifePerSecond = 5;
	public bool isEnabled = false;
	public ParticleSystem smokeParticles;
	
	private platformerControl _platformerController;
	private Collider _localCollider;
	
	void Start() {
		_localCollider = gameObject.GetComponent<Collider>();
		
		if(ignoreCollider != null && ignoreCollider.Count > 0) {
			foreach(Collider collider in ignoreCollider) {
				Physics.IgnoreCollision(_localCollider, collider);
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if(isEnabled) {
			smokeParticles.Play();	
		}
	}
	
	// Use this for initialization
	void OnTriggerStay(Collider other) {
		if(isEnabled) {
			_platformerController = other.gameObject.GetComponent<platformerControl>();
			if(_platformerController != null) {
				CharacterStats.ReduceLife(reducedLifePerSecond * Time.deltaTime);
				if(CharacterStats.CurrentLife == 0) {
					_platformerController.Die();
				}
			}
		}
	}
}
