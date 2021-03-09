// Used to display the 'multiplier' which is one of the results displayed at the end of the game

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplierScorePanel : MonoBehaviour {

	private Text multiplier;

	private int multiplierValue = 0;

	private IEnumerator coroutineMultiplier;

	void Awake () {
		multiplier = GetComponentInChildren<Text> ();
	}

	void Start(){
		int[] rhythmScores = GameValuesContainer.container.rhythmScores;
		int[] coef = new int[6]{ 0, 1, 2, 3, 4, 5 };
		multiplierValue = 0;
		for (int i = 0; i < 6; i++) {
			multiplierValue += rhythmScores [i] * coef [i];
		}
	}

	void Update(){
		if (Input.GetKeyDown("return")) {
			if (coroutineMultiplier != null) {
				StopCoroutine (coroutineMultiplier);
				coroutineMultiplier = null;
				multiplier.text = "x " + multiplierValue;
				GameValuesContainer.container.menuWrapper.GetComponentInChildren<ResultsCanvasInGame> ().OnMultiplierDisplayed ();
			}
		}
	}

	public int GetMultiplierValue(){
		return multiplierValue;
	}

	public void DisplayMultiplier(){
		multiplier.text = "x 0";
		if (coroutineMultiplier != null) {
			StopCoroutine (coroutineMultiplier);
		}
		coroutineMultiplier = CoroutineDisplayMultiplier ();
		StartCoroutine (coroutineMultiplier);
	}

	IEnumerator CoroutineDisplayMultiplier(){
		if (multiplierValue > 0) {
			float buffer = 0;
			while (buffer < multiplierValue) {
				buffer = Mathf.Min (buffer + (float)multiplierValue / 50f, multiplierValue);
				if (multiplier != null) {
					multiplier.text = "x " + (int)buffer;
				}
				yield return new WaitForSeconds(0.02f);
			}
		}
		coroutineMultiplier = null;
		GameValuesContainer.container.menuWrapper.GetComponentInChildren<ResultsCanvasInGame> ().OnMultiplierDisplayed ();
	}
}
