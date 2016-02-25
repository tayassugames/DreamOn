using UnityEngine;
using System.Collections;

public class CollisionSceneLoader : MonoBehaviour {
	
	public SceneControllerBase sceneController;
	public ExitSelection selectedExit = ExitSelection.first;
	
	void OnTriggerEnter() {
		sceneController.LoadNextScene(selectedExit);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
