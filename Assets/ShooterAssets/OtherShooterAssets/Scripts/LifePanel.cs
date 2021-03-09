// Attached to the panel that displays the player's lives

using UnityEngine;
using System.Collections;

public class LifePanel : MonoBehaviour {

	public GameObject[] lifeSprites;
	//public Text scoreText;

	void Start(){
		SetLives (GameValuesContainer.container.lives);
	}

	public bool SetLives(int lives=3){
		GameValuesContainer.container.lives = Mathf.Max(0,Mathf.Min(4,lives));
		for (int i = 0; i < lifeSprites.Length; i++) {
			if (i >= GameValuesContainer.container.lives) {
				lifeSprites [i].gameObject.SetActive (false);
			}
			else {
				lifeSprites [i].gameObject.SetActive (true);
			}
		}
		return (GameValuesContainer.container.lives == lives);
	}

	public bool LivesUp(){
		return SetLives (GameValuesContainer.container.lives + 1);
	}

	public bool LivesDown(){
		return SetLives (GameValuesContainer.container.lives - 1);
	}
}
