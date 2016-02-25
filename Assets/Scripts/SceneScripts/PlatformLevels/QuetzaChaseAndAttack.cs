using UnityEngine;
using System.Collections;
using System;

public class QuetzaChaseAndAttack : MonoBehaviour {
	
	public Transform target;
	public float verticalDistance = 10f;
	public float horizontalDistance = 10f;
	public float startUpHorizontalDistance = -20;
	public float minimumHorizontalOffset = 11;
	public float acceleration = 9f;
	public float smoothingFactor = 0.5f;
	public float maxSpeed = 10f;
	public float attackSpeed = 8;
	public bool canMove = false;
	public bool canAnimate = true;
	public ParticleSystem fireParticles;
	public float maximumXPosition = 252;
	public Transform spawnPoint;
	public FireTrigger fireTrigger;
	public Animation quetzaAnimation;
	public float animationFadeTime = 0.3f;
	public AudioSource audioSource;
	public AudioFader audioFader;
	public AudioClip fireSound;
	public AudioClip roarSound;
	
	private float _currentSpeed;
	private Transform _cachedTransform;
	private Vector3 _offsetVector;
	private Vector3 _startUpVector;
	private Vector3 _targetPosition;
	private Vector3 _targetDirection;
	private bool _isAttacking = false;
	private bool _isInAttack = false;
	private bool _canAttack = true;
	//private float _distance;
	private eventHandler _eventHandler;
	private platformerControl _platformerControl;
	private string _currentAnimationName;
	private bool _playedRoar = false;

	
	// Use this for initialization
	void Start () {
		_cachedTransform = transform;
		_offsetVector = new Vector3(horizontalDistance, verticalDistance, 0);
		_startUpVector = new Vector3(startUpHorizontalDistance, verticalDistance, 0);
		_currentSpeed = 0;
		_eventHandler = target.gameObject.GetComponent<eventHandler>();
		if(_eventHandler == null)
			throw new NullReferenceException("Event handler not found");
		_platformerControl = target.gameObject.GetComponent<platformerControl>();
		if(_platformerControl == null)
			throw new NullReferenceException("Event handler not found");
		
		_currentAnimationName = "flyidle_custom";
	}
	
	void OnEnable() {
		CharacterStats.OnLifeLost += HandleOnLifeLost; 
	}

	private void HandleOnLifeLost (int remaining)
	{
		//Relocate
		_isInAttack = false;
		_isAttacking = false;
		_canAttack = true;
		fireParticles.enableEmission = false;
		_cachedTransform.position = spawnPoint.position + _startUpVector;
	}

	void OnDisable() {
		CharacterStats.OnLifeLost -= HandleOnLifeLost;
	}

	
	// Update is called once per frame
	void Update () {
		if(canAnimate) {
			quetzaAnimation.CrossFade(_currentAnimationName);
		}
	}
	
	void FixedUpdate() {
		if(!canMove)
			return;
		
		//Move
		//Target direction
		_targetPosition = target.position + _offsetVector - _cachedTransform.position;
		_targetDirection = _targetPosition.normalized;
		
		//Check if reached limit
		if(_cachedTransform.position.x < maximumXPosition) {
			//check if reached target
			if(_targetPosition.sqrMagnitude > 1 && !_isAttacking) {
				//Accelerate
				if(_currentSpeed < maxSpeed) {
					_currentSpeed += Mathf.Min(maxSpeed * Time.deltaTime, maxSpeed - _currentSpeed);
				}
				
				//Move only forward
				if(_targetDirection.x > 0) {
					_cachedTransform.Translate(_targetDirection * _currentSpeed * Time.deltaTime);
				}

			} else if(target.position.x < _cachedTransform.position.x + minimumHorizontalOffset) {
				//Death
				canMove = false;
				_canAttack = false;
				_eventHandler.enabled = false;
				_platformerControl.DoStop();
				StartCoroutine(DoKill());
				
			} else if(_canAttack) {
				//Play roar
				//audioSource.PlayOneShot (roarSound);
				
				//Reduce speed
				_currentSpeed = attackSpeed;
				if(_targetDirection.x > 0) {
					_cachedTransform.Translate(_targetDirection * _currentSpeed * Time.deltaTime);
				}
				//Attack
				if(!_isInAttack) {
					StartCoroutine(DoAttack());
				}
			}
		} else {
			if(!_playedRoar) {
				_playedRoar = true;
				audioSource.PlayOneShot(roarSound);
				audioFader.fadeDirection = AudioFader.FadeDirection.FadeOut;
				audioFader.Fade();
			}
		}
		
	}
	
	private IEnumerator DoKill() {
		_platformerControl.DoStop();
		Vector3 targetPos = (_cachedTransform.position + new Vector3(-10, -2, 0));
		Vector2 targetDirection = (targetPos -_cachedTransform.position).normalized;
		
		while((_cachedTransform.position - targetPos).sqrMagnitude > 1) {
			_cachedTransform.Translate(targetDirection * _currentSpeed * Time.deltaTime);
			yield return null;
		}
		
		_currentAnimationName = "attack_custom";

		//Enable particle system
		fireParticles.enableEmission = true;
		fireTrigger.isEnabled = true;
		
		audioSource.Play ();
		while(CharacterStats.CurrentLife > 0) {
			CharacterStats.ReduceLife(40 * Time.deltaTime);
			yield return null;
		}
		
		audioSource.Stop();
		_platformerControl.Die();
		fireParticles.enableEmission = false;
		fireTrigger.isEnabled = false;
		_currentAnimationName = "flyidle_custom";

		_eventHandler.enabled = true;
		canMove = true;
	}
	
	private IEnumerator DoAttack() {
		
		float timer = 0;
		
		_isAttacking = true;
		_isInAttack = true;
		//Play attack animation
		_currentAnimationName = "attack_custom";
		
		//Enable particle system
		fireParticles.enableEmission = true;
		fireTrigger.isEnabled = true;
		
		audioSource.Play();
		while(timer < 3f && _canAttack) {
			timer += Time.deltaTime;
			yield return null;
		}

		fireParticles.enableEmission = false;
		fireTrigger.isEnabled = false;
		audioSource.Stop();
		
		_currentAnimationName = "flyidle_custom";
		
		_isAttacking = false;
		_isInAttack = false;
		
	}
	
	
	public void StartChase() {
		
	}
	
}
