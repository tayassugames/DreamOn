using UnityEngine;
using System.Collections;

public class CreditScrollingHandler : MonoBehaviour {
	
	private Vector3 temp;
	public float textSpeed = 0.8f; //Controls the text rolling speed
	// Use this for initialization
	void Start () {
	
	//textSpeed 
	
	}
	// Update is called once per frame
	void FixedUpdate () {
		
		temp = this.transform.localPosition;
		
		if(this.transform.localPosition.y < 250f )
		{
			temp.y += textSpeed;	
		}
			
		//	temp.y = -255;
		//Cambie el texto
		// si volvio al principio
			
			//or vuelva al principio
			
		//}
		
		this.transform.localPosition = temp;
		
	}
	
	
}
