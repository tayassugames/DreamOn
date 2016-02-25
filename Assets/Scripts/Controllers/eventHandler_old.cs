using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class eventHandler_old : MonoBehaviour 
	{
	public platformerControl motor;
	public Transform CharacterMesh;
		
	void Start () 
		{
		motor = this.GetComponent("platformerControl") as platformerControl;;
		}
	
	void OnEnable() 
		{
		Gesture.onSwipeE += SwipeHandler;
		Gesture.onLongTapE += TapHandler;
		}
	
	void OnDisable() 
		{
		Gesture.onSwipeE -= SwipeHandler;
		Gesture.onLongTapE -= TapHandler;
		}

			
	private void SwipeHandler(SwipeInfo swipeInfo) 
		{	
		if( 0.1f < (1-(swipeInfo.startPoint.x/swipeInfo.endPoint.x)) &&
			        (1-(swipeInfo.startPoint.x/swipeInfo.endPoint.x)) < 0.9f)
			{
			if(swipeInfo.startPoint.x < swipeInfo.endPoint.x) 
				{
				Debug.Log("Derecha " + swipeInfo.startPoint.x + " " + swipeInfo.endPoint.x + " " +  (swipeInfo.duration));
				motor.DoMove(1,Vector2.zero);
				}
			}
		
		if( 0.1f < (1-(swipeInfo.endPoint.x/swipeInfo.startPoint.x)) &&
			        (1-(swipeInfo.endPoint.x/swipeInfo.startPoint.x)) < 0.9f)
			{
			if(swipeInfo.startPoint.x > swipeInfo.endPoint.x) 
				{
				Debug.Log("Izquierda " + swipeInfo.startPoint.x + " " + swipeInfo.endPoint.x + " " +  (swipeInfo.duration));
				motor.DoMove(-1,Vector2.zero);
				}
			}
		
		
		
		if( 0.25f < (1-(swipeInfo.startPoint.y/swipeInfo.endPoint.y)) &&
			        (1-(swipeInfo.startPoint.y/swipeInfo.endPoint.y)) < 0.75f)
			{
			if(swipeInfo.duration < 0.1) 
				{
				// Swipe Rápido resultando en un Hop
				motor.DoJump(0.5f,swipeInfo.endPoint);
				}
			else
				{
				// Swipe Lento resultando en un Jump
				motor.DoJump(1,swipeInfo.endPoint);
				}
			}
		else 
			{
			// Swipe hacia abajo resultado en un slide
			motor.DoSlide(1,Vector2.zero);
			}
		}
	
	private void TapHandler(Vector2 pos)
		{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
			if(hit.collider.transform==CharacterMesh)
				{
				Debug.Log("HitCharacter");
				}
			}
		}
	}
