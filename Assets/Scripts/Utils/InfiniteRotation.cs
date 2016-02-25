using UnityEngine;
using System.Collections;

public class InfiniteRotation : MonoBehaviour {
	
	
	private Transform _transform;
	
	
	public Vector3 rotationAxis = new Vector3(0f, 1f, 0f);
	/// <summary>
	/// The rotation speed in degrees per second.
	/// </summary>
	public float rotationSpeed = 90;
	public bool isEnabled = true;
	
	
	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isEnabled) {
			_transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
		}
	}
}
