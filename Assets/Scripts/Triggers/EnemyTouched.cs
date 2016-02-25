using UnityEngine;
using System.Collections;

public class EnemyTouched : MonoBehaviour {
	
	public float enemyStrength = 10.0f;
	
	void OnTriggerEnter(Collider collider) {
		//Reduce life by amount specified in enemyStrength
		CharacterStats.ReduceLife(enemyStrength);
	}

}
