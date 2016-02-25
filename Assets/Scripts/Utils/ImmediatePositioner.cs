using UnityEngine;
using System.Collections;

public class ImmediatePositioner : MonoBehaviour {
	
	public string targetName;
	public Vector3 offset;
	
	private GameObject _target;
	
	void Start() {
		_target = GameObject.Find (targetName);
	}
	
	public void Relocate() {
		if(_target != null) {
			gameObject.transform.position = _target.transform.position + offset;
		}
	}
	
}
