using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Manager : MonoBehaviour {

	PlanetWrap currentSelectedPlanet;
	LevelWrap currentSelectedLevel;

	Coroutine moveTransform;
	Coroutine rotateTrasnform;

	void MoveToSelectedPlanet(PlanetWrap selectedPlanet) { // from planet selection
		SetupPlanet(selectedPlanet);
		MoveTrasformToLocation(playerTransform, selectedPlanet.cameraLocation);
		CurrentGameState = GameState.InOrbit;
	}

	public void MovetoSelectedPlanet() { // referenced by UI buttons 
		SetupPlanet(currentSelectedPlanet);
		MoveTrasformToLocation(playerTransform, currentSelectedPlanet.cameraLocation);
		CurrentGameState = GameState.InOrbit;
	}

	public void MoveToMenu() { // referenced by UI buttons 
		currentSelectedPlanet.enableCollider = true;
		CleanupPlanet();
		MoveTrasformToLocation(playerTransform, cameraTopPosition);
		CurrentGameState = GameState.InMenu;
	}

	void MoveToSelectedLevel(LevelWrap level) {
		SetupLevel(level);
		StartCoroutine(RotateTransform(currentSelectedPlanet.planetTransform, currentSelectedPlanet.planetTransform.localRotation, .1f, 0f, level));
		CurrentGameState = GameState.InBattle;
	}
	


	//core move logic
	/// <summary>
	/// You must specify whatToMove and currentPos outside the function, as separate variables.  whatToMove.position will change with each iteration.
	/// </summary>
	/// <param name="whatToMove"></param>
	/// <param name="currentPos"></param>
	/// <param name="newPos"></param>
	/// <param name="duration">  how long to take to get there in seconds </param>
	/// <param name="counter"> you can control the percentage with this value for a partial action, 0 is 100% 0.1 is 90%, 0.2 80% all the way to 1 is 0%</param>
	/// <returns></returns>
	IEnumerator MoveTransform(Transform whatToMove, Vector3 currentPos, Vector3 newPos, float duration, float counter) {// must cache the positions otherwise the lerp recalculates the position at each step!!!!!
		MoveState(true, newPos);
		while (counter < duration) {
			counter += Time.deltaTime;
			whatToMove.position = Vector3.Lerp(currentPos, newPos, counter / duration);
			yield return null;
		}
		MoveState(false, newPos);
	}

	void MoveState( bool started,Vector3 whereToLook ) {
		mainCameraTransform.GetChild(0).LookAt(whereToLook);
		warpDrive.SetActive(started);
		allowInput=!started;
		currentSelectedPlanet.allowRotation=!started;
	}

	/// <summary>
	/// You must specify whatToMove and currentRot outside the function, as separate variables.  whatToMove.rotation will change with each iteration.
	/// </summary>
	/// <param name="currentRot"></param>
	/// <param name="newRot"></param>
	/// <param name="duration"> how long to take to get there in seconds </param>
	/// <param name="counter"> you can control the percentage with this value for a partial action, 0 is 100% 0.1 is 90%, 0.2 80% all the way to 1 is 0%</param>
	/// <returns></returns>
	IEnumerator RotateTransform(Transform whatToRotate, Quaternion currentRot, Quaternion newRot, float duration, float counter) {// must cache the positions otherwise the lerp recalculates the position at each step!!!!!
		while (counter < duration) {
			counter += Time.deltaTime;
			whatToRotate.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
			yield return null;
		}
	}
	//Overload for planet positioning.
	IEnumerator RotateTransform( Transform whatToRotate, Quaternion currentRot, float duration, float counter, LevelWrap level ) {// must cache the positions otherwise the lerp recalculates the position at each step!!!!!
		MoveState(true, whatToRotate.position);
		while ( counter<duration ) {
			counter+=Time.deltaTime;
			whatToRotate.localRotation=Quaternion.Lerp(currentRot, currentSelectedPlanet.levelPositions[level.id], counter/duration);
			yield return null;
		}
		MoveTrasformToLocation(playerTransform, level.levelTransform, .5f);
	}

	/// <summary>
	///  Move and Rotate from one location to another over _duration (1f=1second)
	/// </summary>
	/// <param name="_whatToMove"></param>
	/// <param name="_whereToMove"></param>
	/// <param name="_duration"> how long to take to get there in seconds</param>
	public void MoveTrasformToLocation(Transform _whatToMove, Transform _whereToMove, float _duration) {
		float counter = 0f;
		_whatToMove.SetParent(_whereToMove);
		_whatToMove.SetAsFirstSibling();
		if (moveTransform != null) StopCoroutine(moveTransform);
		if (rotateTrasnform != null) StopCoroutine(rotateTrasnform);
		moveTransform = StartCoroutine(MoveTransform(_whatToMove, _whatToMove.position, _whereToMove.position, _duration, counter));
		rotateTrasnform = StartCoroutine(RotateTransform(_whatToMove, _whatToMove.rotation, _whereToMove.rotation, _duration, counter));
	}

	/// <summary>
	/// Move and Rotate from one location to another over 1 second.
	/// </summary>
	/// <param name="_whatToMove"></param>
	/// <param name="_whereToMove"></param>
	public void MoveTrasformToLocation(Transform _whatToMove, Transform _whereToMove) {
		float counter = 0f;
		float _duration = 1f;
		_whatToMove.SetParent(_whereToMove);
		_whatToMove.SetAsFirstSibling();
		if (moveTransform != null) StopCoroutine(moveTransform);
		if (rotateTrasnform != null) StopCoroutine(rotateTrasnform);
		moveTransform = StartCoroutine(MoveTransform(_whatToMove, _whatToMove.position, _whereToMove.position, _duration, counter));
		rotateTrasnform = StartCoroutine(RotateTransform(_whatToMove, _whatToMove.rotation, _whereToMove.rotation, _duration, counter));
	}

	public void RotateObject(Transform objectToRotate, float directionX, float directionY, Space space) {
		objectToRotate.Rotate(mainCameraTransform.up * directionX, space); // rotate this object's transform by however the camera is located-up (or y axis) *speed*rotation speed, in the world's coordinates
		objectToRotate.Rotate(mainCameraTransform.right * directionY, space);// see above for right (x ) axis
	}
}
