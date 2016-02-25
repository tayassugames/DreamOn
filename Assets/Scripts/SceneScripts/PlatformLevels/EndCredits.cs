using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndCredits : MonoBehaviour {
	
	public GameObject quetzaObject;

	public List<Vector3> positions;
	public List<Transform> transforms;
	
	
	// Use this for initialization
	void Start () {
		//iTween.Init(quetzaObject);
		
		
		
		//Hashtable parameters = iTween.Hash("path", positions, "orienttopath", true, "time", 10);
		//iTween.MoveTo(gameObject,iTween.Hash("path",path,"time",7,"orienttopath",true,"looktime",.6,"easetype","easeInOutSine","oncomplete","complete"));	
		//iTween.MoveTo(quetzaObject, iTween.Hash("path",transforms.ToArray(),"time",15,"orienttopath",true,"looktime",.6,"easetype","easeInOutSine"));	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
