using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Manager : MonoBehaviour {

	// dictionary is to reference the Transfrom in scene to Enemey object
	public Dictionary<Transform, EnemyWrap> enemyesDisctionary = new Dictionary<Transform, EnemyWrap>();
	// array is to iterate and get random or by type
	public EnemyWrap[,] enemyArray;
	// dictionary is to reference the Transfrom in scene to Planet object 
	public Dictionary<Transform, PlanetWrap> planetsDictionary = new Dictionary<Transform, PlanetWrap>();

	[Header("World Data")]

	[SerializeField] Transform mainCameraTransform;
	[SerializeField] Transform cameraTopPosition;
	[SerializeField] Transform planetsHolder;
	[SerializeField] Transform enemiesHolder;
	[SerializeField] Transform battleHolder;




	void InitializeWorld() { // get all planets and levels
		for( int i = 0; i<planetsHolder.childCount; i++ ) {
			planetsDictionary.Add(CreatePlanet(planetsHolder.GetChild(i), i).planetGameObject.transform, CreatePlanet(planetsHolder.GetChild(i), i));
		}
	}

	PlanetWrap CreatePlanet( Transform orbit, int iteration ) {
		PlanetWrap newPlanet = new PlanetWrap(iteration, orbit.GetChild(0).gameObject);
		for( int i = 0; i<newPlanet.numberOfLevels; i++ ) {
			LevelWrap newLevel = new LevelWrap(i, newPlanet.planetTransform.GetChild(i).gameObject);
			newPlanet.levels.Add(newLevel.levelTransform, newLevel);
		}
		return newPlanet;
	}

	void InitializeEnemeies() { // get all enemies
		int numberOfEnemyTypes = enemiesHolder.childCount;
		int numberOfEnemiesInType = enemiesHolder.GetChild(0).childCount;
		enemyArray=new EnemyWrap[numberOfEnemyTypes, numberOfEnemiesInType];
		for( int i = 0; i<numberOfEnemyTypes; i++ ) {
			for( int j = 0; j<numberOfEnemiesInType; j++ ) {
				int id = ( i+1 )*numberOfEnemiesInType+j;
				EnemyWrap thisEnemy = new EnemyWrap(id, "type"+i+"Number"+j, enemiesHolder.GetChild(i).GetChild(j).gameObject);
				enemyArray[i, j]=thisEnemy;
				enemyesDisctionary.Add(thisEnemy.myTransform, thisEnemy);
			}
		}
	}
}