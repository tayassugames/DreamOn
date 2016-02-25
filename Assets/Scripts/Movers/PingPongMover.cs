using UnityEngine;
using System.Collections;

public class PingPongMover : MonoBehaviour {
	
	#region Events
	public delegate void AdvancementHandler(int row);
	public event AdvancementHandler OnAdvance;
	
	public delegate void StartAdvanceHandler();
	public event StartAdvanceHandler OnStartAdvance;
	#endregion
	
	public float movementSpeed = 0.5f;
	public float movementDistance = 1.0f;
	public float distanceToAdvance = 4.0f;
	public int cyclesToAdvance = 2;
	public int currentRow = 0;
	public bool isAdvancing = false;
	public float maxVerticalPosition = 80.0f;
	
	private float _movementDirection = 1.0f;
	private float _totalMovement = 0;
	private int _halfCycleCounter = 0;
	private bool _canMove = false;
	private Transform _cachedTransform;
	
	private float _currentAdvancementDistance = 0;
	
	void Start() {
		_cachedTransform = transform;	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!_canMove) {
			return;	
		}
		
		Vector3 currentPos = transform.position;
		if(isAdvancing) {
			if(_currentAdvancementDistance >= distanceToAdvance 
				|| maxVerticalPosition <= _cachedTransform.position.y + distanceToAdvance) {
				//Finish advancing	
				isAdvancing = false;
				
				if (maxVerticalPosition > _cachedTransform.position.y)
					currentRow++;
				
				if(OnAdvance != null) {
					OnAdvance(currentRow);	
				}
				
			} else {
				//Keep moving ahead
				_currentAdvancementDistance += movementSpeed * Time.deltaTime;
				currentPos.y += movementSpeed * Time.deltaTime;
				
			}
			
		} else {
			_totalMovement += movementSpeed * _movementDirection * Time.deltaTime;
			
			if(Mathf.Abs(_totalMovement) > movementDistance) {
				_movementDirection *= -1;
				_halfCycleCounter++;
				if(_halfCycleCounter / 2 >= cyclesToAdvance) {
					_halfCycleCounter = 0;
					_currentAdvancementDistance = 0;
					isAdvancing = true;
					
					if(OnStartAdvance != null) {
						OnStartAdvance();	
					}
				}
			}
			currentPos.x += movementSpeed * _movementDirection * Time.deltaTime;
			
		}
		transform.position = currentPos;
		
	}
	
	public void StartMoving() {
		_canMove = true;
	}
	
	public void StopMoving() {
		_canMove = false;
	}
	
}
