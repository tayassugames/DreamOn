using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class eventHandlerWinLevel : MonoBehaviour 
	{
	
	void Start () 
		{
			
		}
	
	void Update () 
		{
		
		}
	
	void OnEnable() 
	{
		Gesture.onShortTapE += TapHandler;
	}
	
	void OnDisable() 
	{
		Gesture.onShortTapE -= TapHandler;
	}
	

	private void TapHandler(Vector2 pos)
		{
		Application.LoadLevel("SphericWorld");
		}
	}
