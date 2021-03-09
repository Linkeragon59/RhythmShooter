// Script attach to the object that will generate the bubbles for the rhythm game (see Bubble.cs, PlayerTile.cs)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnSpot : MonoBehaviour {

	public GameObject[] bubbles; // prefabs of the Bubbles
	public GameObject[] targets; // the target to reach (may be changed to an array of several targets)
	public float[] reachTimesByDifficulty = new float[4]{1f,1f,1f,1f}; // Times for bubbles to reach their target

	private float stageDuration; // length of the stage
	//private bool isInitialized = false;
	private int difficulty = 1; // difficulty (will varies in function of used items
	private float currentAudioTime = 0;
	private int numberLoops = 0;
	private Queue<BeatInformation> beats; // Contains the beats pattern of the stage

	private struct BeatInformation{
		public BeatInformation(float time, int type, int target, int diffMin, int diffMax){
			this.time = time;
			this.type = type;
			this.target = target;
			this.diffMin = diffMin;
			this.diffMax = diffMax;
		}
		public float time;
		public int type;
		public int target;
		public int diffMin;
		public int diffMax;
	}

	void Start(){
		stageDuration = GameValuesContainer.container.stageDuration;
		fillBeats ();
	}

	void fillBeats(){ // Defines the times, types, tragets and condition of apparition of each beat
		beats = new Queue<BeatInformation> ();
		float[] beatsTimes = GameValuesContainer.container.currentStage.BeatsTimes();
		int[] beatsTypes = GameValuesContainer.container.currentStage.BeatsTypes();
		int[] beatsTargets = GameValuesContainer.container.currentStage.BeatsTargets();
		int[] beatsDiffMin = GameValuesContainer.container.currentStage.BeatsDifficultiesMin();
		int[] beatsDiffMax = GameValuesContainer.container.currentStage.BeatsDifficultiesMax();
		float time = 0;
		int loop = -1;
		int i = 0;
		while (time < stageDuration) {
			if (i == 0) {
				loop += 1;
			}
			time = beatsTimes [i] + loop * GameValuesContainer.container.audioHandler.stageMusic.length;
			BeatInformation info = new BeatInformation (time, beatsTypes [i], beatsTargets [i], beatsDiffMin [i], beatsDiffMax [i]);
			beats.Enqueue (info);
			i = (int)Mathf.Repeat (i + 1, beatsTimes.Length);
		}

	}

	public float GetTime(){
		UpdateCurrentAudioTime ();
		return currentAudioTime + numberLoops * GameValuesContainer.container.audioHandler.stageMusic.length;
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

	protected void Spawn(GameObject target, float reachTime, int bubbleIndex=0){ // Spawn a bubble
		if (reachTime > 0.90*reachTimesByDifficulty [difficulty - 1]) {
			GameObject go = (GameObject)Instantiate (bubbles[bubbleIndex], this.transform.position, Quaternion.identity);
			go.transform.SetParent (GameValuesContainer.container.bubblesContainer.transform);
			go.GetComponent<Bubble> ().Initialize (target, reachTime);
		}
	}

	void UpdateCurrentAudioTime(){
		if (GameValuesContainer.container.audioHandler.isPlayingRhythmGame()) {
			if (GameValuesContainer.container.audioHandler.GetComponent<AudioSource> ().time < currentAudioTime) {
				numberLoops++;
			}
			currentAudioTime = GameValuesContainer.container.audioHandler.GetComponent<AudioSource> ().time;
		}
	}

	// Update is called once per frame
	void Update () {
		if (GameValuesContainer.container.audioHandler.isPlayingRhythmGame()) {
			UpdateCurrentAudioTime ();
			////////////////////////////////////////////////
			////// Spawning in function of difficulty //////
			////////////////////////////////////////////////
			SpawnInRhythm();
		}
	}
		
	void SpawnInRhythm(){
		float realTime = currentAudioTime + numberLoops * GameValuesContainer.container.audioHandler.stageMusic.length;
		while(beats.Count>0 && beats.Peek().time<realTime+reachTimesByDifficulty[difficulty-1]){
			BeatInformation info = beats.Dequeue ();
			if (info.diffMin <= difficulty && info.diffMax >= difficulty) {
				Spawn (targets [info.target], info.time - realTime, info.type);
			}
		}
	}
}