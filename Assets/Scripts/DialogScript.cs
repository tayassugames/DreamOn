using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;

public class DialogScript : MonoBehaviour {
	
	public enum Language {
		ENGLISH = 0,
		SPANISH = 1,
		PORTUGUESE = 2
	}
	
	#region Static Variables
	public static Language language = Language.ENGLISH;
	#endregion
	
	#region Events
	public delegate void DialogEndHandler();
	public event DialogEndHandler OnDialogEnd;
	#endregion
	
	#region GUI Texture Placeholders
	//Texturas para las distintas partes.
	public Texture imagePlaceHolder;
	public Texture dialogPlaceHolder;
	public Texture textPlaceHolder;
	public Texture iconPlaceHolder;
	public Texture skipImage;
	
	public List<Texture> images;
	
	#endregion
	
	#region Rectangle GUI Sizes
	public Rect dialog = new Rect(70, 8, 500, 128);  		// Tamaños deben ser definidos.
	public Rect image = new Rect(0, 0, 128, 0); 		//El ancho de la imagen debe ser definido siempre.
	public Rect text = new Rect(0, 0, 0, 0); 			//Se calcula por completo.
	public Rect icon = new Rect(0, 0, 20, 20);			//El ancho y altura deben ser definidos.
	public Rect skipButton = new Rect(283, 140, 74, 32);			//El ancho y altura deben ser definidos.
	#endregion
	
	#region Public Text Printing Variables
	public GUIStyle textStyle = new GUIStyle();
	//El tamaño del "marco".
	public int imageGapSize = 10;
	public bool pictureLeft = true;
	//Velocidad del texto.
	public float textSpeed = 0.05f;
	//Entre cuanto dividir la velocidad del texto a la hora de alentar.
	public float slowerSpeed = 3f;
	
	public int currentChapter;
	public int currentScene;
	public TextAsset dialogFile;
	public List<TextAsset> dialogFiles;
	public bool skipEnabled = false;
	public bool useFeedback = false;
	#endregion
	
	#region Private Text Printing Variables
	//Donde se irá almacenando lo que se imprime en pantalla.
	private string _printedPhrase = "";
	//Para restaurar la velocidad.
	private float _originalTextSpeed = -1f;
	//Indicador de por donde se va en la hilera.
	private int _textPosition = 0;
	//Para saber si se debe dejar de escribir en el frame.
	private bool _continueWriting = true;
	//Para saber si se muestra el ícono de pasar al proximo.
	private bool _showIcon = false;
	//private bool _lastWord = false;
	private string _originalPhrase = "";
	
	#endregion
	
	#region Text Speed Methods
	private void SpeedUpText(){
		textSpeed = _originalTextSpeed / slowerSpeed;
	}
	
	private void RestoreTextSpeed(){
		textSpeed = _originalTextSpeed;
	}
	#endregion
	
	#region XML File Variables
	private XmlReader _xmlReader;
	private XmlDocument _xmlDocument;
	private XmlNode _chapterNode;
	private XmlNode _sceneNode;
	private int _dialogIndex = -1;
	private bool _inScene = false;
	private XmlNode _rootNode;
	#endregion
	
	private bool _isTouching = false;
	private bool _closeDialog = false;
	
	#region MonoBehavior methods
	
	void OnEnable() {
		Gesture.onTouchE += HandleGestureonTouchE;
		Gesture.onMouse1E += HandleGestureonTouchE;
		Gesture.onTouchDownE += HandleGestureonTouchDownE;
		Gesture.onMouse1DownE += HandleGestureonTouchDownE;
		Gesture.onTouchUpE += HandleGestureonTouchUpE;
		Gesture.onMouse1UpE += HandleGestureonTouchUpE;
	}

	void OnDisable() {
		Gesture.onTouchE -= HandleGestureonTouchE;
		Gesture.onMouse1E -= HandleGestureonTouchE;
		Gesture.onTouchDownE -= HandleGestureonTouchDownE;
		Gesture.onMouse1DownE -= HandleGestureonTouchDownE;
		Gesture.onTouchUpE -= HandleGestureonTouchUpE;
		Gesture.onMouse1UpE -= HandleGestureonTouchUpE;
	}
	
	
	void Awake() {
		if(imagePlaceHolder == null)
			throw new NullReferenceException("image place holder not found");
		if(dialogPlaceHolder == null)
			throw new NullReferenceException("dialog place holder not found");
		//if(textPlaceHolder == null)
		//	throw new NullReferenceException("text place holder not found");
		if(iconPlaceHolder == null)
			throw new NullReferenceException("icon place holder not found");
		
		useFeedback = useFeedback && (FeedbackLabel.Instance != null);
			
	}
	
