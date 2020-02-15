using UnityEngine;
using TMPro;

public partial class Manager : MonoBehaviour {
	public static Manager manager;

	static public GameData saveDataInMememory;
	static public SettingsData settingsDataInMemory;



	[Header("Game Effects")]

	[SerializeField] GameObject warpDrive;

	[Header("Game Meta")]
	public bool allowInput; //ability to disable input
	private GameState gameState;
	public TextMeshPro displayText; //for debug remove before final

	[Header("Game Variables")]
	public Transform playerTransform;

	private void Update() {
		////debug shit

		//if (Input.GetKeyUp(KeyCode.A)) {
		//	MoveToSelectedPlanet(planets[0]);
		//}
		//if (Input.GetKeyUp(KeyCode.S)) {
		//	MoveToSelectedPlanet(planets[1]);
		//}
		//if (Input.GetKeyUp(KeyCode.D)) {
		//	MoveToMenu();
		//}

		////debug shit end
		MouseInput();
	}

	void Awake() {
		InitializeEffects();
		InitializeManager();
		InitializeInput(); // initialize input
		InitializeWorld(); // get all the components
		InitializeEnemeies(); // get all the enemies
		InitializeData(); //get the saved data
		InitilizeSounds();
		InitializeBattle(); // get the battle default layout
		InistializeMenuButtons();
	}

	void InitializeEffects() {
		warpDrive.SetActive(false);
	}

	void InitializeManager() {
		if (manager == null) { //if there is not a dataManager already in this scene
			manager = this; // the control is this object
		} else if (manager != this) { // if the dataManager is not this
			Destroy(gameObject); //kill this game object
		}
	}

	void InitializeData() {
		saveDataInMememory = LoadData<GameData>();
		settingsDataInMemory = LoadData<SettingsData>();
	}
	public T LoadData<T>() {
		return SaveSystem.LoadData<T>();
	}

	public void SaveData(object t) {
		SaveSystem.SaveData(t);
	}

	public GameState CurrentGameState {
		get => gameState;
		set {
			gameState = value;
			ChangeMenu(value);
		}
	}
}
public enum GameState {
	InBattle,
	InOrbit,
	InMenu,
	OnPlanet
}

