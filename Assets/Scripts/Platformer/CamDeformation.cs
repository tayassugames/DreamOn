using UnityEngine;
using System.Collections;

public class CamDeformation : MonoBehaviour {
	
	//public perpCam persp;

	void Start () {
	
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit){
		if (hit.gameObject.tag == "Player"){
			print ("start deformation on camera");
		}
	}
	
	void Update () {
	
	}
}
