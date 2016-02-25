using UnityEngine;
using System.Collections;

public class LanguageSelector : MonoBehaviour {
	
	public string nextLevel;
	
	public float baseButtonWidth = 200;
	public float baseButtonHeight = 50;
	public float baseButtonLeft = 220;
	
	public float englishButtonWidth = 200;
	public float englishButtonHeight = 50;
	public float englishButtonLeft = 220;	
	public float englishButtonTop = 100;
	
	public float spanishButtonWidth = 200;
	public float spanishButtonHeight = 50;
	public float spanishButtonLeft = 220;
	public float spanishButtonTop = 200;
	
	public float portugueseButtonWidth = 200;
	public float portugueseButtonHeight = 50;
	public float portugueseButtonLeft = 220;
	public float portugueseButtonTop = 300;

	public AudioSource audioSource;
	
	public AudioClip buttonClick;
	
	public Texture backgroundImage;
	
	private bool _loadNext = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//FadeOut
		if(_loadNext && Application.CanStreamedLevelBeLoaded(nextLevel)) {
			_loadNext = false;
			Application.LoadLevel(nextLevel);
		}
	}
	
	void OnGUI() {
		if(GUI.Button(ScaleManager.GetScaledRect (new Rect(englishButtonLeft, englishButtonTop, englishButtonWidth, englishButtonHeight)), 
			"English")) {
			DialogScript.language = DialogScript.Language.ENGLISH;
			StartCoroutine(LoadNextScene());
		}

		if(GUI.Button(ScaleManager.GetScaledRect (new Rect(spanishButtonLeft, spanishButtonTop, spanishButtonWidth, spanishButtonHeight)), 
			"Español")) {
			DialogScript.language = DialogScript.Language.SPANISH;
			StartCoroutine(LoadNextScene());
		}

		if(GUI.Button(ScaleManager.GetScaledRect (new Rect(portugueseButtonLeft, portugueseButtonTop, portugueseButtonWidth, portugueseButtonHeight)), 
			"Portugueis")) {
			DialogScript.language = DialogScript.Language.PORTUGUESE;
			StartCoroutine(LoadNextScene());
		}

		GUI.DrawTexture(ScaleManager.GetScaledRect(new Rect(0,0,backgroundImage.width,backgroundImage.height)), backgroundImage);
		
	}
	
	private IEnumerator LoadNextScene() {
		audioSource.PlayOneShot(buttonClick);
		yield return new WaitForSeconds(2);
		_loadNext = true;
	}
}
