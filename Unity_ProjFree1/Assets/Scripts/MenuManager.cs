using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour{
	private enum PlayState {
		StartMenu,
		PauseMenu,
		EndMenu,
		noMenu
	};

	private PlayState playState;

	[Header("Keys")]
	public KeyCode pauseKey;
	public KeyCode exitKey;
	public KeyCode nextKey;
	public KeyCode prevKey;

	[Header("Menu")]
	public GameObject menuObj;

	[Space(10)]
	public Image dialogImage;
	public Sprite[] introSprites;
	public Sprite[] outroSprites;
	private Sprite[] dialogSprites;

	[Space(10)]
	public Text dialogText;
	public string[] introStrings;
	public string[] outroStrings;
	private string[] dialogStrings;
	private int dialogId;

	[Space(10)]
	public Text startButton;
	public Text returnButton;
	public Button nextButton;
	public Button prevButton;
	private bool canNextBt;
	private bool canPrevBt;

	[Header("Effects")]
	public AudioSource soundObj;
	public AudioClip hoverSound;
	public AudioClip clickSound;

	void Start(){
		if(Manager.firstLoad == true) {
			StateSwitch();
			CheckDialogButtons();
		}
	}

    void Update(){
		ButtonCheck();
	}

	public void OnStartButton() {
		if(playState == PlayState.StartMenu) {
			StartGame();
		} else if (playState == PlayState.PauseMenu || playState == PlayState.EndMenu) {
			ToStartMenu();
		}
	}

	public void OnReturnButton() {
		if(playState == PlayState.StartMenu || playState == PlayState.EndMenu) {
			ExitGame();
		} else if (playState == PlayState.PauseMenu){
			playState = PlayState.noMenu;
			StateSwitch();
		}
	}

	void StartGame() {
		playState = PlayState.noMenu;
		StateSwitch();
	}

	void ToStartMenu() {
		playState = PlayState.StartMenu;
		StateSwitch();
	}

	void ExitGame() {
		Application.Quit();
	}

	public void EndGame() {
		playState = PlayState.EndMenu;
		StateSwitch();
	}

	public void SwitchDialog(int count) {
		dialogId += count;
		if (dialogId >= dialogSprites.Length || dialogId < 0) {
			return;
		}

		dialogImage.sprite = dialogSprites[dialogId];
		dialogText.text = dialogStrings[dialogId];

		CheckDialogButtons();
	}

	void CheckDialogButtons() {
		canNextBt = dialogId < dialogSprites.Length -1;
		nextButton.interactable = canNextBt;

		canPrevBt = dialogId > 0;
		prevButton.interactable = canPrevBt;
	}

	void ButtonCheck() {
		//exit the game when in start or end menu and toggles pause
		if(Input.GetKeyDown(exitKey)) {
			if (playState == PlayState.StartMenu || playState == PlayState.EndMenu) {
				ExitGame();
			} else if (playState == PlayState.PauseMenu) {
				playState = PlayState.noMenu;
				StateSwitch();
			} else if(playState == PlayState.noMenu) {
				playState = PlayState.PauseMenu;
				StateSwitch();
			}
		//toggles pause
		} else if(Input.GetKeyDown(pauseKey)) {
			if(playState == PlayState.PauseMenu) {
				playState = PlayState.noMenu;
				StateSwitch();
			} else if(playState == PlayState.noMenu) {
				playState = PlayState.PauseMenu;
				StateSwitch();
			}
		//handles the dialog scenes
		} else if(Input.GetKeyDown(nextKey) && canNextBt) {
			SwitchDialog(1);
		} else if(Input.GetKeyDown(prevKey) && canPrevBt) {
			SwitchDialog(-1);
		}
		
	}

	void StateSwitch() {
		switch(playState) {
			case PlayState.StartMenu:
				StartMenu();
				break;
			case PlayState.PauseMenu:
				PauseMenu();
				break;
			case PlayState.EndMenu:
				EndMenu();
				break;
			case PlayState.noMenu:
				NoMenu();
				break;
		}
	}

	void StartMenu() {
		menuObj.SetActive(true);
		Time.timeScale = 0;
		startButton.text = "Start";
		startButton.fontSize = 23;
		returnButton.text = "Exit";

		SetSelected(startButton.transform.parent.gameObject);
		GetDialog(introSprites, introStrings);
	}

	void PauseMenu() {
		menuObj.SetActive(true);
		Time.timeScale = 0;
		startButton.text = "Start Menu";
		startButton.fontSize = 18;
		returnButton.text = "Return";

		SetSelected(returnButton.transform.parent.gameObject);
		GetDialog(introSprites, introStrings);
	}

	void EndMenu() {
		menuObj.SetActive(true);
		Time.timeScale = 0;
		startButton.text = "Start Menu";
		startButton.fontSize = 18;
		returnButton.text = "Exit";

		SetSelected(startButton.transform.parent.gameObject);
		dialogId = 0;
		GetDialog(outroSprites, outroStrings);
	}

	void GetDialog(Sprite[] dSprites, string[] dStrings) {
		dialogSprites = dSprites;
		dialogStrings = dStrings;

		dialogImage.sprite = dialogSprites[dialogId];
		dialogText.text = dialogStrings[dialogId];
	}

	void SetSelected(GameObject button) {
		EventSystem es = EventSystem.current;
		es.SetSelectedGameObject(null);
		es.firstSelectedGameObject = button;
		es.SetSelectedGameObject(button);
	}

	//in gameplay
	void NoMenu() {
		menuObj.SetActive(false);
		Time.timeScale = 1;
	}

	public void OnSelect(GameObject glow) {
		glow.SetActive(true);
	}

	public void OnDeselect(GameObject glow) {
		glow.SetActive(false);
	}

	public void OnHover() {
		if (soundObj && hoverSound)
			soundObj.PlayOneShot(hoverSound);
	}

	public void OnClick() {
		if(soundObj && clickSound)
			soundObj.PlayOneShot(clickSound);
	}
}
