// Used to display the recorded results of the currently selected stage in the stage selection menu

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DescriptionCanvas : ResultsCanvas {

	public Text bestComboText;
	public Text bestScoreText;
	public Text nameText;

	private RhythmScorePanel rhythmPanel;
	private Grade gradeImage;

	private string stageName = "";

	void Awake(){
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = calculationClip;
		rhythmPanel = GetComponentInChildren<RhythmScorePanel> ();
		gradeImage = GetComponentInChildren<Grade> ();
		Display (false);
	}

	public void Display(bool active, string stageName = ""){
		audioSource.Stop ();
		nameText.text = stageName;
		nameText.transform.parent.gameObject.SetActive (active);
		if (stageName != "Title") {
			bestComboText.transform.parent.gameObject.SetActive (active);
			bestScoreText.transform.parent.gameObject.SetActive (active);
			rhythmPanel.gameObject.SetActive (active);
			gradeImage.gameObject.SetActive (active);
			if (active) {
				this.stageName = stageName;
				StartDisplaying ();
			}
		}
	}

	override public void StartDisplaying(){
		if (Game.current.bestScore.ContainsKey (stageName)) {
			SetScoreText (bestScoreText, Game.current.bestScore [stageName]);
			bestComboText.text = "Best combo : "+Game.current.bestCombo [stageName];
			gradeImage.SetGrade (Game.current.bestGrade [stageName]);
		}
		else {
			SetScoreText (bestScoreText, 0);
			bestComboText.text = "Best combo : 0";
			gradeImage.SetGrade (-1);
		}
		DisplayGraph ();
	}

	override protected void DisplayGraph(){
		audioSource.Play ();
		if (Game.current.bestRhythmScores.ContainsKey (stageName)) {
			rhythmPanel.SetScores (Game.current.bestRhythmScores [stageName],0.1f);
		}
		else {
			
			rhythmPanel.SetScores (new int[6] {0, 0, 0, 0, 0, 0});
		}
	}

	override public void OnGraphDisplayed(){
		audioSource.Stop ();
		audioSource.PlayOneShot (endCalculationClip);
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
