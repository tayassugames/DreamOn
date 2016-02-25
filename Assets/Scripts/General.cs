using UnityEngine;
using System.Collections;

public class General : MonoBehaviour {

	private Vector2 lastPos;
	private bool dragging=false;
	private bool draggingInitiated=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//#if UNITY_IPHONE || UNITY_ANDROID
		
		if(Input.touchCount>0){
			foreach(Touch touch in Input.touches){
				if(touch.phase==TouchPhase.Began) Gesture.OnTouchDown(touch.position);
				else if(touch.phase==TouchPhase.Ended) Gesture.OnTouchUp(touch.position);
				else Gesture.OnTouch(touch.position);
			}
		}
		
		if(Input.GetMouseButtonDown(0)) Gesture.OnMouse1Down(Input.mousePosition);
		else if(Input.GetMouseButtonUp(0)) Gesture.OnMouse1Up(Input.mousePosition);
		else if(Input.GetMouseButton(0)) Gesture.OnMouse1(Input.mousePosition);
		
		if(Input.GetMouseButtonDown(1)) Gesture.OnMouse2Down(Input.mousePosition);
		else if(Input.GetMouseButtonUp(1)) Gesture.OnMouse2Up(Input.mousePosition);
		else if(Input.GetMouseButton(1)) Gesture.OnMouse2(Input.mousePosition);
		
		
		//drag event detection goes here
		InputEvent inputEvent=new InputEvent();
		
		if(Input.touchCount==1){
			Touch touch=Input.touches[0];
			_InputState state;
			if(touch.phase==TouchPhase.Began) state=_InputState.Down;
			else if(touch.phase==TouchPhase.Ended) state=_InputState.Up;
			else state=_InputState.On;
			inputEvent=new InputEvent(touch.position, _InputType.Touch, state);
		}
		else if(Input.touchCount==0){
			if(Input.GetMouseButtonDown(0)){
				inputEvent=new InputEvent(Input.mousePosition, _InputType.Mouse1, _InputState.Down);
			}
			else if(Input.GetMouseButton(0)){
				inputEvent=new InputEvent(Input.mousePosition, _InputType.Mouse1, _InputState.On);
			}
			else if(Input.GetMouseButtonUp(0)){
				inputEvent=new InputEvent(Input.mousePosition, _InputType.Mouse1, _InputState.Up);
			}
			else if(Input.GetMouseButtonDown(1)){
				inputEvent=new InputEvent(Input.mousePosition, _InputType.Mouse2, _InputState.Down);
			}
			else if(Input.GetMouseButton(1)){
				inputEvent=new InputEvent(Input.mousePosition, _InputType.Mouse2, _InputState.On);
			}
			else if(Input.GetMouseButtonUp(1)){
				inputEvent=new InputEvent(Input.mousePosition, _InputType.Mouse2, _InputState.Up);
			}
		}
		
		if(inputEvent.inputType!=_InputType.None){
			if(inputEvent.inputState==_InputState.Down){
				lastPos=inputEvent.pos;
				draggingInitiated=true;
			}
			if(inputEvent.inputState==_InputState.On){
				Vector2 curPos=inputEvent.pos;
				
				if(!draggingInitiated){
					draggingInitiated=true;
				}
				else{
					Vector2 delta=curPos-lastPos;
					if(Mathf.Abs((delta).magnitude)>0){
						dragging=true;
						
						int inputType=0;
						if(inputEvent.inputType==_InputType.Mouse1) inputType=1;
						else if(inputEvent.inputType==_InputType.Mouse2) inputType=2;
						
						DragInfo dragInfo=new DragInfo(inputType, curPos, delta);
						Gesture.Dragging(dragInfo);
					}
					else{
						draggingInitiated=false;
					}
				}
				
				lastPos=inputEvent.pos;
			}
			if(inputEvent.inputState==_InputState.Up){
				if(dragging){
					dragging=false;
					Gesture.DraggingEnd(inputEvent.pos);
				}
				if(draggingInitiated) draggingInitiated=false;
			}
		}
		else{
			if(dragging){
				dragging=false;
				Gesture.DraggingEnd(inputEvent.pos);
			}
			if(draggingInitiated) draggingInitiated=false;
		}
		
		//if(te==null) Debug.Log("null");
		//else Debug.Log("not null. "+te.pos);
		
		//~ #if UNITY_IPHONE || UNITY_ANDROID
			//~ if(Input.touchCount>0){
				//~ foreach(Touch touch in Input.touches){
					//~ if(touch.phase==TouchPhase.Began){
						//~ Gesture.Down(touch.position);
					//~ }
					//~ else if(touch.phase==TouchPhase.Ended){
						//~ Gesture.Up(touch.position);
					//~ }
					//~ else{
						//~ Gesture.On(touch.position);
					//~ }
				//~ }
			//~ }
			
			//~ if(Input.touchCount==1){
				//~ Touch touch=Input.touches[0];
				//~ if(touch.phase == TouchPhase.Moved){
					//~ if(!dragging) dragging=true;
					//~ DragInfo dragInfo=new DragInfo(0, touch.position, touch.deltaPosition);
					//~ Gesture.Dragging(dragInfo);
				//~ }
				//~ //lastPos=touch.position;
			//~ }
			//~ else{
				//~ if(dragging){
					//~ dragging=false;
					//~ Gesture.DraggingEnd(lastPos);
				//~ }
			//~ }
		//~ #endif
			
			
		//~ #if (!UNITY_IPHONE && !UNITY_ANDROID) || UNITY_EDITOR
			//~ if(Input.GetMouseButtonDown(0)){
				//~ Gesture.Down(Input.mousePosition);
				//~ lastPos=Input.mousePosition;
			//~ }
			//~ if(Input.GetMouseButton(0)){
				//~ Gesture.On(Input.mousePosition);
				
				//~ Vector2 curPos=Input.mousePosition;
				//~ Vector2 delta=curPos-lastPos;
				
				//~ if(Mathf.Abs((delta).magnitude)>0){
					//~ dragging=true;
					//~ DragInfo dragInfo=new DragInfo(1, curPos, delta);
					//~ Gesture.Dragging(dragInfo);
				//~ }
				
				//~ lastPos=Input.mousePosition;
			//~ }
			//~ if(Input.GetMouseButtonUp(0)){
				//~ Gesture.Up(Input.mousePosition);
				
				//~ if(dragging){
					//~ dragging=false;
					//~ Gesture.DraggingEnd(Input.mousePosition);
				//~ }
			//~ }
			
			
			//~ if(Input.GetMouseButtonDown(1)){
				//~ Gesture.DownAlt(Input.mousePosition);
				//~ lastPos=Input.mousePosition;
			//~ }
			//~ else if(Input.GetMouseButton(1)){
				//~ Gesture.OnAlt(Input.mousePosition);
				//~ Vector2 curPos=Input.mousePosition;
				//~ Vector2 delta=curPos-lastPos;
				
				//~ if(Mathf.Abs((delta).magnitude)>0){
					//~ draggingAlt=true;
					//~ DragInfo dragInfo=new DragInfo(2, curPos, delta);
					//~ Gesture.Dragging(dragInfo);
				//~ }
				
				//~ lastPos=Input.mousePosition;
			//~ }
			//~ else if(Input.GetMouseButtonUp(1)){
				//~ Gesture.Up(Input.mousePosition);
				
				//~ if(draggingAlt){
					//~ draggingAlt=false;
					//~ Gesture.DraggingEnd(Input.mousePosition);
				//~ }
			//~ }
		//~ #endif
			
		
	}
	
	
}


