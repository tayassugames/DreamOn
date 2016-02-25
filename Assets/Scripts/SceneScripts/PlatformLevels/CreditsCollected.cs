using UnityEngine;
using System.Collections;

public class CreditsCollected : MonoBehaviour {
	
	public PlatformerDialogSpotController initialDialog;
	public PlatformerDialogSpotController notEnoughDialog;
	public PlatformerDialogSpotController enoughCredsDialog;
	public PlatformerLevelController levelController;
	public string endDialogSceneName = "endScene";
	
	public bool useTest = false;
	public int testCredits = 0;
	
	public DialogScript dialogs;
	
	void OnEnable() {
		GlobalEventManager.OnGlobalEvent += HandleGlobalEvent;
	}

	private void HandleGlobalEvent (string eventName, System.Object[] eventParams)
	{
		if(eventName == "DIALOG_FINISHED") {
			if(eventParams.Length == 2) {
				int chapter = (int)eventParams[0];
				int scene = (int)eventParams[1];
				
				if(chapter == 4 && scene == 2) {
					levelController.mapLevel = endDialogSceneName;
					levelController.ExitLevel();
				}
			}
		}
	}
	
	void OnDisable() {
		GlobalEventManager.OnGlobalEvent -= HandleGlobalEvent;
	}
	
	
	// Use this for initialization
	void Start () {
		if(useTest) {
			CharacterStats.CreditCount = testCredits;
		}
		
		if(CharacterStats.CreditCount == 0) {
			initialDialog.dialogEnabled = true;
			notEnoughDialog.dialogEnabled = false;
			enoughCredsDialog.dialogEnabled = false;
		} else if(CharacterStats.CreditCount >= 10) {
			initialDialog.dialogEnabled = false;
			notEnoughDialog.dialogEnabled = false;
			enoughCredsDialog.dialogEnabled = true;
		} else if(CharacterStats.CreditCount > 0) {
			initialDialog.dialogEnabled = false;
			notEnoughDialog.dialogEnabled = true;
			enoughCredsDialog.dialogEnabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