	// Use this for initialization
	void Start(){
		_originalTextSpeed = textSpeed;
		_xmlDocument = new XmlDocument();
		
		TextAsset currentDialogFile = null;
		
		switch(DialogScript.language) {
			case Language.ENGLISH:
				currentDialogFile = dialogFiles[0];
				break;
			case Language.SPANISH:
				currentDialogFile = dialogFiles[1];
				break;
			case Language.PORTUGUESE:
				currentDialogFile = dialogFiles[2];
				break;
			default:
				currentDialogFile = dialogFiles[0];
				break;
		}
		
		_xmlDocument.Load(new StringReader(currentDialogFile.text));

		_rootNode = _xmlDocument.DocumentElement;

		
		//Center dialog horizontally
		if(Screen.width > 640) {
			textStyle.fontSize = 48;	
		}
	}
	
	//GUI on screen drawing
	void OnGUI(){
		if(_inScene){
			//Background rect
			GUI.DrawTexture(ScaleManager.GetScaledRect(dialog), dialogPlaceHolder);
			
			//TextBox
			if(textPlaceHolder != null) {
				GUI.DrawTexture(ScaleManager.GetScaledRect(text), textPlaceHolder);
			}
				
			//Icon
			if(_showIcon){
				GUI.DrawTexture(ScaleManager.GetScaledRect(icon), iconPlaceHolder);
			}
			//Image
			GUI.DrawTexture(ScaleManager.GetScaledRect(image), imagePlaceHolder);
			//Text
			GUI.Label(ScaleManager.GetScaledRect(text), _printedPhrase, textStyle);
			
			//Skip button
			if(skipEnabled) {
				Rect localizedSkip = skipButton;
				localizedSkip.y += dialog.y;
				if(GUI.Button(ScaleManager.GetScaledRect(localizedSkip), "skip")) {
					EndScene();	
				}
				GUI.DrawTexture(ScaleManager.GetScaledRect(localizedSkip), skipImage);
			}
		}
	}
	
	//Updating GUI positions (and debug key events)
	void LateUpdate(){
		if(!_inScene)
			return;
		
		//La caja de texto siempre sera del mismo tamaño.
		text.width = (dialog.width - (imageGapSize * 3) - image.width);
		text.height = (dialog.height - (imageGapSize * 2));
		
		//Todo se reacomoda dependiendo si se quiere la imagen
		//a la izquierda o a la derecha.
		if(pictureLeft){
			image.x = dialog.x + imageGapSize;
			image.y = dialog.y + imageGapSize;
			
			image.height = dialog.height - (imageGapSize * 2);
			
			text.x = dialog.x + (imageGapSize * 2) + image.width;
			text.y = dialog.y + imageGapSize;
		}
		else{
			image.x = dialog.x + (imageGapSize * 2) + text.width;
			image.y = dialog.y + imageGapSize;
			
			image.height = dialog.height - (imageGapSize * 2);
			
			text.x = dialog.x + imageGapSize;
			text.y = dialog.y + imageGapSize;
		}
		
		//El icono siempre ira en la esquina derecha del texto.
		icon.x = text.x + text.width - icon.width;
		icon.y = text.y + text.height - icon.height;
		
		
		//para hacer el texto mas rapido
		if(_isTouching){
			SpeedUpText();
		} else {
			RestoreTextSpeed();
		}
		
//	}
	
//	void LateUpdate() {
		//_isTouching = false;
		if(_inScene && _closeDialog) {
			InitDialog();
			
			StopCoroutine("UpdatePrintedText");
			if(OnDialogEnd != null) {
				OnDialogEnd();
			}
		}
	}
	
	#endregion
	
	private void InitDialog() {
		_inScene = false;
		_closeDialog = false;
		_printedPhrase = string.Empty;
		_textPosition = 0;
		_continueWriting = true;
		_showIcon = false;
		_originalPhrase = string.Empty;
	}
	
	//Corutina que se ejecuta mientras estemos en dialogos (DEBE CAMBIARSE XQ AHORITA ESTA ETERNO)
	private IEnumerator UpdatePrintedText(){
		while(_inScene){
			//Puede seguir escribiendo la misma frase si cabe.
			if(_continueWriting){
				if(_textPosition < _originalPhrase.Length){
					//Añado una letra cada vez.
					_printedPhrase += _originalPhrase[_textPosition];
					_textPosition++;
					
					//En cada espacio calculo si la proxima palabra cabe
					if((_originalPhrase[(_textPosition - 1)] == ' ')){
						NextWordFits();
					}
				}
			}
			
			//Si ya no cabe entonces muestre el ícono.
			if(!_continueWriting){
				_showIcon = true;
			}
			
			//Si ya no cabe y se presiona una tecla se continua con el texto.
			if(!_continueWriting && (_isTouching || Input.GetKeyDown(KeyCode.Space))){
				_printedPhrase = "";
				_continueWriting = true;
				_showIcon = false;
			}
			
			//Si llega al final de la frase debería buscar haber si hay otras.
			if(_textPosition == _originalPhrase.Length){
				_showIcon = true;
				if(_isTouching || Input.GetKeyDown(KeyCode.Space)){
					AssignNextDiaglog();
				}
			}
			
			yield return new WaitForSeconds(textSpeed);
		}
	}	
	
