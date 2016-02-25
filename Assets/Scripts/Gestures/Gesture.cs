using UnityEngine;
using System.Collections;

[RequireComponent (typeof (General))]
[RequireComponent (typeof (TapDetector))]
[RequireComponent (typeof (SwipeDetector))]
[RequireComponent (typeof (DualFingerDetector))]


public class Gesture : MonoBehaviour {

	public static Gesture gesture;
	
	
	//standard
	public delegate void ShortTapHandler(Vector2 pos); 
	public static event ShortTapHandler onShortTapE;
	
	public delegate void LongTapHandler(Vector2 pos); 
	public static event LongTapHandler onLongTapE;
	
	public delegate void DoubleTapHandler(Vector2 pos); 
	public static event DoubleTapHandler onDoubleTapE;
	
	public delegate void ChargingHandler(ChargedInfo cInfo); 
	public static event ChargingHandler onChargingE;
	
	public delegate void ChargeEndHandler(ChargedInfo cInfo); 
	public static event ChargeEndHandler onChargeEndE;
	
	
	//Dual Finger Standard
	public delegate void DFShortTapHandler(Vector2 pos); 
	public static event DFShortTapHandler onDFShortTapE;
	
	public delegate void DFLongTapHandler(Vector2 pos); 
	public static event DFLongTapHandler onDFLongTapE;
	
	public delegate void DFDoubleTapHandler(Vector2 pos); 
	public static event DFDoubleTapHandler onDFDoubleTapE;
	
	public delegate void DFChargingHandler(ChargedInfo cInfo); 
	public static event DFChargingHandler onDFChargingE;
	
	public delegate void DFChargeEndHandler(ChargedInfo cInfo); 
	public static event DFChargeEndHandler onDFChargeEndE;
	
	
	//dragging
	public delegate void DraggingHandler(DragInfo dragInfo);
	public static event DraggingHandler onDraggingE;
	
	public delegate void DualFDragHandler(DragInfo dragInfo); 
	public static event DualFDragHandler onDualFDraggingE;
	
	public delegate void DraggingEndHandler(Vector2 pos); 
	public static event DraggingEndHandler onDraggingEndE;
	
	public delegate void DualFDraggingEndHandler(Vector2 pos); 
	public static event DualFDraggingEndHandler onDualFDraggingEndE;
	
	
	//special
	public delegate void SwipeHandler(SwipeInfo sw); 
	public static event SwipeHandler onSwipeE;
	
	public delegate void PinchHandler(float val); 
	public static event PinchHandler onPinchE;
	
	public delegate void RotateHandler(float val); 
	public static event RotateHandler onRotateE;
	
	
	
	//press and unpress
	//~ public delegate void DownHandler(Vector2 pos); 
	//~ public static event DownHandler onDownE;
	
	//~ public delegate void DownAltHandler(Vector2 pos); 
	//~ public static event DownAltHandler onDownAltE;
	
	//~ public delegate void UpHandler(Vector2 pos); 
	//~ public static event UpHandler onUpE;
	
	//~ public delegate void UpAltHandler(Vector2 pos); 
	//~ public static event UpAltHandler onUpAltE;
	
	//~ public delegate void OnHandler(Vector2 pos); 
	//~ public static event OnHandler onOnE;
	
	//~ public delegate void OnAltHandler(Vector2 pos); 
	//~ public static event OnAltHandler onOnAltE;
	
	public delegate void TouchDownHandler(Vector2 pos); 
	public static event TouchDownHandler onTouchDownE;
	
	public delegate void TouchUpHandler(Vector2 pos); 
	public static event TouchUpHandler onTouchUpE;
	
	public delegate void TouchHandler(Vector2 pos); 
	public static event TouchHandler onTouchE;
	
	public delegate void Mouse1DownHandler(Vector2 pos); 
	public static event Mouse1DownHandler onMouse1DownE;
	
	public delegate void Mouse1UpHandler(Vector2 pos); 
	public static event Mouse1UpHandler onMouse1UpE;
	
