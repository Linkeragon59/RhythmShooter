// Attached to the Audio Handler in the game scenes
// Allow to change the music of the game in function of the state of the game (boss battle...)

using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

	public AudioClip stageMusic;
	public AudioClip bossMusic;
	public AudioClip warning;
	public AudioClip victory;

	private bool playRhythmGame = false;

	void Awake(){
		if (GameValuesContainer.container == null) {
			GameValuesContainer.container = new GameValuesContainer ();
		}
		GameValuesContainer.container.audioHandler = this;
	}

	public bool isPlayingRhythmGame(){
		return playRhythmGame;
	}

	public void Play(){
		if(!GetComponent<AudioSource> ().isPlaying){
			GetComponent<AudioSource> ().Play ();
		}
	}

	public void Pause(){
		if (GetComponent<AudioSource> ().isPlaying) {
			GetComponent<AudioSource> ().Pause ();
		}
	}

	public void Stop(){
		if (GetComponent<AudioSource> ().isPlaying) {
			GetComponent<AudioSource> ().Stop ();
		}
	}

	public void PlayStageMusic(){
		Pause ();
		GetComponent<AudioSource> ().clip = stageMusic;
		playRhythmGame = true;
		Play ();
	}

	public void PlayBossMusic(){
		Pause ();
		GetComponent<AudioSource> ().clip = bossMusic;
		playRhythmGame = false;
		Play ();
	}

	public void PlayWarning(){
		Pause ();
		GetComponent<AudioSource> ().clip = warning;
		playRhythmGame = false;
		Play ();
	}

	public float PlayVictory(){
		Stop ();
		GetComponent<AudioSource> ().PlayOneShot (victory);
		playRhythmGame = false;
		return victory.length;
	}
}
