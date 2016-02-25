using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ProfileSelector : MonoBehaviour {
	
	public string nextLevel;
	public string worldMapLevelName;
	public Texture backgroundTexture;
	public Texture2D button1Texture;
	public Texture2D button2Texture;
	public Texture2D button3Texture;
	public Texture blankSelection;
	public Texture eraseOn;
	public Texture eraseOff;
	public Texture confirmationClean;
	public Texture confirmationYes;
	public Texture confirmationNo;
	
	public AudioSource audioSource;
	
	public AudioClip confirmYes;
	public AudioClip confirmNo;
	public AudioClip eraseButton;
	public AudioClip buttonStart;
	
	public GUISkin skin;
	
	public List<Texture> numbers;
	public float horizontalCreditCountUnitOffset = 200;
	public float verticalCreditCountUnitOffset = 5;
	
	public float eraseHPosition = 300;
	public float eraseVPosition = 300;
	
	public Vector2 confirmationPosition = new Vector2(300, 300);
	public Vector2 confirmationButtonDimensions = new Vector2(30, 30);
	public Vector2 yesButtonPosition = new Vector2(300, 300);
	public Vector2 noButtonPosition = new Vector2(300, 300);
	
	private bool _eraseToggle = false;
	private bool _confirmDelete = false;
	private bool _confirmAnswer = false;
	private bool _doBlink = false;
	private bool _showConfirmationButtons = false;
	private bool _showSelection = false;
	private int _profileToDelete = 0;
	//private Vector2 _selectionPosition = new Vector2(300, 300);
	private Rect selectionRect = new Rect(0,0,0,0);
	//private string _labelText;
	
	
	private List<int> _profileCreditCount;
	
	void Awake() {
		if(string.IsNullOrEmpty(nextLevel)) {
			throw new NullReferenceException("Next level not defined");
		}
		
		if(backgroundTexture == null) {
			throw new NullReferenceException("Texture not defined");
		}
	}
	
	void Start() {
		LoadProfileInfo();
	}
	
	private void LoadProfileInfo() {
		_profileCreditCount = new List<int>();
		
		CharacterStats.SelectProfile (0);
		CharacterStats.SetCurrentAsLastProfile();
		CharacterStats.LoadStats();
		
		if(CharacterStats.CreditCount > 0) {
			_profileCreditCount.Add (CharacterStats.CreditCount);
		} else {
			_profileCreditCount.Add (0);
		}
		
		CharacterStats.SelectProfile (1);
		CharacterStats.SetCurrentAsLastProfile();
		CharacterStats.LoadStats();
		
		if(CharacterStats.CreditCount > 0) {
			_profileCreditCount.Add (CharacterStats.CreditCount);
		} else {
			_profileCreditCount.Add (0);
		}
		
		CharacterStats.SelectProfile (2);
		CharacterStats.SetCurrentAsLastProfile();
		CharacterStats.LoadStats();
		
		if(CharacterStats.CreditCount > 0) {
			_profileCreditCount.Add (CharacterStats.CreditCount);
		} else {
			_profileCreditCount.Add (0);
		}

	}
	
	/// <summary>
	/// REFACTOR
	/// </summary>
	void OnGUI() {
		
		GUI.skin = skin;
		GUI.DrawTexture (new Rect(0,0, Screen.width, Screen.height), backgroundTexture, ScaleMode.ScaleToFit);
				
		//GUI.Label(new Rect(0,0,300,100), _labelText);
		
		
		//BUTTON 1
		Rect button1Rect = ScaleManager.GetScaledRect(new Rect(100, 164, button1Texture.width, button1Texture.height));
		if(!_confirmDelete) {
			if(GUI.Button (button1Rect, button1Texture)) {
				if(!_eraseToggle) {
					selectionRect = button1Rect;
					StartCoroutine(SetProfile(0));
				} else {
					ConfirmDeleteProfile(0);
				}
			}
		}
		GUI.DrawTexture (button1Rect, button1Texture, ScaleMode.ScaleToFit);
		//Numbers
		if(_profileCreditCount != null) {
			int units = (_profileCreditCount.Count > 0 ? _profileCreditCount[0] / 10 : 0);
			
			GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(100 + horizontalCreditCountUnitOffset - numbers[units].width + 4, 
				164 + verticalCreditCountUnitOffset, 
				numbers[units].width, 
				numbers[units].height)),
				numbers[units], ScaleMode.ScaleToFit);
		}
		if(_profileCreditCount != null) {
			int units = (_profileCreditCount.Count > 0 ? _profileCreditCount[0] % 10 : 0);
			
			GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(100 + horizontalCreditCountUnitOffset, 
				164 + verticalCreditCountUnitOffset, 
				numbers[units].width, 
				numbers[units].height)),
				numbers[units], ScaleMode.ScaleToFit);
		}
		
		//BUTTON 2
		Rect button2Rect = ScaleManager.GetScaledRect(new Rect(100, 244, button2Texture.width, button2Texture.height));
		if(!_confirmDelete) {
			if(GUI.Button (button2Rect, button2Texture)) {
				if(!_eraseToggle) {
					selectionRect = button2Rect;
					StartCoroutine(SetProfile(1));
				} else {
					ConfirmDeleteProfile(1);
				}
			}
		}
		GUI.DrawTexture (button2Rect, button2Texture, ScaleMode.ScaleToFit);
		//Numbers
		if(_profileCreditCount != null) {
			int units = (_profileCreditCount.Count > 1 ? _profileCreditCount[1] / 10 : 0);
			
			GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(100 + horizontalCreditCountUnitOffset - numbers[units].width + 4, 
				244 + verticalCreditCountUnitOffset, 
				numbers[units].width, 
				numbers[units].height)),
				numbers[units], ScaleMode.ScaleToFit);
		}
		if(_profileCreditCount != null) {
			int units = (_profileCreditCount.Count > 1 ? _profileCreditCount[1] % 10 : 0);
			
			GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(100 + horizontalCreditCountUnitOffset, 
				244 + verticalCreditCountUnitOffset, 
				numbers[units].width, 
				numbers[units].height)),
				numbers[units], ScaleMode.ScaleToFit);
		}

		
		//BUTTON 3
		Rect button3Rect = ScaleManager.GetScaledRect(new Rect(100, 324, button3Texture.width, button3Texture.height));
		if(!_confirmDelete) {
			if(GUI.Button (button3Rect, button3Texture)) {
				if(!_eraseToggle) {
					selectionRect = button3Rect;
					StartCoroutine(SetProfile(2));
				} else {
					ConfirmDeleteProfile(2);
				}
			}
		}
		GUI.DrawTexture (button3Rect, button3Texture, ScaleMode.ScaleToFit);
		//Numbers
		if(_profileCreditCount != null) {
			int units = (_profileCreditCount.Count > 2 ? _profileCreditCount[2] / 10 : 0);
			
			GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(100 + horizontalCreditCountUnitOffset - numbers[units].width + 4, 
				324 + verticalCreditCountUnitOffset, 
				numbers[units].width, 
				numbers[units].height)),
				numbers[units], ScaleMode.ScaleToFit);
		}
		if(_profileCreditCount != null) {
			int units = (_profileCreditCount.Count > 2 ? _profileCreditCount[2] % 10 : 0);

			GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(100 + horizontalCreditCountUnitOffset, 
				324 + verticalCreditCountUnitOffset, 
				numbers[units].width, 
				numbers[units].height)),
				numbers[units], ScaleMode.ScaleToFit);
		}
		
		//Blink over selection
		if(_showSelection) {
			GUI.DrawTexture (selectionRect, blankSelection, ScaleMode.ScaleToFit);
		}

		
		//Erase button
		if(!_confirmDelete) {
			if(GUI.Button (ScaleManager.GetScaledRect(new Rect(eraseHPosition, eraseVPosition, eraseOff.width, eraseOff.height - 2)),
				eraseOff)) {
				audioSource.PlayOneShot (eraseButton);
				_eraseToggle = !_eraseToggle;
			}
		}
		if(!_eraseToggle) {
			GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(eraseHPosition, eraseVPosition, eraseOff.width, eraseOff.height)),
				eraseOff, ScaleMode.ScaleToFit);
		} else {
			GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(eraseHPosition, eraseVPosition, eraseOn.width, eraseOn.height)),
				eraseOn, ScaleMode.ScaleToFit);			
		}
		
		
		//Confirmation dialog
		if(_confirmDelete) {
			if(!_doBlink) {
				if(_showConfirmationButtons) {
					if(GUI.Button (ScaleManager.GetScaledRect(new Rect(yesButtonPosition.x, 
						yesButtonPosition.y, confirmationButtonDimensions.x, confirmationButtonDimensions.y)),
						"YES")) {
						_confirmAnswer = true;
						audioSource.PlayOneShot (confirmYes);
						StartCoroutine(DoBlink());
					}
					if(GUI.Button (ScaleManager.GetScaledRect(new Rect(noButtonPosition.x, 
						noButtonPosition.y, confirmationButtonDimensions.x, confirmationButtonDimensions.y)),
						"NO")) {
						_confirmAnswer = false;
						audioSource.PlayOneShot (confirmNo);
						StartCoroutine(DoBlink());
					}
				}

				GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(confirmationPosition.x, 
					confirmationPosition.y, confirmationClean.width, confirmationClean.height)),
					confirmationClean, ScaleMode.ScaleToFit);	
			} else {
				if(_confirmAnswer) {
					GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(confirmationPosition.x, 
					confirmationPosition.y, confirmationYes.width, confirmationYes.height)),
						confirmationYes, ScaleMode.ScaleToFit);
				} else {
					GUI.DrawTexture (ScaleManager.GetScaledRect(new Rect(confirmationPosition.x, 
					confirmationPosition.y, confirmationNo.width, confirmationNo.height)),
						confirmationNo, ScaleMode.ScaleToFit);
				}
			}
		}
		
	}
	
	private IEnumerator SetProfile(int profileIndex) {
	
		StartCoroutine(DoSelectionBlink());
		
		audioSource.PlayOneShot(buttonStart);
		yield return new WaitForSeconds(1.5f);
		
		//_labelText = "loading";
		try {
			CharacterStats.SelectProfile(profileIndex);
			CharacterStats.SetCurrentAsLastProfile();
			CharacterStats.LoadStats();
		} catch(Exception ex) {
			//_labelText = ex.Message + "\n" + ex.StackTrace;	
			throw ex;
		}
		
		//FadeOut
		if(CharacterStats.CreditCount > 0 && Application.CanStreamedLevelBeLoaded(worldMapLevelName)) {
			//_labelText = "worldMap";
			Application.LoadLevel(worldMapLevelName);
		} else {
			//_labelText = "nextLevel";
			Application.LoadLevel(nextLevel);
		}
	}
	
	private void ConfirmDeleteProfile(int profileIndex) {
		audioSource.PlayOneShot(eraseButton);
		_confirmDelete = true;
		_showConfirmationButtons = true;
		_doBlink = false;
		_profileToDelete = profileIndex;
	}
	
	private IEnumerator DoBlink() {
		CharacterStats.SelectProfile(_profileToDelete);
		CharacterStats.SetCurrentAsLastProfile();
		CharacterStats.LoadStats();
		
		_showConfirmationButtons = false;
		
		float maxTime = 0.5f;
		float interval = 0.125f;
		float timer = 0;
		
		while(timer < maxTime) {
			_doBlink = !_doBlink;
			yield return(new WaitForSeconds(interval));
			timer += interval;
		}
		_doBlink = false;
		
		//Delete profile
		if(_confirmAnswer) {
			CharacterStats.ClearStats();
			CharacterStats.SaveStats();
			
			LoadProfileInfo();
		}
		_confirmDelete = false;
		_eraseToggle = false;
	}
	
	private IEnumerator DoSelectionBlink() {
		
		float maxTime = 0.5f;
		float interval = 0.05f;
		float timer = 0;
		
		while(timer < maxTime) {
			_showSelection = !_showSelection;
			yield return new WaitForSeconds(interval);
			timer += interval;
		}
		_showSelection = false;		
	}	
}
