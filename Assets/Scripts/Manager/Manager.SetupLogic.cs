using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Manager : MonoBehaviour {

	// setup planet
	void ToggleLevelsOnPlanet(bool value) {
		foreach (KeyValuePair<Transform, LevelWrap> entry in currentSelectedPlanet.levels) {
			entry.Value.levelMarker.SetActive(value);
		}
	}

	void SetupPlanet(PlanetWrap planet) {
		currentSelectedPlanet = planet;
		planet.enableCollider = false;
		ToggleLevelsOnPlanet(true);
		CleanupLevel();
	}

	void CleanupPlanet() {
		ToggleLevelsOnPlanet(false);
	}


	// setup level
	void SetupLevel(LevelWrap level) {
		currentSelectedLevel = level;
		MoveMainBattle(level.levelTransform);
		ToggleLevelsOnPlanet(true);
		level.visited = true;
	}

	void CleanupLevel() {
		mainBattle.battleTransform.gameObject.SetActive(false);
	}
}