	public delegate void Mouse1Handler(Vector2 pos); 
	public static event Mouse1Handler onMouse1E;
	
	public delegate void Mouse2DownHandler(Vector2 pos); 
	public static event Mouse2DownHandler onMouse2DownE;
	
	public delegate void Mouse2UpHandler(Vector2 pos); 
	public static event Mouse2UpHandler onMouse2UpE;
	
	public delegate void Mouse2Handler(Vector2 pos); 
	public static event Mouse2Handler onMouse2E;
	
	
	
	void Awake(){
		gesture=this;
	}
	
	
	//standard
	public static void ShortTap(Vector2 pos){
		//Debug.Log("short tap "+pos);
		if(onShortTapE!=null) onShortTapE(pos);
	}
	
	public static void LongTap(Vector2 pos){
		//Debug.Log("long tap "+pos);
		if(onLongTapE!=null) onLongTapE(pos);
	}
	
	public static void DoubleTap(Vector2 pos){
		//Debug.Log("Double tap "+pos);
		if(onDoubleTapE!=null) onDoubleTapE(pos);
	}
	
	public static void Charging(ChargedInfo cInfo){
		//Debug.Log("charging "+chargePercent);
		if(onChargingE!=null) onChargingE(cInfo);
	}
	
	public static void ChargeEnd(ChargedInfo cInfo){
		//Debug.Log("charge end "+cInfo.percent);
		if(onChargeEndE!=null) onChargeEndE(cInfo);
	}
	
	
	//Dual Finger standard
	public static void DFShortTap(Vector2 pos){
		if(onDFShortTapE!=null) onDFShortTapE(pos);
	}
	
	public static void DFLongTap(Vector2 pos){
		if(onDFLongTapE!=null) onDFLongTapE(pos);
	}
	
	public static void DFDoubleTap(Vector2 pos){
		if(onDFDoubleTapE!=null) onDFDoubleTapE(pos);
	}
	
	public static void DFCharging(ChargedInfo cInfo){
		if(onDFChargingE!=null) onDFChargingE(cInfo);
	}
	
	public static void DFChargeEnd(ChargedInfo cInfo){
		if(onDFChargeEndE!=null) onDFChargeEndE(cInfo);
	}
	
	
	//Dual Finger dragging
	public static void Dragging(DragInfo dragInfo){
		//Debug.Log("dragging "+dir);
		if(onDraggingE!=null) onDraggingE(dragInfo);
	}
	
	public static void DualFingerDragging(DragInfo dragInfo){
		//Debug.Log("DualFingerDrag "+dir);
		if(onDualFDraggingE!=null) onDualFDraggingE(dragInfo);
	}
	
	public static void DraggingEnd(Vector2 pos){
		if(onDraggingEndE!=null) onDraggingEndE(pos);
	}
	
	public static void DualFingerDraggingEnd(Vector2 pos){
		//Debug.Log("DualFingerDrag "+dir);
		if(onDualFDraggingEndE!=null) onDualFDraggingEndE(pos);
	}
	

	//special
	public static void Swipe(SwipeInfo sw){
		//Debug.Log("swipe start at "+"   "+pos);
		if(onSwipeE!=null) onSwipeE(sw);
	}

	public static void Pinch(float val){
		//Debug.Log("Pinch "+val);
		if(onPinchE!=null) onPinchE(val);
	}
	
	public static void Rotate(float val){
		//if(val>0) Debug.Log("RotateCC "+val);
		//else Debug.Log("RotateC "+val);
		if(onRotateE!=null) onRotateE(val);
	}
	
	
	//general event
	//~ public static void Down(Vector2 pos){
		//~ if(onDownE!=null) onDownE(pos);
	//~ }
	
	//~ public static void DownAlt(Vector2 pos){
		//~ if(onDownAltE!=null) onDownAltE(pos);
	//~ }
	
	//~ public static void Up(Vector2 pos){
		//~ if(onUpE!=null) onUpE(pos);
	//~ }
	
