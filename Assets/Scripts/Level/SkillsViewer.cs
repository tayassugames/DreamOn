using UnityEngine;
using System.Collections;

public class SkillsViewer : MonoBehaviour {
	
	/*
 		SPRINT = 0,
		DOUBLE_JUMP = 1,
		SLIDE = 2,
		WALL_SLIDE = 3,
		ATTACK = 4

	 * 
	 */
	
	private bool _showButtons = false;
	
	public bool sprint;
	public bool doubleJump;
	public bool slide;
	public bool wallSlide;
	public bool attack;
	
	public bool sprintSet;
	public bool doubleJumpSet;
	public bool slideSet;
	public bool wallSlideSet;
	public bool attackSet;
	
	void Start() {
		sprint = true;
		doubleJump = true;
		slide = true;
		wallSlide = true;
		attack = true;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		sprint = CharacterStats.GetSkill(CharacterStats.NonBasicCharacterSkill.SPRINT);
		doubleJump = CharacterStats.GetSkill(CharacterStats.NonBasicCharacterSkill.DOUBLE_JUMP);
		slide = CharacterStats.GetSkill(CharacterStats.NonBasicCharacterSkill.SLIDE);
		wallSlide = CharacterStats.GetSkill(CharacterStats.NonBasicCharacterSkill.WALL_SLIDE);
		attack = CharacterStats.GetSkill(CharacterStats.NonBasicCharacterSkill.ATTACK);
		
		if(sprintSet) {
			CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.SPRINT, !sprint);
			sprintSet = false;
		}
		if(doubleJumpSet) {
			CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.DOUBLE_JUMP, !doubleJump);
			doubleJumpSet = false;
		}
		if(slideSet) {
			CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.SLIDE, !slide);
			slideSet = false;
		}
		if(wallSlideSet) {
			CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.WALL_SLIDE, !wallSlide);
			wallSlideSet = false;
		}
		if(attackSet) {
			CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.ATTACK, !attack);
			attackSet = false;
		}
		
	}
	
	void OnGUI() {
		if(!_showButtons) {
			if(GUI.Button(new Rect(Screen.width - 70, 100, 65, 50), "Skills")) {
				_showButtons = true;
				
			}
		} else { 
			if(GUI.Button(new Rect(10, 100, 65, 50), "Sprint")) {
				_showButtons = false;
				CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.SPRINT, !sprint);
			}
			if(GUI.Button(new Rect(80, 100, 65, 50), "DJump")) {
				_showButtons = false;
				CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.DOUBLE_JUMP, !doubleJump);
			}
			if(GUI.Button(new Rect(150, 100, 65, 50), "Slide")) {
				_showButtons = false;
				CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.SLIDE, !slide);
			}
			if(GUI.Button(new Rect(220, 100, 65, 50), "WallSl")) {
				_showButtons = false;
				CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.WALL_SLIDE, !wallSlide);
			}
			if(GUI.Button(new Rect(290, 100, 65, 50), "Attack")) {
				_showButtons = false;
				CharacterStats.SetSkill(CharacterStats.NonBasicCharacterSkill.ATTACK, !attack);
			}
			if(GUI.Button(new Rect(360, 100, 65, 50), "Exit")) {
				_showButtons = false;
			}
				
		}
		
	}
}
