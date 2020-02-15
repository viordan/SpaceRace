using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Manager : MonoBehaviour {

	[Header("Menu Buttons")]

	[SerializeField] Transform menu;

	GameObject[] menuButtons = new GameObject[3];

	void InistializeMenuButtons() {
		for (int i = 0; i < menu.childCount; i++) {
			menuButtons[i] = menu.GetChild(i).gameObject;
		}
		CurrentGameState = GameState.InMenu; // set the initial state of the game
	}

	void DissableAllButtons() {
		foreach (GameObject button in menuButtons) {
			button.SetActive(false);
		}
	}

	public void ChangeGameState(GameState state) {
		CurrentGameState = state;
	}

	public void SetDisplayText(string value) {
		displayText.text = value;
	}

	void ChangeMenu(GameState state) {
		DissableAllButtons();
		switch (state) {
			case (GameState.InMenu):
			//menuButtons[2].SetActive(true);
			break;
			case (GameState.InOrbit):
			menuButtons[0].SetActive(true);
			break;
			case (GameState.InBattle):
			menuButtons[1].SetActive(true);
			break;
		}
	}
}
