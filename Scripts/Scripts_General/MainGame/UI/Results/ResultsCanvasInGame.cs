// Attached to the canvas where the results are displayed at the end of the game

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultsCanvasInGame : ResultsCanvas {

	private ShooterScorePanel score;
	private RhythmScorePanel graph;
	private MultiplierScorePanel multiplier;
	private Grade gradeImage;

	private bool isDisplayedScore = false;
	private bool isDisplayedGraph = false;

	private int maxCombo;
	private int finalScore;
	private int[] rhythmScores;
	private int grade;

	private bool isFinished = false;

	delegate void FooDelegate();

	void Awake(){
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = calculationClip;
		audioSource.loop = true;
		score = GetComponentInChildren<ShooterScorePanel>();
		graph = GetComponentInChildren<RhythmScorePanel>();
		multiplier = GetComponentInChildren<MultiplierScorePanel>();
		gradeImage = GetComponentInChildren<Grade> ();
		gradeImage.gameObject.SetActive (false);
	}

	void Update(){
		if (Input.GetKeyDown("return") && isFinished) {
			SceneManager.LoadSceneAsync ("StageSelection");
		}
	}

	override public void StartDisplaying(){
		audioSource.Play ();
		DisplayScore ();
		DisplayGraph ();
	}

	void DisplayScore(){
		if (score != null) {
			score.SetScore ();
		}
	}

	public void OnScoreDisplayed(){
		isDisplayedScore = true;
		OnScoreAndGraphDisplayed ();
	}

	override protected void DisplayGraph(){
		if (graph != null) {
			graph.SetScores ();
		}
	}

	override public void OnGraphDisplayed(){
		isDisplayedGraph = true;
		OnScoreAndGraphDisplayed ();
	}

	void OnScoreAndGraphDisplayed(){
		if (isDisplayedGraph && isDisplayedScore) {
			audioSource.Stop ();
			audioSource.PlayOneShot (endCalculationClip);
			StartCoroutine(Delay(DisplayMultiplier,endCalculationClip.length+0.1f));
		}
	}

	void DisplayMultiplier(){
		audioSource.Play ();
		multiplier.DisplayMultiplier ();
	}

	public void OnMultiplierDisplayed(){
		audioSource.Stop ();
		audioSource.PlayOneShot (endCalculationClip);
		StartCoroutine(Delay(DisplayResult,endCalculationClip.length+0.1f));
	}

	void DisplayResult(){
		audioSource.Play ();
		score.SetScore(GameValuesContainer.container.shootingScore * multiplier.GetMultiplierValue());
	}

	public void OnResultDisplayed(bool newBest){
		audioSource.Stop ();
		audioSource.PlayOneShot (endCalculationClip);
		SaveResults ();
		StartCoroutine(Delay (SetFinished,endCalculationClip.length+0.5f));
	}

	void SetFinished(){
		gradeImage.SetGrade (grade);
		gradeImage.gameObject.SetActive (true);
		isFinished = true;
	}

	void SaveResults(){
		maxCombo = GameValuesContainer.container.maxCombo;
		rhythmScores = GameValuesContainer.container.rhythmScores;
		finalScore = GameValuesContainer.container.shootingScore * multiplier.GetMultiplierValue();
		grade = CalculateGrade();
		string sceneName = SceneManager.GetActiveScene ().name;
		if (!Game.current.bestScore.ContainsKey(sceneName) || finalScore > Game.current.bestScore[sceneName]) {
			Game.current.bestScore [sceneName] = finalScore;
			Game.current.bestCombo [sceneName] = maxCombo;
			Game.current.bestRhythmScores [sceneName] = rhythmScores;
			Game.current.bestGrade [sceneName] = grade;
		}
		SaveLoad.Save ();
	}

	int CalculateGrade(){
		float shootingPrecision = (float)GameValuesContainer.container.killedEnemies / (float)GameValuesContainer.container.spawnedEnemies;
		int sum = 0;
		for (int i = 0; i < rhythmScores.Length; i++) {
			sum += rhythmScores [i];
		}
		float rhythmPrecision = 0f;
		for (int i = 0; i < rhythmScores.Length; i++) {
			rhythmPrecision += ((float)i/5f) * ((float)rhythmScores[i]/(float)sum);
		}
		float precision = Mathf.Sqrt(shootingPrecision * rhythmPrecision);
		int grade = 0;
		if (precision >= 1) {
			grade = 6;
		}
		else if(precision>0.95){
			grade = 5;
		}
		else if(precision>0.9){
			grade = 4;
		}
		else if(precision>0.8){
			grade = 3;
		}
		else if(precision>0.7){
			grade = 2;
		}
		else if(precision>0.5){
			grade = 1;
		}
		return grade;
	}

	IEnumerator Delay(FooDelegate function, float delay){
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + delay) {
			yield return null;
		}
		function ();
	}
}