	//Para evitar que se escriba una palabra que no cabe.
	private void NextWordFits(){
		string temporaryString = _printedPhrase;
		int temporaryIndex = _textPosition;
		bool overboard = false;
		
		//Creo una hilera temporal con la siguiente palabra
		while(!overboard && (_originalPhrase[temporaryIndex] != ' ')){
			temporaryString += _originalPhrase[temporaryIndex];
			temporaryIndex++;
			
			//En el caso de la ultima palabra evitamos desborde.
			if(temporaryIndex >= _originalPhrase.Length){
				overboard = true; 
			}
		}

		//Me fijo si la hilera temporal que contiene una palabra mas cabe en el recuadro.
		_continueWriting = (textStyle.CalcHeight(new GUIContent(temporaryString), text.width) < text.height);
	}
	
	//Choosing which scene to play.
	public void StartScene(int chapterNumber, int sceneNumber){
		_inScene = true;
		_chapterNode = _rootNode.ChildNodes[chapterNumber];
		_sceneNode = _chapterNode.SelectSingleNode("Scenes").ChildNodes[sceneNumber];
		_dialogIndex = -1;
		
		_originalPhrase = NextDialog();
		currentChapter = chapterNumber;
		currentScene = sceneNumber;
		
		//If the GUI exists, we should diable the collider so it doesnt get pressed by mistake
		if(GameObject.Find("GUI") != null)
		{
			GameObject andyObject = GameObject.Find("AndySmall");
			if(andyObject != null) {
				andyObject.GetComponent<Collider>().enabled = false;
			}
		}
		
		//Starts the text printing coroutine
		StartCoroutine(UpdatePrintedText());
	}
	
	//Finding the next dialog in scene
	public string NextDialog(){
		if(_inScene){
			string dialogLine = "";
			
			_dialogIndex++;
			
			if((_dialogIndex) >= _sceneNode.ChildNodes.Count){
				EndScene();
				return string.Empty;
			}
			
			XmlNode mainNode = _sceneNode.ChildNodes[_dialogIndex];
			
			dialogLine = mainNode.InnerText;
			
			int imageIndex = int.Parse(mainNode.Attributes["ImageIndex"].Value);
			imagePlaceHolder = images[imageIndex];
			
			if(mainNode.Attributes["Top"] != null) {
				
				string[] vertPosString = mainNode.Attributes["Top"].Value.Split('+');
				float vertPos = 0;
				if(vertPosString.Length > 1) {
					if(vertPosString[0] == "HALF") {
						vertPos = Screen.height/2 + float.Parse(vertPosString[1]);
					} else {
						throw new Exception("Vertical value not supported");	
					}
				} else {
					vertPos = float.Parse(vertPosString[0]);
				}
				
				dialog.y = vertPos;
			} else {
				dialog.y = 10;	
			}
			
			if(mainNode.Attributes[1].Value == "L"){
				pictureLeft = true;
			}
			else{
				pictureLeft = false;
			}
			
			return dialogLine.Trim();
		}
		else{
			Debug.LogError("Can't ask for next dialog when inScene = false, call CheckInScene before.");
			
			return string.Empty;
		}
	}
	
	//Setting up the next dialog in scene.
	public void AssignNextDiaglog(){
		_printedPhrase = "";
		_textPosition = 0;
		_originalPhrase = NextDialog();
	}

	//Its a WRAP!
	public void EndScene(){
		_closeDialog = true;
		
		//If the Gui exists we should re-enable the menu so the player can press it 
		if(GameObject.Find("GUI") != null && !Application.loadedLevelName.Equals("TrainerLevel") && !Application.loadedLevelName.Equals("quetzaLevel"))
		{
		GameObject.Find("AndySmall").GetComponent<Collider>().enabled = true;
		}
	}
	
		
	private void HandleGestureonTouchE (Vector2 pos)
	{
		if(useFeedback) {
			FeedbackLabel.Instance.SetText("Touch");	
		}

		if(_inScene)
			_isTouching = true;
	}
	
	private void HandleGestureonTouchDownE (Vector2 pos)
	{
		if(useFeedback) {
			FeedbackLabel.Instance.SetText("TouchDown");	
		}
		
		if(_inScene)
			_isTouching = true;		
	}
	
	private void HandleGestureonTouchUpE (Vector2 pos)
	{
		if(useFeedback) {
			FeedbackLabel.Instance.SetText("TouchUp");	
		}

		if(_inScene)
			_isTouching = false;		
	}
	
	
}
