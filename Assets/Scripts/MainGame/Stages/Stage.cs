// abstract class that defines the common characteristics of all stages

using UnityEngine;
using System.Collections;

abstract public class Stage : MonoBehaviour {

	void Awake(){
		if (GameValuesContainer.container == null) {
			GameValuesContainer.container = new GameValuesContainer ();
		}
		GameValuesContainer.container.currentStage = this;
		GameValuesContainer.container.stageDuration = GetStageDuration ();
	}

	abstract public float[] BeatsTimes ();
	abstract public int[] BeatsTypes ();
	abstract public int[] BeatsTargets ();
	abstract public int[] BeatsDifficultiesMin ();
	abstract public int[] BeatsDifficultiesMax ();

	abstract public float[] SpawnsTimes ();
	abstract public int[] SpawnsTypes ();
	abstract public float[] SpawnsPositions ();
	abstract public int[] SpawnsDifficultiesMin ();
	abstract public int[] SpawnsDifficultiesMax ();

	float GetStageDuration(){
		float[] spawnTimes = SpawnsTimes ();
		int[] spawnTypes = SpawnsTypes ();
		for (int i = 0; i < spawnTypes.Length; i++) {
			if (spawnTypes [i] == -1) {
				return spawnTimes[i];
			}
		}
		return -1f;
	}
}
