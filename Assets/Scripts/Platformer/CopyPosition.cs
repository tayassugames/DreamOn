using UnityEngine;
using System.Collections;

public class CopyPosition : MonoBehaviour {

	public Transform Target;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(Target.position.x, Target.position.y, Target.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Slerp(Target.position,transform.position, Time.deltaTime);
		//transform.position = new Vector3(Target.position.x, Target.position.y, Target.position.z);
		//transform.localScale(1,1,1);
	}
}
