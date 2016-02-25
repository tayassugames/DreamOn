/* GameStar Project 2012
 * 2.5d camera smoothing script. Written by Anthony Najjar
 * Last Updated: June 19, 2012
 */

using UnityEngine;
using System.Collections;

public class CameraSmoothing : MonoBehaviour {
	
	public bool smoothingEnabled = true;
	public Transform target;
	public CharacterStats characterStats;
	public float smoothing;
	private float currentVelocity;
	public float zDistance;
	/// <summary>
	/// The x tolerance for determining direction.
	/// </summary>
	public float xTolerance = 0.05f; 
	public float smoothingAccelerationPercentage = 0.05f;
	public float maxFallDistanceIgnore = 5;
	
	
	public float xOffset = 0.26f;
	public float yOffset = 0.26f;
	public float verticalMovementOffset = 2.0f;
	public bool useFeedback = false;
	//public float maxSmoothingDistance = 20.0f;
	
	private Vector3 _previousTargetPosition;
	private int _cameraDirection;
	//private float _currentVerticalMovementOffset = 0.0f;
	//private float _sqrMaxSmoothingDistance = 0.0f;
	
	// iPhone optimization
	private Transform _thisTransform;
	private Transform _cachedTarget;
	private float _currentSmoothing;
	private Vector3 targetPosition;
	private float sqrDistance;
	private Vector3 intermediatePosition;
	private bool camUnlock;

	void Start () {
		camUnlock = false;
		_thisTransform = transform;
		_cachedTarget = target;
		_previousTargetPosition = _cachedTarget.position;
		_currentSmoothing = 0;
		useFeedback = useFeedback && (FeedbackLabel.Instance != null);
		//_sqrMaxSmoothingDistance = maxSmoothingDistance * maxSmoothingDistance; //Performance tweak
	}
	
	//Smooth camera follow by reference float and player transform
	void LateUpdate () {

		switch(CharacterStats.CurrentState) {
			case States.Jump:
			CameraJumpMode(); 
			break;
			
			case States.DoubleJump:
			CameraJumpMode(); 
			break;
			
			case States.Fall:
			if(CharacterStats.oldState == States.WallKick) {
				CameraStaticMode(); 
			} 
			else {
				CameraJumpMode(); 
			}

			break;

			case States.Glide:
			CameraStaticMode(); 
			break;

			case States.WallKick:
			CameraStaticMode(); 
			break;

			default: 
			CameraDefaultMode();
			break; 
		}
	}

	private void ConditionalDirection() {
		//Change direction 
		if( _previousTargetPosition.x < _cachedTarget.position.x) {
			//Was moving in another direction
			if(_cameraDirection != 1) {
				_currentSmoothing = 0;	
			}
			
			//Is moving right
			_cameraDirection = 1;	
		} 
		
		else if(_previousTargetPosition.x > _cachedTarget.position.x) {
			//Was moving in another direction
			if(_cameraDirection != -1) {
				_currentSmoothing = 0;	
			}
			
			//Is moving left
			_cameraDirection = -1;
		} 
		
		else if(Mathf.Abs(_previousTargetPosition.y - _cachedTarget.position.y) > 0.1f) {
			//Was moving in another direction
			if(_cameraDirection != 0) {
				_currentSmoothing = 0;	
			}
			
			// Is falling
			_cameraDirection = 0;
		}
	}

