// script attached to the PlayerTiles, targets of the rhythm game.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerTile : MonoBehaviour {

	public string[] keys; // key to press when a bubble is on the target

	private Queue<Bubble> bubbles; // contains the bubble that are currently over the target (there can be several at the same time)
	private Animator animator;

	// Use this for initialization
	void Start () {
		bubbles = new Queue<Bubble> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () { // used to detect when the player press the key corresponding to the target
		bool pressed = false;
		for (int i = 0; i < keys.Length; i++) {
			if(Input.GetKeyDown (keys[i])){
				pressed = true;
			}
		}
		if (Time.timeScale>0 && pressed) {
			OnPressed ();
		}
	}

	void OnPressed(){
		animator.SetTrigger ("Press");
		if (bubbles.Count>0){
			Bubble bubble = bubbles.Dequeue(); // associate the press event with the first bubble that entered the target (and that is still over it)
			if (bubble != null) {
				float currentAudioTime = GameValuesContainer.container.rhythmHandler.spawnSpot.GetTime ();
				float score = Mathf.Abs (currentAudioTime - bubble.GetBeatTime ());
				if (score < 0.02) {
					SendScore (5); // Fantastic precision
				} else if (score < 0.04) {
					SendScore (4); // Great precision
				} else if (score < 0.1) {
					SendScore (3); // Good precision
				} else if (score < 0.15) {
					SendScore (2); // Almost precision
				} else if (score < 0.2) {
					SendScore (1); // Bad precision
				} else {
					SendScore (0); // Miss
				}
				Destroy (bubble.gameObject); // remove the physical dequeued bubble for the game
			}
			else {
				OnPressed ();
			}
		}
	}

	void OnTriggerEnter(Collider other) { // add the bubble to the queue
		if (other.gameObject.GetComponent<Bubble>()!=null) {
			bubbles.Enqueue(other.gameObject.GetComponent<Bubble>());
		}
	}

	void OnTriggerExit(Collider other) { // occurs when the player has not press the key in time
		if (other.gameObject.GetComponent<Bubble>()!=null) {
			SendScore (0); // Miss
			bubbles.Dequeue();
		}
	}

	void SendScore(int score){ // send the score to the events handler that will update the combo,... see GameEventsHandler.cs
		GameValuesContainer.container.rhythmHandler.UpdateRhythmScore (score);
	}
}
