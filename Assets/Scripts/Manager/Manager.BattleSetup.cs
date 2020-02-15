using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Manager : MonoBehaviour {

	public BattleWrap mainBattle;
	void InitializeBattle() {
		mainBattle = new BattleWrap(battleHolder);
		mainBattle.battleGameObject.SetActive(false);
	}


	public void DoBattleSetup() {
		EnemyWrap[] toPosition = Get5RandomEnemies();
		for (int i = 1; i < toPosition.Length; i++) {
			toPosition[i - 1].myTransform.position = mainBattle.enemyPositions[i].position;
			toPosition[i - 1].myTransform.rotation = mainBattle.enemyPositions[i].rotation;
			toPosition[i - 1].myTransform.SetParent(mainBattle.enemyPositions[i]);
		}
	}

	EnemyWrap[] Get5RandomEnemies() {
		EnemyWrap[] arrayOfRandomEnemies = new EnemyWrap[5];
		bool pickedCorrectly = false;
		EnemyWrap pickedEnemy;
		for (int i = 0; i < mainBattle.enemyPositions.Length; i++) {
			do {
				int randomType = Random.Range(0, enemyArray.GetLength(0));
				int randomTypeNumber = Random.Range(0, enemyArray.GetLength(1));
				pickedEnemy = enemyArray[randomType, randomTypeNumber];
				if (!pickedEnemy.amIPositioned) pickedCorrectly = true; 
			} while (!pickedCorrectly);
			pickedEnemy.amIPositioned = true;
			arrayOfRandomEnemies[i] = pickedEnemy;
		}
		return arrayOfRandomEnemies;
	}

	public void MoveMainBattle(Transform location) {
		battleHolder.gameObject.SetActive(true);
		mainBattle.battleTransform.SetParent(location);
		mainBattle.battleTransform.position = location.position;
		mainBattle.battleTransform.rotation = location.rotation;
	}
}
