using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraResizer : MonoBehaviour {
	
	private Camera _camera;
	public List<Rect> cameraRect;
	public List<float> widthRects;
	public int index;
	public float width;
	public float ratio;
	public float baseWidth = 480f;
	public float baeHeight = 320f;
	
	// Use this for initialization
	void Start () {
		_camera = gameObject.GetComponent<Camera>();
		width = Screen.width;
		ratio = (float)Screen.height / (float)Screen.width;
		
		//cameraRect[index] = _camera.pixelRect;
	}
	
	// Update is called once per frame
	void Update () {
		
		_camera.pixelRect = cameraRect[index];
	}
}
