// Is used to display a graph of the result of the rhythm game

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RhythmGraph : MonoBehaviour {

	private float[] scores = new float[6]{0,0,0,0,0,0};

	void Awake(){
		DisplayResults (MaxScore(scores));
	}

	void OnRectTransformDimensionsChange()
	{
		DisplayResults (MaxScore(scores));
	}

	public void SetScore(int index,float value){
		this.scores [index] = value;
	}

	// Value used to calibrate the results
	float MaxScore(float[] array){
		float max = 0;
		foreach(float num in array){
			max = Mathf.Max (max, num);
		}
		return max;
	}

	public void DisplayResults(float scale){
		int i = 0;
		Vector2 sizePanel = GetComponent<RectTransform> ().sizeDelta;
		foreach (RectTransform child in transform) {
			if (scores[i] > 0) {
				child.sizeDelta = new Vector2 (sizePanel.x/6, sizePanel.y * scores [i] / scale);
				child.localPosition = new Vector3 ((sizePanel.x/6)*(i-2.5f), (sizePanel.y/2)*(scores [i]/scale-1), 0);
			}
			else {
				child.sizeDelta = new Vector2 (sizePanel.x/6, 0);
				child.localPosition = new Vector3 (0, -sizePanel.y/2, 0);
			}
			i++;
		}
	}
}
