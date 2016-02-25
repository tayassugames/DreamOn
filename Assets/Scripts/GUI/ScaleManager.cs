using UnityEngine;
using System.Collections;

public class ScaleManager {

	private static float _xScaleFactor = 0;
	public static float XScaleFactor {
		get {
			if(_xScaleFactor == 0) {
				_xScaleFactor = (float)Screen.width / 640.0f;
			}
			return _xScaleFactor;
		}
	}
	
	private static float _yScaleFactor = 0;
	public static float YScaleFactor {
		get {
			if(_yScaleFactor == 0) {
				_yScaleFactor = (float)Screen.height / 480.0f;
			}
			return _yScaleFactor;
		}
	}
	
	public static Rect GetScaledRect(Rect sourceRect) {
		Rect result = new Rect(sourceRect.x * XScaleFactor,
			sourceRect.y * YScaleFactor,
			sourceRect.width * XScaleFactor,
			sourceRect.height * YScaleFactor);
		
		return result;		
	}
	
	public static Rect GetCenteredScaledRect(Rect sourceRect) {
		Rect result = new Rect(Screen.width/2 - (sourceRect.width/2 * XScaleFactor),
			sourceRect.y * YScaleFactor,
			sourceRect.width * XScaleFactor,
			sourceRect.height * YScaleFactor);
		
		return result;
	}
	
	public static Rect GetPositionedScaledRect(Rect sourceRect) {
		Rect result = new Rect(sourceRect.x,
			sourceRect.y,
			sourceRect.width * XScaleFactor,
			sourceRect.height * YScaleFactor);
		
		return result;

	}
	
}
