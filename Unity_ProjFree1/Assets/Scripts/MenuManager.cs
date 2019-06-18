using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	public Image dialogImage;
	public Sprite[] introSprites;
	public Sprite[] outroSprites;
	[Space(10)]
	public Text dialogText;
	public string[] introTexts;
	public string[] outroTexts;
	[Space(10)]
	public Text startButton;
	public Text returnButton;
	public Button nextButton;
	public Button prevButton;
	[Space(10)]
	public Text keyText;
	


    void Start(){
		//keyText.text = "Interact " + CharacterSkills.pickupKey;

	}


    void Update(){
		StateCheck();
	}

	void StateCheck() {
		switch(playState) {
			case PlayState.StartMenu:
				if(Input.GetKeyDown(exitKey)) {
					//exit game
				}
				break;
			case PlayState.PauseMenu:
				if(Input.GetKeyDown(pauseKey) || Input.GetKeyDown(exitKey)) {
					playState = PlayState.noMenu;
				}
				break;
			case PlayState.EndMenu:
				if(Input.GetKeyDown(exitKey)) {
					//exitgame
				}
				break;
			case PlayState.noMenu:
				if(Input.GetKeyDown(pauseKey) || Input.GetKeyDown(exitKey)) {
					playState = PlayState.PauseMenu;
				}
				break;
		}
	}

	void StateSwitch() {

	}

	void StartMenu() {
		startButton.text = "Start";
		returnButton.text = "Exit";
	}

	void PauseMenu() {
		startButton.text = "StartMenu";
		returnButton.text = "Return";
	}

	void EndMenu() {
		startButton.text = "StartMenu";
		returnButton.text = "Exit";
	}
}