	//~ public static void UpAlt(Vector2 pos){
		//~ if(onUpAltE!=null) onUpAltE(pos);
	//~ }
	
	//~ public static void On(Vector2 pos){
		//~ if(onOnE!=null) onOnE(pos);
	//~ }
	
	//~ public static void OnAlt(Vector2 pos){
		//~ if(onOnAltE!=null) onOnAltE(pos);
	//~ }
	
	
	public static void OnTouchDown(Vector2 pos){
		if(onTouchDownE!=null) onTouchDownE(pos);
	}
	public static void OnTouchUp(Vector2 pos){
		if(onTouchUpE!=null) onTouchUpE(pos);
	}
	public static void OnTouch(Vector2 pos){
		if(onTouchE!=null) onTouchE(pos);
	}
	
	public static void OnMouse1Down(Vector2 pos){
		if(onMouse1DownE!=null) onMouse1DownE(pos);
	}
	public static void OnMouse1Up(Vector2 pos){
		if(onMouse1UpE!=null) onMouse1UpE(pos);
	}
	public static void OnMouse1(Vector2 pos){
		if(onMouse1E!=null) onMouse1E(pos);
	}
	
	public static void OnMouse2Down(Vector2 pos){
		if(onMouse2DownE!=null) onMouse2DownE(pos);
	}
	public static void OnMouse2Up(Vector2 pos){
		if(onMouse2UpE!=null) onMouse2UpE(pos);
	}
	public static void OnMouse2(Vector2 pos){
		if(onMouse2E!=null) onMouse2E(pos);
	}
	
	
	
	//utility for converting vector to angle
	public static float VectorToAngle(Vector2 dir){
		
		if(dir.x==0){
			if(dir.y>0) return 90;
			else if(dir.y<0) return 270;
			else return 0;
		}
		else if(dir.y==0){
			if(dir.x>0) return 0;
			else if(dir.x<0) return 180;
			else return 0;
		}
		
		float h=Mathf.Sqrt(dir.x*dir.x+dir.y*dir.y);
		float angle=Mathf.Asin(dir.y/h)*Mathf.Rad2Deg;
		
		if(dir.y>0){
			if(dir.x<0)  angle=180-angle;
		}
		else{
			if(dir.x>0)  angle=360+angle;
			if(dir.x<0)  angle=180-angle;
		}
		
		//Debug.Log(angle);
		return angle;
	}
	
}




public enum _InputType{None, Mouse1, Mouse2, Touch}
public enum _InputState{On, Down, Up}


public class InputEvent{
	public _InputType inputType=_InputType.None;
	public _InputState inputState;
	public Vector2 pos=new Vector3(-999, -999);
	
	public InputEvent(){
		
	}
	
	public InputEvent(Vector3 p, _InputType type, _InputState state){
		pos=p;
		inputType=type;
		inputState=state;
	}
}


public class ChargedInfo{
	public float percent=0;
	public Vector2 pos;
	public Vector2 pos1;
	public Vector2 pos2;
	
	public ChargedInfo(Vector2 p, float val){
		pos=p;
		percent=val;
	}
	
	public ChargedInfo(Vector2 p, float val, Vector2 p1, Vector2 p2){
		pos=p;
		percent=val;
		pos1=p1;
		pos2=p2;
	}
}

public class DragInfo{
	public int type=-1;
	public Vector2 pos;
	public Vector2 delta;
	
	public DragInfo(int t, Vector2 p, Vector2 dir){
		type=t;
		pos=p;
		delta=dir;
	}
}

public class SwipeInfo{
	public Vector2 startPoint;
	public Vector2 endPoint;
	
	public Vector2 direction;
	public float angle;
	
	public float duration;
	public float speed;
	
	public SwipeInfo(Vector2 p1, Vector2 p2, Vector2 dir, float startT){
		startPoint=p1;
		endPoint=p2;
		direction=dir;
		angle=Gesture.VectorToAngle(dir);
		duration=Time.time-startT;
		speed=dir.magnitude/duration;
	}
}




