using UnityEngine;
using System.Collections;

public class SkyScrolling : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 temp = this.transform.position;
		temp.x -= 0.003f;
		temp.y -= 0.001f;
		this.transform.position = temp;
		
	}
}
