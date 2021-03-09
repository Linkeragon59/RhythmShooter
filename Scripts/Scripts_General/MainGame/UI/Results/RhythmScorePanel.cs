using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RhythmScorePanel : MonoBehaviour {

	private RhythmGraph graph;
	private RhythmLabel label;
	private IEnumerator coroutine = null;

	private ResultsCanvas parentCanvas;

	// Use this for initialization
	void Awake () {
		graph = GetComponentInChildren<RhythmGraph> ();
		label = GetComponentInChildren<RhythmLabel> ();
		parentCanvas = transform.parent.gameObject.GetComponent<ResultsCanvas> ();
		UpdateLayout ();
	}

	void Update(){
		if (Input.GetKeyDown("return")) {
			if (coroutine != null) {
				StopCoroutine (coroutine);
				coroutine = null;
				int[] scores = GameValuesContainer.container.rhythmScores;
				for (int j = 0; j < 6; j++) {
					graph.SetScore(j,(float)scores[j]);
				}
				graph.DisplayResults (MaxScore(scores));
				parentCanvas.OnGraphDisplayed ();
			}
		}
	}

	void OnRectTransformDimensionsChange()
	{
		UpdateLayout ();
	}

	public void SetScores(float duration = 1f){
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		coroutine = CoroutineSetScores (GameValuesContainer.container.rhythmScores,duration);
		StartCoroutine (coroutine);
	}

	public void SetScores(int[] scores,float duration = 1f){
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		coroutine = CoroutineSetScores (scores,duration);
		StartCoroutine (coroutine);
	}

	IEnumerator CoroutineSetScores(int[] scores, float duration){
		float max = MaxScore (scores);
		if (scores.Length == 6 && max > 0) {
			float f = 0;
			while (f < max) {
				f = Mathf.Min (f + max / (duration*100f), max);
				for (int j = 0; j < 6; j++) {
					graph.SetScore (j, Mathf.Min ((float)scores [j], f));
				}
				graph.DisplayResults (max);
				yield return new WaitForSeconds(0.01f);
			}
		}
		else if (scores.Length == 6 && max == 0) {
			for (int j = 0; j < 6; j++) {
				graph.SetScore (j, 0f);
			}
			graph.DisplayResults (1f);
		}
		coroutine = null;
		parentCanvas.OnGraphDisplayed ();
	}

	float MaxScore(int[] array){
		float max = 0;
		foreach(int num in array){
			max = Mathf.Max (max, num);
		}
		return max;
	}

	void UpdateLayout(){
		Vector2 sizePanel = GetComponent<RectTransform> ().sizeDelta;
		Vector2 pivot = GetComponent<RectTransform> ().pivot - new Vector2 (0.5f, 0.5f);
		if (graph != null && label != null) {
			graph.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (sizePanel.x, sizePanel.y * 0.8f);
			graph.gameObject.GetComponent<RectTransform> ().localPosition = new Vector3 (-sizePanel.x * pivot.x, sizePanel.y * 0.1f - sizePanel.y * pivot.y, 0);
			label.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (sizePanel.x, sizePanel.y * 0.2f);
			label.gameObject.GetComponent<RectTransform> ().localPosition = new Vector3 (-sizePanel.x * pivot.x, -sizePanel.y * 0.4f - sizePanel.y * pivot.y, 0);
		}
	}
}
