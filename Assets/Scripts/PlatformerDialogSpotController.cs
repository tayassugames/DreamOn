using UnityEngine;
using System.Collections;

public class PlatformerDialogSpotController : MonoBehaviour {
	
	public bool dialogEnabled = true;
	public bool reenable = false;
	public bool randomize = false;
	public bool restoreMovement = true;
	public platformerControl platformerController;
	public eventHandler touchEventHandler;
	public DialogScript dialog;
	public int chapterNumber;
	public int dialogNumber;
	
	public int dialogCount;
	private int _dialogCounter = 0;
	private int _currentDialog = 0;
	private bool _dialogOpen = false;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(_dialogOpen) {
			platformerController.DoStop();
		}
	}
	
	
	void OnEnable() {
		dialog.OnDialogEnd += DialogFinishedHandler;	
	}
	
	void OnDisable() {
		dialog.OnDialogEnd -= DialogFinishedHandler;
	}
	
	void OnTriggerEnter(Collider collider) {
		if(!dialogEnabled)
			return;
		
		//Triggers only for the Player object
		if(collider.tag == "Player") {
			//mover.canMove = false;
			touchEventHandler.enabled = false;
			_dialogOpen = true;
			dialogEnabled = reenable;
			
			if(reenable) {
				dialog.StartScene(chapterNumber,_dialogCounter);
				_currentDialog = _dialogCounter;
				if(_dialogCounter == dialogCount - 1) {
					_dialogCounter = 0;
				} else {
					_dialogCounter++;
				}
			} else {
				dialog.StartScene(chapterNumber,dialogNumber);
			}
		}
	}
	
	private void DialogFinishedHandler() {
		//Make sure we're notifying the same dialog event
		if(dialog.currentChapter == chapterNumber 
			&& dialog.currentScene == (reenable ? _currentDialog : dialogNumber)) {
			_dialogOpen = false;
			//Notify global event
			GlobalEventManager.NotifyEvent("DIALOG_FINISHED", new object[] {chapterNumber, dialogNumber});
			if(restoreMovement) {
				//mover.canMove = true;
				touchEventHandler.enabled = true;
			}
		}
		
	}
	
}
