// Sample Reference
// https://docs.microsoft.com/en-us/dotnet/api/microsoft.cognitiveservices.speech.translation.translationrecognizer?view=azure-dotnet

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;
using TMPro;

public class ContinuousSpeechTranslation : MonoBehaviour {

	// Config
	private bool isUIVisible;
	private GameObject ui;
	private Canvas uiCanvas;
	private CanvasScaler uiScaler;

	private InputField apiKeyInputField;
	private InputField apiRegionInputField;

	private Dropdown modeList;

	private Button startStopButton;
	private Text startStopButtonText;

	// Subtitles
	private GameObject subtitles;
	private TextMeshPro subtitlesText;

	private string translatingMessage;
	private Color translatingMessageColor;

	private string translatedMessage;
	private Color translatedMessageColor;

	// Engine
	private TranslationRecognizer recognizer;
	private object threadLocker = new object();
	private bool isRecognitionStarted;
	private bool isRecognitionStateChanged;





	void Start() {
		Debug.Log("Start");

		isRecognitionStarted = false;
		isRecognitionStateChanged = false;
		recognizer = null;

		ui = GameObject.Find("Canvas");
		uiCanvas = ui.GetComponent<Canvas>();
		uiScaler = ui.GetComponent<CanvasScaler>();
		isUIVisible = true;
		HideUI();

		apiKeyInputField = GameObject.Find("ApiKeyInputField").GetComponent<InputField>();
		apiRegionInputField = GameObject.Find("ApiRegionInputField").GetComponent<InputField>();

		modeList = GameObject.Find("ModeList").GetComponent<Dropdown>();
		modeList.onValueChanged.AddListener(ModeListValueChangeHandler);

		startStopButton = GameObject.Find("StartStopButton").GetComponent<Button>();
		startStopButton.onClick.AddListener(StartStopButtonClickHandler);
		startStopButtonText = GameObject.Find("StartStopButtonText").GetComponent<Text>();
		applyStartStopButtonLabel();

		subtitles = GameObject.Find("Subtitles");
		subtitlesText = subtitles.GetComponent<TextMeshPro>();
		subtitlesText.text = "";

		float gray = 0.8f;
		translatingMessageColor = new Color(gray, gray, gray, 0.5f);
		translatedMessageColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}

	void Update() {
		lock (threadLocker) {
			applySubtitles();
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			ToggleUI();
		}

		if (isRecognitionStateChanged) {
			applyStartStopButtonLabel();
		}

		isRecognitionStateChanged = false;
	}

	void OnDestroy() {
		StopRecognition();
	}





	private void applySubtitles() {
		if (translatedMessage != "") {
			subtitlesText.color = translatedMessageColor;
			subtitlesText.text = translatedMessage;
		} else if (translatingMessage != "") {
			subtitlesText.color = translatingMessageColor;
			subtitlesText.text = translatingMessage;
		} else {
			subtitlesText.text = "";
		}
	}




	private void applyStartStopButtonLabel() {
		if (isRecognitionStarted) {
			startStopButtonText.text = "Stop";
		} else {
			startStopButtonText.text = "Start";
		}
	}

	private async void StartStopButtonClickHandler() {
		ToggleRecognition();
	}





	private async void ModeListValueChangeHandler(int index) {
		Debug.Log("mode list change : " + index.ToString());
		bool isSterted = isRecognitionStarted;
		StopRecognition();
		if (isSterted) {
			await Task.Delay(2000);
			StartRecognition();
		}
	}




	private async void StartRecognition() {
		if (isRecognitionStarted) return;
		Debug.Log("start recognition");

		string fromLang;
		string toLang;
		if (modeList.value == 0) {
			fromLang = "ja-JP";
			toLang = "en";
		} else {
			fromLang = "en-US";
			toLang = "ja";
		}
		Debug.Log("mode : " + fromLang + " -> " + toLang);

		var config = SpeechTranslationConfig.FromSubscription(apiKeyInputField.text, apiRegionInputField.text);
		config.SpeechRecognitionLanguage = fromLang;
		config.AddTargetLanguage(toLang);

		recognizer = new TranslationRecognizer(config);
		recognizer.Canceled += CanceledHandler;
		recognizer.SessionStarted += SessionStartedHandler;
		recognizer.SessionStopped += SessionStoppedHandler;
		recognizer.SpeechStartDetected += SpeechStartDetectedHandler;
		recognizer.SpeechEndDetected += SpeechEndDetectedHandler;
		recognizer.Recognizing += RecognizingHandler;
		recognizer.Recognized += RecognizedHandler;

		await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

		isRecognitionStarted = true;
		isRecognitionStateChanged = true;
	}

	private async void StopRecognition() {
		if (!isRecognitionStarted) return;
		Debug.Log("stop recognition");

		translatingMessage = "";
		translatedMessage = "";

		await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

		recognizer.Canceled -= CanceledHandler;
		recognizer.SessionStarted -= SessionStartedHandler;
		recognizer.SessionStopped -= SessionStoppedHandler;
		recognizer.SpeechStartDetected -= SpeechStartDetectedHandler;
		recognizer.SpeechEndDetected -= SpeechEndDetectedHandler;
		recognizer.Recognizing -= RecognizingHandler;
		recognizer.Recognized -= RecognizedHandler;
		recognizer.Dispose();
		recognizer = null;

		isRecognitionStarted = false;
		isRecognitionStateChanged = true;
	}

	private async void ToggleRecognition() {
		if (isRecognitionStarted) {
			StopRecognition();
		} else {
			StartRecognition();
		}
	}

	private void CanceledHandler(object sender, TranslationRecognitionEventArgs e) {
		Debug.Log("recognizer canceled");
	}

	private void SessionStartedHandler(object sender, SessionEventArgs e) {
		Debug.Log("recognizer session started");
	}

	private void SessionStoppedHandler(object sender, SessionEventArgs e) {
		Debug.Log("recognizer session stopped");
	}

	private void SpeechStartDetectedHandler(object sender, RecognitionEventArgs e) {
		Debug.Log("recognizer speech start detected");
	}

	private void SpeechEndDetectedHandler(object sender, RecognitionEventArgs e) {
		Debug.Log("recognizer speech end detected");
	}

	private void RecognizingHandler(object sender, TranslationRecognitionEventArgs e) {
		Debug.Log("recognizing : reason = " + e.Result.Reason + ", text = " + e.Result.Text);

		lock (threadLocker) {
			if (e.Result.Reason == ResultReason.TranslatingSpeech) {
				translatedMessage = "";
				translatingMessage = "";
				foreach (var element in e.Result.Translations) {
					Debug.Log("	translations : " + element.Key + " -> " + element.Value);
					translatingMessage += element.Value;
				}
			}
		}
	}

	private void RecognizedHandler(object sender, TranslationRecognitionEventArgs e) {
		Debug.Log("recognized : " + e.Result.Text);

		if (e.Result.Reason == ResultReason.TranslatedSpeech) {
			translatedMessage = "";
			foreach (var element in e.Result.Translations) {
				Debug.Log("	translations : " + element.Key + " -> " + element.Value);
				translatedMessage += element.Value;
			}
		}
	}





	private void ShowUI() {
		if (isUIVisible) return;
		isUIVisible = true;

		uiCanvas.targetDisplay = 0;
	}

	private void HideUI() {
		if (!isUIVisible) return;
		isUIVisible = false;

		uiCanvas.targetDisplay = 1;
	}

	private void ToggleUI() {
		if (isUIVisible) {
			HideUI();
		} else {
			ShowUI();
		}
	}
}