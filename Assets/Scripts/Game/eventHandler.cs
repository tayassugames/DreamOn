// Navarro Touch Device Platformer System (NATDEP)

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class eventHandler : MonoBehaviour {

	public platformerControl motor;
	public Transform CharacterMesh;
	public double ScreenHeight = Screen.height;
	public bool useFeedback = false;
	
	public double fireRate = 0.5F;
    private double nextFire = 0.0F;
	
	
	void Awake() {

	}
	
	void Start () {
		useFeedback = useFeedback && (FeedbackLabel.Instance != null);
	}



	void Update () {
		// Controles WASD
		
		if (Input.GetKey("d") && Time.time > nextFire) { Right(); }
		if (Input.GetKey("a") && Time.time > nextFire) { Left(); }
		if (Input.GetKey("w") && Time.time > nextFire) { Up(); }
		if (Input.GetKey("s") && Time.time > nextFire) { Down(); }
		
		// Flechas de teclado
		
		if (Input.GetKey("right") && Time.time > nextFire) { Right(); }
		if (Input.GetKey("left")  && Time.time > nextFire) { Left(); }
    	if (Input.GetKey("up")    && Time.time > nextFire) { Up(); }
		if (Input.GetKey("down")  && Time.time > nextFire) { Down(); }

		// Detener con space
		
		if (Input.GetKey ("space")  && Time.time > nextFire) { Stop(); }
	}

	public void Left() {
		nextFire = Time.time + fireRate;
		motor.DoMove(-1,Vector2.zero);
	}

	public void Right() {
		nextFire = Time.time + fireRate;
		motor.DoMove(1,Vector2.zero);
	}

	public void Up() {
		nextFire = Time.time + fireRate;
		motor.DoJump(1,Vector2.zero);
	}

	public void Down() {
		nextFire = Time.time + fireRate;
		motor.DoSlide (1,Vector2.zero);
	}

	public void Stop() {
		nextFire = Time.time + fireRate;
		motor.DoStop();
	}

	void OnEnable() 
	{
		// se crean la funciones de swipeHandler y TapHandler para manejar el personaje
		Gesture.onSwipeE += SwipeHandler;
		Gesture.onShortTapE += TapHandler;
	}
	
	void OnDisable() 
	{
		Gesture.onSwipeE -= SwipeHandler;
		Gesture.onShortTapE -= TapHandler;
	}
	
	private void SwipeHandler(SwipeInfo swipeInfo) 
	{		
		if(useFeedback) {
			FeedbackLabel.Instance.SetText (swipeInfo.angle.ToString(), 0);
			FeedbackLabel.Instance.SetText (swipeInfo.direction.ToString(), 1);
			FeedbackLabel.Instance.SetText (swipeInfo.duration.ToString(), 2);
		}
		
		
		// Swipe derecho a
		if(swipeInfo.angle < 20 && swipeInfo.angle >= 0) {
			if(useFeedback) {
				FeedbackLabel.Instance.SetText ("right swipe a", 3);	
			}
			motor.DoMove(1,swipeInfo.endPoint);
		}
		
		// Swipe drecho b
		if(swipeInfo.angle <= 360 && swipeInfo.angle > 340) {
			if(useFeedback) {
				FeedbackLabel.Instance.SetText ("right swipe b", 3);	
			}
			motor.DoMove(1,swipeInfo.endPoint);
		}
		
		// Swipe a la izquierda
		if(swipeInfo.angle > 160 && swipeInfo.angle < 200) {
			if(useFeedback) {
				FeedbackLabel.Instance.SetText ("left swipe", 3);	
			}
			motor.DoMove(-1,swipeInfo.endPoint);
		}
		
		
		// Swipe arriba
		if(swipeInfo.angle < 150 && swipeInfo.angle > 30) {
			if(useFeedback) {
				FeedbackLabel.Instance.SetText ("up swipe", 3);	
			}
			//Debug.Log(swipeInfo.angle);
			motor.DoJump(1,swipeInfo.endPoint);
		}
		
		// Swipe abajo
		if(swipeInfo.angle > 240 && swipeInfo.angle < 300) {
			if(useFeedback) {
				FeedbackLabel.Instance.SetText ("down swipe", 3);	
			}
			//Debug.Log(swipeInfo.angle);
			motor.DoSlide(1f,swipeInfo.endPoint);
		}
	}
	
	
	// metodo para detectar el tap en el screen
	private void TapHandler(Vector2 pos) {
		if(useFeedback) {
			FeedbackLabel.Instance.SetText ("tap", 2);	
		}
		
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, 100)) {
			if(hit.collider.gameObject.name == "StopCube") {
				motor.DoStop();
			} 
		}
		
	}
}
