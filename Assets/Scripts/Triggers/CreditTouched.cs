using UnityEngine;
using System.Collections;

public class CreditTouched : MonoBehaviour {
	
	public int creditAmount = 1;
	
	void OnTriggerEnter() {
		//Add credit amount to Character
		CharacterStats.AddCredits(creditAmount);
		this.gameObject.SetActive (false);
	}
}
