using UnityEngine;
using System.Collections;

public class AutoMovement : MonoBehaviour {
	public States DesiredAction = States.Run;
	public CharDirecctions Direcction = CharDirecctions.right;

	private platformerControl _character;
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			_character = other.GetComponent("platformerControl") as platformerControl;

			if(DesiredAction == States.Run) {
				_character.DoMove(1,Vector2.zero);
			} else if (DesiredAction == States.Jump) {
				_character.DoJump(1f,Vector2.zero);
			} else if (DesiredAction == States.Slide) {
				_character.DoSlide(-1,Vector2.zero);
			}

			_character = null;
		}
	}
}
