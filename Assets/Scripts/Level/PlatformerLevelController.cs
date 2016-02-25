using UnityEngine;
using System.Collections;
using System;

public class PlatformerLevelController : SceneControllerBase {

	public RuleEvaluator ruleEvaluator;
	public string currentLevelName;
	public string primaryExitLevel;
	public string secondExitLevel;
	public string thirdExitLevel;
	public string fourthExitLevel;
	public string mapLevel = "SphericWorld";
	public ExitSelection defaultExit = ExitSelection.first;
	public FadeOutScript fader;
	public AudioSource mainAudioSource;
	public AudioFader audioFader;
	public Texture clearedCoin;
	
	private bool _started;
	private bool _reloading;
	private bool _exiting;
	private bool _costumexiting = false;
	private string _costumexitname = null;
	
	void Awake() {
		if(string.IsNullOrEmpty(currentLevelName)) {
			throw new NullReferenceException("current level name is empty in PlatformLevelController");
		}
		
		if(fader == null) {
			throw new NullReferenceException("Fader not found");	
		}

		if(audioFader == null) {
			throw new NullReferenceException("Audio Fader not found");	
		}

	}
	
	void Start() {
		//Clean EventContext
		EventContext.ClearEvents();
		
		
		//Load character stats
		CharacterStats.GetLastProfile();
		CharacterStats.SetCurrentLevel(currentLevelName);
		CharacterStats.LoadStats();
		CharacterStats.MaxLivesCount = 2;
		CharacterStats.MaxLife = 100;
		
		CharacterStats.ResetLife();
		CharacterStats.ResetLifeCount();
		CharacterStats.ResetLevelCredits();
		
		_started = false;
		_exiting = false;
		_reloading = false;
		if(mainAudioSource != null) {
			mainAudioSource.Play();
		}
		
		if(Enum.IsDefined( typeof(Levels), currentLevelName))
			{
			//Changes the color of the Credit Mesh Texture if already taken
			if(CharacterStats.ClearedLevel()) {
				GameObject.Find("credit").GetComponent<Renderer>().material.mainTexture = clearedCoin;
			}
		}
	}
	
	void OnEnable() {
		if(ruleEvaluator != null) {
			ruleEvaluator.OnGameEvent += HandleRuleEvaluatorOnGameEvent;	
		}
		fader.OnFadeFinished += HandleFadeFinshed;
		audioFader.OnFadeFinished += HandleAudioFadeFinished;
		GlobalEventManager.OnGlobalEvent += HandleGlobalEvent;
	}

	void OnDisable() {
		if(ruleEvaluator != null) {
			ruleEvaluator.OnGameEvent -= HandleRuleEvaluatorOnGameEvent;
		}
		fader.OnFadeFinished -= HandleFadeFinshed;
		audioFader.OnFadeFinished -= HandleAudioFadeFinished;
		GlobalEventManager.OnGlobalEvent -= HandleGlobalEvent;
	}
	
	void Update() {
		if(!_started) {
			fader.fadeDirection = FadeOutScript.FadeDirection.FadeIn;
			fader.Fade();
			
			_started = true;
		}
		
		if(Input.GetKeyDown(KeyCode.P)) {
			LoadNextScene(defaultExit);	
		}
		
	}
	
	void LateUpdate() {
		if(EventContext.Events.ContainsKey ("NoLivesRemaining")) {
			EventContext.Events.Remove ("NoLivesRemaining");
			ReloadLevel();
		}
	}
	
	
	public void ReloadLevel() {
		_reloading = true;
		fader.fadeDirection = FadeOutScript.FadeDirection.FadeOut;
		fader.Fade();		
	}
	
	
	public void ExitLevel() {
		_exiting = true;
		fader.fadeDirection = FadeOutScript.FadeDirection.FadeOut;
		fader.Fade();
	}
	
	public void PreferedExitLevel(string levelname) {
		_exiting = true;
		_costumexiting = true;
		_costumexitname = levelname;
		fader.fadeDirection = FadeOutScript.FadeDirection.FadeOut;
		fader.Fade();
	}
	
	private void HandleFadeFinshed() {
		if(_exiting || _reloading) {
			audioFader.fadeDirection = AudioFader.FadeDirection.FadeOut;
			audioFader.Fade();
		}
	}

	private void HandleAudioFadeFinished() {
		if(_exiting) {
			if(_costumexiting) {
				Application.LoadLevel(_costumexitname);
			}
			else {
				Application.LoadLevel(mapLevel);
			}
		}
		if(_reloading) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	
	private void HandleRuleEvaluatorOnGameEvent(GameEvents eventType)
	{
		switch(eventType) {
		case GameEvents.Win:
			//save stats
			CharacterStats.ApplyCreditCount();
			CharacterStats.SaveStats();
			
			Application.LoadLevel("HeroWinLevel");
			break;
		case GameEvents.Loose:
			//Reload level
		
			if(Enum.IsDefined( typeof(Levels), currentLevelName))
			{
				Application.LoadLevel("HeroLoseLevel");
			}
			else
			{
				Application.LoadLevel(Application.loadedLevel);
			}
			break;
		}
	}
	
	private void HandleGlobalEvent(string eventName, object[] eventParams)
	{
		if(eventName == "Reload") {
			ReloadLevel();
		}
		if(eventName == "Unload") {
			ExitLevel();
		}
	}


	
	public override void LoadNextScene (ExitSelection exit)
	{
		switch(exit) {
			case ExitSelection.default_exit:
			ExitLevel();
			break;
			
			case ExitSelection.first:
			if(primaryExitLevel != null) {
				PreferedExitLevel(primaryExitLevel);
			}
			else {
				ExitLevel();
			}
			break;
			case ExitSelection.second:
			if(primaryExitLevel != null) {
				PreferedExitLevel(secondExitLevel);
			}
			else {
				ExitLevel();
			}
			break;
			case ExitSelection.third:
			if(primaryExitLevel != null) {
				PreferedExitLevel(thirdExitLevel);
			}
			else {
				ExitLevel();
			}
			break;
			case ExitSelection.fourth:
			if(primaryExitLevel != null) {
				PreferedExitLevel(fourthExitLevel);
			}
			else {
				ExitLevel();
			}
			break;
			
			
			
			
		}
		
		
		
	}
	
}
