using UnityEngine;
using System.Collections;

public class SkillItemTouched : MonoBehaviour {
	
	public CharacterStats.NonBasicCharacterSkill skill = CharacterStats.NonBasicCharacterSkill.SPRINT;
	public bool skillSetting = true;
	public bool saveImmediately = false;

	
	// Use this for initialization
	void OnTriggerEnter(Collider collider) {
		CharacterStats.SetSkill (skill, skillSetting);
		if(saveImmediately) {
			CharacterStats.SaveStats();	
		}
	}
}
