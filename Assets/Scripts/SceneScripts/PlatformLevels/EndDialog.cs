using UnityEngine;
using System.Collections;

public class EndDialog : MonoBehaviour {

	public platformerControl platformerController;
	public DialogScript dialog;
	public FadeOutScript fader;
	public bool skip = false;
	public PlatformerLevelController levelController;
	
	private int _actNumber = 0;
	private bool _exiting = false;
	
	
	// Use this for initialization
	void Start () {
		StartCoroutine(StartScene());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable() {
		dialog.OnDialogEnd += HandleDialogFinished;
		fader.OnFadeFinished += FadeFinishedHandler;
	}

	void OnDisable() {
		dialog.OnDialogEnd -= HandleDialogFinished;
		fader.OnFadeFinished -= FadeFinishedHandler;
	}
	
	
	public IEnumerator StartScene() {
		yield return new WaitForSeconds(3f);
		if(!skip) {
			StartCoroutine (PlayDialog(5,0));
			while(_actNumber < 1)
				yield return null;
			
			yield return new WaitForSeconds(2f);
			levelController.ExitLevel();			
		}	
	}
	
	
	private IEnumerator PlayDialog(int chapter, int scene) {
		yield return new WaitForSeconds(0.5f);
		dialog.StartScene(chapter, scene);
	}
	
	private void HandleDialogFinished() {
		_actNumber++;
	}
	
	private void FadeFinishedHandler() {
		if(_exiting) {
			StartCoroutine(StartScene());
		}
	}


}
