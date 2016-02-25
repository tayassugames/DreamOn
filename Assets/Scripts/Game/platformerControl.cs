// Navarro Touch Device Platformer System (NATDEP)

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class platformerControl : MonoBehaviour {

	// Variables custimizables desde el editor 
	public float jumpSpeedMaster = 20.0f;
	public float WallKickJumpSpeedMaster = 15.0f;
	public float maxFallVelocity = 15.0f;
	public float maxFallGlideVelocity = 2.0f;
	public float maxJumpVerticalDistance = 2.0f;
	public float doubleJumpFactor = 0.8f;
	public float slideFactor = 20.0f;
	public float BaseLimit = 0f;
	public float hitFactor = 20.0f;
	public Transform[] spawnPoints;
	public float gravity = 50.0f;
	public float runFactor = 7f;
	public float sprintFactor = 10f;
	public float sprintMaxDistance = 10;
	public float slideMaxDistance = 5; 
	public float wallkickHorizontalSpeed = 10f;
	public Vector3 slidePositionOffset = new Vector3(0, -1, 0);
	public float verticalRayOffset = 0.4f;
	public Vector3 verticalRayOffsetVector = new Vector3(0.4f, 0, 0);
	public float lifeTakenInDeathZone = 100.0f;
	public float verticalVelosityThreshold = -1.5f;
	public TrailRenderer AndyTrail;

	// Variable privadas que cargan valores en tiempo de ejecución
	private CharacterController _character;
	private Vector3 _verticalVelocity = Vector3.zero;		
	private bool canJump = false;
	private bool canDoubleJump = false;
	private bool canDoubleJumpWallKick = false;
	private bool _queueStop = false;
	private Vector3 slideAcc = Vector3.zero;
	private Vector3 hitAcc = Vector3.zero;
	private float HorizontalDirection = 1f; 
	private Vector3 _horizontalMovement;
	private bool dead = false;
	private bool used = false;
	private float slideLimit;
	private float HitLimit = 3f;
	private bool isDoubleJumping = false;
	private bool isDoubleJumpingWallKick = false;
	private bool isWallKicking = false;
	private GameObject perspCam;
	private Transform _thisTransform;
	private Transform _geometryTransform;
	private Vector3 _unitVector = new Vector3(1, 1, 1);
	private Vector3 _verticalScaleVector = new Vector3(1, 4, 1);
	private Vector3 totalMovement;
	

	void  Start () {
		_horizontalMovement = Vector3.zero;
		_thisTransform = transform;
		_geometryTransform = GameObject.Find("platformerSystem/Character/hero").transform;
		_character = GetComponent< CharacterController >();	

		// Quitar est FIND !!!
		// GameObject spawn = GameObject.Find( "PlayerSpawn" );
		CharacterStats.SetState(States.Fall);
		
		// if (spawn) {
		// 	_thisTransform.position = spawn.transform.position;
		// }
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit) {
		if (hit.gameObject.tag == "DEATH ZONE") {
			CharacterStats.ReduceLife(lifeTakenInDeathZone);
			dead = true;
		}
		
		if (hit.gameObject.tag == "REVERSE" && used == false) {
			used = true;
		}
	}


	private void DebugLines() {
		Debug.DrawRay(new Vector3(_thisTransform.position.x + verticalRayOffset,_thisTransform.position.y+1,_thisTransform.position.z),Vector3.up,Color.red);
		Debug.DrawRay(new Vector3(_thisTransform.position.x - verticalRayOffset,_thisTransform.position.y+1,_thisTransform.position.z),Vector3.up,Color.yellow);
		Debug.DrawRay(new Vector3(_thisTransform.position.x,_thisTransform.position.y+1,_thisTransform.position.z),Vector3.left,Color.cyan);
		Debug.DrawRay(new Vector3(_thisTransform.position.x,_thisTransform.position.y+1,_thisTransform.position.z),Vector3.right,Color.magenta);
	}

	void Update () {	
		DebugLines();
		
		// operaciones cuando el personaje esta en SLIDE
		// Se accelera al personaje mientras este en Slide y dentro del Slide Limit
		if( CharacterStats.CurrentState == States.Slide ) {
			if(!IsCrouchingTouch()) {
				if(HorizontalDirection > 0 && _thisTransform.position.x >= slideLimit) { // 1 derecha, -1 izquierda 
					StopSlide();
				}
				else if(HorizontalDirection < 0 && _thisTransform.position.x <= slideLimit) { // verifica que la distancia limite se ha completado
					StopSlide();
				}
			}
			if(IsWallTouchLow()) {
				StopSlideToIdle();
			}
		}
		
				
		// Operaciones cuando el presonaje ha recibido daño
		if( CharacterStats.CurrentState == States.Hit ) {		
			if(BaseLimit+HitLimit < _thisTransform.position.x && BaseLimit-HitLimit > _thisTransform.position.x) { // 1 derecha, -1 izquierda 
				DoDirectStop();
				Debug.Log("StopAlarm");
			}
			
		}
		
		// Verifica la rotacion del personaje
		//
		if(HorizontalDirection==1) {
			_thisTransform.rotation = Quaternion.LookRotation(Vector3.forward);
		}
		else {
			_thisTransform.rotation =  Quaternion.LookRotation(Vector3.back);
		}
		
		// operaciones cuando esta en el suelo y no esta en SLIDE
		// si esta en el suelo
		
		if (CharacterStats.CurrentState == States.Jump && _verticalVelocity.y <= 0 ) {
			CharacterStats.SetState(States.Fall);
		}
		
		else if (CharacterStats.CurrentState == States.DoubleJump && _verticalVelocity.y <= 0 ) {
			CharacterStats.SetState(States.Fall);
		}
		
		if (_character.isGrounded && CharacterStats.CurrentState != States.Slide) {
			// verifica que las veolocidades sean correctas excepto para Slide
			switch(CharacterStats.CurrentState) {
				case States.Run: 	_horizontalMovement = Vector3.right * runFactor * HorizontalDirection;
									break;
				case States.Sprint: _horizontalMovement = Vector3.right * sprintFactor * HorizontalDirection;
									break;
			}
			
			// si esta en el suelo no tiene derecho a hacer un DOUBLE JUMP
			isDoubleJumping = false;
			isDoubleJumpingWallKick = false;
			
			// si existe la orden de ejecutar JUMP para velocidad vertical positiva
			if(canJump) {
				if(isWallKicking) {
					// ejecuta velocidad vertical positiva para que el personaje suba
					//currentVerticalLimit = _thisTransform.position.y + maxJumpVerticalDistance;
					_verticalVelocity = Vector3.zero;
					_verticalVelocity.y = WallKickJumpSpeedMaster;
					canJump = false;
					isDoubleJumping = false;
					isWallKicking = false;
				}
				else {
					// ejecuta velocidad vertical positiva para que el personaje suba
					//currentVerticalLimit = _thisTransform.position.y + maxJumpVerticalDistance;
					_verticalVelocity = Vector3.zero;
					_verticalVelocity.y = jumpSpeedMaster;
					canJump = false;
				}
			} 
			else {
				// si no existen orden de aplicar velocidad verical positiva
								
				// si el estado es caer 
				if(CharacterStats.CurrentState == States.Fall || CharacterStats.CurrentState == States.Glide) { //Evaluar si acaba de caer
				
					// TODO: Agregar logica para caidas muy fuertes
					// Cambiar estado a corriendo si hay componente horizontal
					// sino, cambiar a estatico
					
					
					
					// se evalua el desplazamiento horizotal con velocidades verticales positivas y negativas
					if(_horizontalMovement != Vector3.zero)  {
						// si existen desplazamientos horizontales, el personaje entra en esto RUN
						CharacterStats.SetState(States.Run);
					} 
					else {
						// si no existen desplazamientos horizontales el personaje queda en idle.
						CharacterStats.SetState(States.Idle);	
					}
				}				
				//Agregar componente vertical negativa
				//para garantizar que character controller
				//detecte pendientes
				_verticalVelocity.y = -0.01f;
			}
		}
		// si esta en aire subiendo o bajando
		else {
			_verticalVelocity += Vector3.down * gravity * Time.deltaTime;
			//si la velocidad de caida es superior a la maxima, usar maxima
			
			// si esta pegado a una pared la velocidad es menor
			// de lo contrario cae a velocidad normal
			if(CharacterStats.CurrentState == States.Glide) {
				_verticalVelocity.y = Mathf.Max(_verticalVelocity.y, maxFallGlideVelocity * -1);
			}
			else {
				_verticalVelocity.y = Mathf.Max(_verticalVelocity.y, maxFallVelocity * -1);
			}
			
			//Si componente y es menor que 0, esta cayendo
			if(_verticalVelocity.y < verticalVelosityThreshold && CharacterStats.CurrentState != States.Slide ) {
				bool isTouchingWall = IsWallTouch();
				if(isTouchingWall && !isWallKicking) {
					_horizontalMovement = Vector3.zero;
					CharacterStats.SetState(States.Glide);
					//isWallKicking = false;
				}
				else {
					CharacterStats.SetState(States.Fall);
					if(!isTouchingWall && isWallKicking) {
						isDoubleJumping = false;
					}
					isWallKicking = false;
				}
				if ( canDoubleJump ) {
					_verticalVelocity = Vector3.zero;
					_verticalVelocity.y = jumpSpeedMaster * doubleJumpFactor;
					isDoubleJumping = true;
					canDoubleJump = false;
				}
				
				if ( canDoubleJumpWallKick ) {
					_verticalVelocity = Vector3.zero;
					_verticalVelocity.y = jumpSpeedMaster * doubleJumpFactor;
					isDoubleJumpingWallKick = true;
					canDoubleJumpWallKick = false;
				}
			}
		}		
		
		// desacceleración del Slide
		slideAcc = new Vector3(slideAcc.x*0.5f,0,0);
		
		// desacceleración del Hit
		hitAcc = new Vector3(slideAcc.x*0.5f,0,0);
		
		// ejecucicón del movimiento
		totalMovement = _horizontalMovement + _verticalVelocity + slideAcc + hitAcc; // se hace la sumatorias de movimiento
		
		//Correcion forzada de z=0 para deactivar cualquier tipo de salida.
		//_thisTransform.position = new Vector3(transform.position.x, transform.position.y,0);
		
		if ( dead == true) { // Estado de Muerte, Recordar sacar la detecci'on del estado de muerte.
			Transform closestPoint = spawnPoints[0];
			float shortestDistance = Vector3.Distance(_thisTransform.position, closestPoint.position);
			foreach (Transform pos in spawnPoints) {
				float currentDistance = Vector3.Distance(_thisTransform.position, pos.position);
				if(shortestDistance > currentDistance) {
					shortestDistance = currentDistance;
					closestPoint = pos;
					}
				}
			_thisTransform.position = closestPoint.position;
			dead = false;
			CharacterStats.ResetLife();
			_horizontalMovement = Vector3.zero;
		}
		
		_character.Move(totalMovement * Time.deltaTime); // se ejecuta el movimiento
		//_thisTransform.position = new Vector3(_thisTransform.position.x, _thisTransform.position.y, 0);
		
		// Trail Render On/Off
		if (Mathf.Abs(_horizontalMovement.x) >= sprintFactor) {
			AndyTrail.time = 0.25f;
		}
		else {
			AndyTrail.time = 0f;
		}
	}
	

	
	public void DoHit() {
		// validación para que el slide no se llame mientras se esta ejecutando
		if(CharacterStats.CurrentState != States.Hit) {
			CharacterStats.SetState(States.Hit);
			DoDirectStop();
			hitAcc = new Vector3(hitFactor * HorizontalDirection * -1, 0, 0);
			BaseLimit = _thisTransform.position.x;	
		}
	}
	
	
	
	
	public void DoJump(float jumpMagnitude, Vector2 screenPos) {
		// validacion para ejecutar un JUMP para estados que no sean el mismo JUMP
		if(_character.isGrounded && CharacterStats.CurrentState != States.Jump) {			
			// el personaje debe estar en el suelo,
			// si esta en el suelo no debe estar en slide o
			// si esta en slide, no debe estar debajo de un collider
			if(!canJump && _character.isGrounded && (CharacterStats.CurrentState != States.Slide || !IsCrouchingTouch())) {
				// se autoriza a acelerar hacia arriba
				CharacterStats.SetState(States.Jump);
				NormalCollider();
				canJump = true;
			}
		}
		else {
			// si se solicta hacer Jump estado en otros es
			if(CharacterStats.CurrentState == States.Fall) {
				//Se permite el double jump solo si esta habilitado en los stats - 2013-04-30 LDV
				bool isDoubleJumpEnabled = CharacterStats.GetSkill(CharacterStats.NonBasicCharacterSkill.DOUBLE_JUMP);
				if(!isDoubleJumping && !isDoubleJumpingWallKick && isDoubleJumpEnabled) {
					CharacterStats.SetState(States.DoubleJump);
					NormalCollider();
					canDoubleJumpWallKick = true;
					isDoubleJumping = true;
				}
				else if(isDoubleJumping && !isDoubleJumpingWallKick && isDoubleJumpEnabled) {
					CharacterStats.SetState(States.DoubleJump);
					NormalCollider();
					canDoubleJump = true;
					isDoubleJumpingWallKick = true;
				}
			}
		}
	}
	
	public void DoWallKick() {
		bool isWallKickEnabled = CharacterStats.GetSkill (CharacterStats.NonBasicCharacterSkill.WALL_SLIDE);
		if(CharacterStats.CurrentState != States.WallKick && isWallKickEnabled) {
			HorizontalDirection = -1 * IsWallTouchDirection();
			isWallKicking=true;
			isDoubleJumping = false;
			canJump = false;
			NormalCollider();
			canDoubleJump = true;
			_horizontalMovement = Vector3.right * wallkickHorizontalSpeed * HorizontalDirection;
			CharacterStats.SetState(States.WallKick);
		}
	}
	
	public void DoSlide(float slideMagnitude, Vector2 screenPos) {
		bool isSlideEnabled = CharacterStats.GetSkill (CharacterStats.NonBasicCharacterSkill.SLIDE);
		
		// validación para que el slide no se llame mientras se esta ejecutando
		if(CharacterStats.CurrentState != States.Slide && isSlideEnabled) {
			//LDV - 20120816 - Hace slide tambien cuando esta en sprint
			if(_character.isGrounded && (CharacterStats.CurrentState == States.Run || CharacterStats.CurrentState == States.Sprint)) {
				CharacterStats.SetState(States.Slide);
				ShrinkCollider();
				slideAcc = new Vector3(slideFactor * HorizontalDirection, 0, 0);
				
				slideLimit = _thisTransform.position.x + (slideMaxDistance * Mathf.Sign(HorizontalDirection));

			}	
		}
	}

	public void DoStop() {
		// detenerse solo se puede solitar en el suelo
		if(_character.isGrounded && (CharacterStats.CurrentState != States.Slide || !IsCrouchingTouch() || _queueStop)) {			
			// cambia el estado, normaliza el collider y deteiene el movimiento
			CharacterStats.SetState(States.Idle);
			NormalCollider();
			//HorizontalDirection = 0;
			_horizontalMovement = Vector3.zero;
			_verticalVelocity = Vector3.zero;
			slideAcc = Vector3.zero;	
			hitAcc = Vector3.zero;
			_queueStop = false;
		} 
		else {
			_queueStop = true;	
		}
	}
	
	public void DoDirectStop() {
		// detenerse solo se puede solitar en el suelo
		// cambia el estado, normaliza el collider y deteiene el movimiento
		CharacterStats.SetState(States.Idle);
		NormalCollider();
		HorizontalDirection = 0;
		_horizontalMovement = Vector3.zero;
		_verticalVelocity = Vector3.zero;
		slideAcc = Vector3.zero;	
		hitAcc = Vector3.zero;
	}
	
	public void DoMove(float swipeHorizontalDirection, Vector2 screenPos)
	{
		// se guarda la direccion anterior
		float oldHorizontalDirection = HorizontalDirection;

		// estas operaciones solo son válidas si el personaje esta en el piso
		if(_character.isGrounded)
		{	
			bool isSprintEnabled = CharacterStats.GetSkill (CharacterStats.NonBasicCharacterSkill.SPRINT);
			
			// se cambia la nueva direccion
			// si esta haciendo slide no debe cambiar direccion
			if(CharacterStats.CurrentState != States.Slide)
				HorizontalDirection = swipeHorizontalDirection;

			// ¿corre o si esta detenido?
			if(CharacterStats.CurrentState == States.Run || CharacterStats.CurrentState == States.Idle) {
				// evalua un cambio de dirección
				if(CharacterStats.CurrentState == States.Run && HorizontalDirection == oldHorizontalDirection) {
					
					// si solicta hacer un sprint
					if(isSprintEnabled) {
						CharacterStats.SetState(States.Sprint);
						_horizontalMovement = Vector3.right * sprintFactor * swipeHorizontalDirection;
					}
				}
				else {
					// si solicta un cambio de direccion
					CharacterStats.SetState(States.Run);
					_horizontalMovement = Vector3.right * runFactor * swipeHorizontalDirection;
				}
			}
			
			// evalua lenavtarse mientras esta haciendo un slide y no esta bajo un collider
			else if(CharacterStats.CurrentState == States.Slide && !IsCrouchingTouch()) {
				// ejecuta run y normaliza el collider del personaje
				HorizontalDirection = swipeHorizontalDirection;
				CharacterStats.SetState(States.Run);
				_horizontalMovement = Vector3.right * sprintFactor * swipeHorizontalDirection;
				NormalCollider();
			}
			
			
			// evalua cambio de direccion durante un SPRINT 
			else if(CharacterStats.CurrentState == States.Sprint) {
				// se requiere que no haya un cambio de dirección
				if(HorizontalDirection != oldHorizontalDirection) {
					// ejecuta run y normaliza el collider del personaje
					CharacterStats.SetState(States.Run);
					_horizontalMovement = Vector3.right * sprintFactor * swipeHorizontalDirection;
					NormalCollider();
				}
			}
		}
		else {
			if(CharacterStats.CurrentState == States.Glide) {
				DoWallKick();
			}
		}
	}
	
	public void Die() {
		dead = true;	
	}
	
	#region Private Methods
	
	private void ShrinkCollider() {
		//_character.height = _character.height/4;
		//_character.center += new Vector3(0f, -0.4f, 0f);
		
		_thisTransform.localScale = new Vector3(transform.localScale.x,0.25f,1f);
		_geometryTransform.localScale = _verticalScaleVector;
		
		//Bajar geometria para que se vea el barrido bien
		_geometryTransform.localPosition += slidePositionOffset;
	}
	
	private void NormalCollider() {
		//_character.height = _character.height * 4;
		_thisTransform.localScale = new Vector3(transform.localScale.x,1,1);
		_geometryTransform.localScale = _unitVector;
		
		//Restaurar posicion de geometria
		_geometryTransform.localPosition = Vector3.zero;
	}

	private bool IsCrouchingTouch() {
		//LDV - 20120816 - Usa dos rayos para dar mas espacio al salir de un espacio estrecho
		if (Physics.Raycast(_thisTransform.position + verticalRayOffsetVector, Vector3.up, 1)) {
			return true;
		}
		if (Physics.Raycast(_thisTransform.position - verticalRayOffsetVector, Vector3.up, 1)) {
			return true;
		}
		return false;
	}
	
	private bool IsWallTouch() {
		//Debe considerar la direccion
		if(HorizontalDirection < 0) {
			if (Physics.Raycast(new Vector3(_thisTransform.position.x,_thisTransform.position.y+1,_thisTransform.position.z), Vector3.left, 0.75f)) {
				return true;
			}
		}
		if(HorizontalDirection > 0) {
			if (Physics.Raycast(new Vector3(_thisTransform.position.x,_thisTransform.position.y+1,_thisTransform.position.z), Vector3.right, 0.75f)) {
				return true;
			}
		}
		return false;
	}
	
	private bool IsWallTouchLow() {
		//Debe considerar la direccion
		if(HorizontalDirection < 0) {
			if (Physics.Raycast(new Vector3(_thisTransform.position.x,_thisTransform.position.y,_thisTransform.position.z), Vector3.left, 0.75f)) {
				return true;
			}
		}
		if(HorizontalDirection > 0) {
			if (Physics.Raycast(new Vector3(_thisTransform.position.x,_thisTransform.position.y,_thisTransform.position.z), Vector3.right, 0.75f)) {
				return true;
			}
		}
		return false;
	}
	
	private int IsWallTouchDirection() {
		if (Physics.Raycast(new Vector3(_thisTransform.position.x,_thisTransform.position.y+1,_thisTransform.position.z), Vector3.left, 0.75f)) {
			return -1;
		}
		else if (Physics.Raycast(new Vector3(_thisTransform.position.x,_thisTransform.position.y+1,_thisTransform.position.z), Vector3.right, 0.75f)) {
			return 1;
		}
		else { 
			return 0; 
		}
	}
	
	private void StopSlide() {
		slideAcc = Vector3.zero;
		DoMove(HorizontalDirection,Vector2.zero);
		NormalCollider();
	}
	
	private void StopSlideToIdle() {
		slideAcc = Vector3.zero;
		DoStop();
		NormalCollider();
	}
	
	
	
	#endregion
}