	private void CameraDefaultMode() {
		camUnlock = false;
		if(smoothingEnabled) {
			if(Time.timeScale > 0) {

				ConditionalDirection();
				
				targetPosition = new Vector3(_cachedTarget.position.x + (xOffset * _cameraDirection), _cachedTarget.position.y + yOffset, zDistance);
				
				sqrDistance = Vector3.SqrMagnitude(_thisTransform.position - targetPosition);
				
				if(useFeedback) {
					FeedbackLabel.Instance.SetText (sqrDistance.ToString(), 0);
				}				
				
				if(_currentSmoothing < smoothing) {
					_currentSmoothing = Mathf.Min(smoothing, _currentSmoothing + (smoothing * smoothingAccelerationPercentage));	
				} 
				else if(sqrDistance > 15f) { // _sqrMaxSmoothingDistance) 
					_currentSmoothing = Mathf.Min(1.0f, _currentSmoothing + Time.deltaTime);
					
					if(useFeedback) {
						FeedbackLabel.Instance.SetText (_currentSmoothing.ToString(), 1);
					}
				}
				
				intermediatePosition = Vector3.Lerp(_thisTransform.position, targetPosition, _currentSmoothing);
				
				_thisTransform.position = intermediatePosition;
				_previousTargetPosition = _cachedTarget.position;
			}
		}
	}

	private void CameraStaticMode() {
		camUnlock = false;
		if(smoothingEnabled) {
			if(Time.timeScale > 0) {
				
				_currentSmoothing = 0.01f;	
				_cameraDirection = 0;
				
				targetPosition = new Vector3(this.transform.position.x, _cachedTarget.position.y, zDistance);
				
				//sqrDistance = Vector3.SqrMagnitude(_thisTransform.position - targetPosition);
				
				if(useFeedback) {
					FeedbackLabel.Instance.SetText (sqrDistance.ToString(), 0);
				}				
				
				if(_currentSmoothing < smoothing) {
					_currentSmoothing = Mathf.Min(smoothing, _currentSmoothing + (smoothing * smoothingAccelerationPercentage));	
				} 
				else if(sqrDistance > 15f) { // _sqrMaxSmoothingDistance) 
					_currentSmoothing = Mathf.Min(1.0f, _currentSmoothing + Time.deltaTime);
					
					if(useFeedback) {
						FeedbackLabel.Instance.SetText (_currentSmoothing.ToString(), 1);
					}
				}
				
				intermediatePosition = Vector3.Lerp(_thisTransform.position, targetPosition, _currentSmoothing);
				
				_thisTransform.position = intermediatePosition;
				_previousTargetPosition = _cachedTarget.position;
			}
		}
	}

	private void CameraJumpMode() {
		if(smoothingEnabled) {
			if(Time.timeScale > 0) {

				ConditionalDirection();
				_currentSmoothing = 0.1f;	

				if( camUnlock || Mathf.Abs( Mathf.Abs(_cachedTarget.position.y) - Mathf.Abs(this.transform.position.y) ) >= maxFallDistanceIgnore ) {
					targetPosition = new Vector3(_cachedTarget.position.x + (xOffset * _cameraDirection), _cachedTarget.position.y, zDistance);
					camUnlock = true;
				}

				else {
					targetPosition = new Vector3(_cachedTarget.position.x + (xOffset * _cameraDirection), this.transform.position.y, zDistance);
				}


				sqrDistance = Vector3.SqrMagnitude(_thisTransform.position - targetPosition);
				
				if(useFeedback) {
					FeedbackLabel.Instance.SetText (sqrDistance.ToString(), 0);
				}				
				
				if(_currentSmoothing < smoothing) {
					_currentSmoothing = Mathf.Min(smoothing, _currentSmoothing + (smoothing * smoothingAccelerationPercentage));	
				} 

				else if(sqrDistance > 15f) { // _sqrMaxSmoothingDistance) 
					_currentSmoothing = Mathf.Min(1.0f, _currentSmoothing + Time.deltaTime);
					
					if(useFeedback) {
						FeedbackLabel.Instance.SetText (_currentSmoothing.ToString(), 1);
					}
				}
				
				intermediatePosition = Vector3.Lerp(_thisTransform.position, targetPosition, _currentSmoothing);
				
				_thisTransform.position = intermediatePosition;
				_previousTargetPosition = _cachedTarget.position;
			}
		}
	}
}
