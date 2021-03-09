// Attached to the HP bar of the boss

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBarCircle : MonoBehaviour {

	private float max = 1;
	private Image image;

	void Awake(){
		image = GetComponent<Image>();
	}

	public void SetMaximum(float max){
		this.max = max;
	}

	public void SetValue(float value){
		image.fillAmount = value / max;
	}
}
