// attached to the panel that contains the characteristics of items
// necessary to position and size correctly the objects inside the panel
// used by CharacteristicsPanel.cs

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Characteristic : MonoBehaviour {

	public Text displayedText;
	public GameObject bar;

	void Start(){
		Rect rect = GetComponent<RectTransform> ().rect;
		displayedText.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width*0.5f, displayedText.GetComponent<RectTransform> ().sizeDelta.y);
	}

	public void UpdateContent(string displayedText, int value, int maxValue, bool reverse){
		bar.GetComponent<Slider> ().maxValue = maxValue;
		if (value == -1) {
			this.displayedText.text = displayedText+"inf";
			bar.GetComponent<Slider> ().value = maxValue;
		}
		else {
			this.displayedText.text = displayedText+value;
			if (reverse) {
				bar.GetComponent<Slider> ().value = maxValue - value;
			}
			else {
				bar.GetComponent<Slider> ().value = value;
			}
		}
	}
}
