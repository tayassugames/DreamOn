using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {
	
	public Transform target;
	public Vector3 offset = Vector3.zero;
	
	private Transform _cachedTransform;
	
	// Use this for initialization
	void Start () {
		_cachedTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		_cachedTransform.position = target.position + offset;
	}
}
