// used to display the score of the shooting game (depending on the number of killed enemies)

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShooterScorePanel : MonoBehaviour {

	public Text shootingScore;
	public Text bestShootingScore;
	public Text newBestScore;
	private IEnumerator coroutine;

	private int currentScore = 0;
	private int bestScore = 0;
	private bool multiplied = false;

	void Start(){
		newBestScore.gameObject.SetActive (false);
		string sceneName = SceneManager.GetActiveScene ().name;
		if (Game.current.bestScore.ContainsKey (sceneName)) {
			bestScore = Game.current.bestScore [sceneName];
		}
		SetScoreText (bestShootingScore, bestScore);
	}

	void Update(){
		if (Input.GetKeyDown("return")) {
			if (coroutine != null) {
				StopCoroutine (coroutine);
				coroutine = null;
				SetScoreText (shootingScore,currentScore);
				if (multiplied) {
					if (currentScore > bestScore) {
						SetScoreText (bestShootingScore,currentScore);
					}
					GameValuesContainer.container.menuWrapper.GetComponentInChildren<ResultsCanvasInGame> ().OnResultDisplayed (CheckNewBestScore ());
				}
				else {
					GameValuesContainer.container.menuWrapper.GetComponentInChildren<ResultsCanvasInGame> ().OnScoreDisplayed ();
				}
			}
		}
	}

	public void SetScore(){
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		coroutine = CoroutineSetScore (GameValuesContainer.container.shootingScore,multiplied);
		StartCoroutine (coroutine);
	}

	public void SetScore(int score){
		multiplied = true;
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		coroutine = CoroutineSetScore (score,multiplied);
		StartCoroutine (coroutine);
	}
	
	IEnumerator CoroutineSetScore(int score,bool multiplied){
		if (shootingScore != null) {
			int buffer = currentScore;
			currentScore = score;
			SetScoreText (shootingScore,buffer);
			if(score>0){
				while (buffer < score) {
					buffer = (int)Mathf.Min ((int)(buffer + score/100f), score);
					SetScoreText (shootingScore,buffer);
					if (multiplied && buffer > bestScore) {
						SetScoreText (bestShootingScore,buffer);
					}
					yield return new WaitForSeconds(0.01f);
				}
			}
		}
		coroutine = null;
		if (multiplied) {
			GameValuesContainer.container.menuWrapper.GetComponentInChildren<ResultsCanvasInGame> ().OnResultDisplayed (CheckNewBestScore ());
		}
		else {
			GameValuesContainer.container.menuWrapper.GetComponentInChildren<ResultsCanvasInGame> ().OnScoreDisplayed ();
		}
	}

	bool CheckNewBestScore(){
		newBestScore.gameObject.SetActive (currentScore > bestScore);
		return currentScore > bestScore;
	}

	public void SetScoreText(Text textObject, int points){
		int buff = points;
		int unit;
		string score = "";
		for (int i = 0; i < 9; i++) {
			unit = buff / (int)Mathf.Pow (10, 8 - i);
			buff = buff - unit * (int)Mathf.Pow (10, 8 - i);
			score += ""+unit;
			if (i==2 || i==5) {
				score += " ";
			}
		}
		textObject.text = score;
	}
}
