// script used to spawn enemies and asteroids
// attached to an object not visible on the screen, at the top of the playing field

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public GameObject[] enemies; // ennemies that can be generated (ships, asteroids...)
	public GameObject boss;
	private Queue<EnemyInformation> spawns;

	private int difficulty = 1; // difficulty (will varies in function of used items
	private IEnumerator coroutineBoss;

	private struct EnemyInformation{
		public EnemyInformation(float time, int type, float position, int diffMin, int diffMax){
			this.time = time;
			this.type = type;
			this.position = position;
			this.diffMin = diffMin;
			this.diffMax = diffMax;
		}
		public float time;
		public int type;
		public float position;
		public int diffMin;
		public int diffMax;
	}

	void Start(){
		fillSpawns ();
	}

	void fillSpawns(){ // Defines the times where the beats of the rhythm games are
		spawns = new Queue<EnemyInformation>();
		float[] times = GameValuesContainer.container.currentStage.SpawnsTimes();
		int[] types = GameValuesContainer.container.currentStage.SpawnsTypes();
		float[] positions = GameValuesContainer.container.currentStage.SpawnsPositions();
		int[] diffMin = GameValuesContainer.container.currentStage.SpawnsDifficultiesMin();
		int[] diffMax = GameValuesContainer.container.currentStage.SpawnsDifficultiesMax();
		for (int i=0;i<Mathf.Min(times.Length,types.Length);i++) {
			EnemyInformation info = new EnemyInformation (times [i], types [i], positions [i], diffMin [i], diffMax [i]);
			spawns.Enqueue (info);
		}

	}

	public void SetDifficulty(int difficulty){
		this.difficulty = difficulty;
	}

	public void IncreaseDifficulty(){
		SetDifficulty (Mathf.Min(4,difficulty + 1));
	}

	public void DecreaseDifficulty(){
		SetDifficulty (Mathf.Max(1,difficulty - 1));
	}

	void SpawnEnemy(int enemyIndex, float position){
		GameValuesContainer.container.spawnedEnemies++;
		float size = GetComponent<Renderer> ().bounds.size.z;
		Vector3 spawnPoint = new Vector3(0,0,(position-0.5f)*size) + GetComponent<Renderer> ().bounds.center;
		GameObject enemy = (GameObject)Instantiate (enemies [enemyIndex], spawnPoint, Quaternion.identity);
		enemy.transform.SetParent (GameValuesContainer.container.enemiesContainer.transform);
	}

	void SpawnBoss(float position){
		if (coroutineBoss != null) {
			StopCoroutine (coroutineBoss);
		}
		coroutineBoss = CoroutineSpawnBoss (position);
		StartCoroutine (coroutineBoss);
	}

	IEnumerator CoroutineSpawnBoss(float position){
		GameValuesContainer.container.audioHandler.PlayWarning ();
		GameValuesContainer.container.shooterHandler.DisplayIndication ("The Boss is approaching !",false,5f);
		yield return new WaitForSeconds (2f);
		float size = GetComponent<Renderer> ().bounds.size.z;
		Vector3 spawnPoint = new Vector3(0,0,(position-0.5f)*size) + GetComponent<Renderer> ().bounds.center;
		Instantiate (boss, spawnPoint, Quaternion.identity);
		coroutineBoss = null;
	}

	// Update is called once per frame
	void Update () {
		if (GameValuesContainer.container.audioHandler.isPlayingRhythmGame()) {
			SpawnAtTimes ();
		}
	}

	void SpawnAtTimes(){
		float currentAudioTime = GameValuesContainer.container.rhythmHandler.spawnSpot.GetTime ();
		while (spawns.Count > 0 && spawns.Peek ().time <= currentAudioTime) {
			EnemyInformation info = spawns.Dequeue ();
			if (info.type == -1) {
				SpawnBoss (info.position);
			}
			else if(info.diffMin<=difficulty && info.diffMax>=difficulty){
				SpawnEnemy (info.type, info.position);
			}
		}
	}
}
