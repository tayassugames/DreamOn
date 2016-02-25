using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FeedbackLabel : MonoBehaviour {
	
	
	private List<string> _textList;
	//private string _text = "text";
	
	private static FeedbackLabel _instance;
	public static FeedbackLabel Instance {
		get
		{
			return _instance;
		}
	}
	
	void Awake() {
		_instance = this;
		_textList = new List<string>();
		_textList.Add("text");
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		float initialTopLine = Screen.height - (35 * _textList.Count);
		
		foreach(string text in _textList) {
			GUI.Label(new Rect(Screen.width - 155, initialTopLine, 150, 30), text);
			initialTopLine += 35;
		}
	}
	
	
	public void SetText(string text) {
		SetText(text, 0);	
	}
	public void SetText(string text, int channel) {
		if(_textList.Count < channel + 1) {
			//Add channel
			_textList.Add(text);
		} else {
			_textList[channel] = text;
		}
	}
}
