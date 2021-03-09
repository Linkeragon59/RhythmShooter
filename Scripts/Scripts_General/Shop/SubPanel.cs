// attached to the panel that correspond to each menu items in the shop

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SubPanel : MonoBehaviour {

	public string nameSubPanel = "";
	public Text displayedText;
	public GameObject checkedBox;
	public GameObject uncheckedBox;

	public Color selectedColor = Color.gray;
	public Color nonSelectedColor = Color.white;
	private int selectedFontSize;
	private int nonSelectedFontSize;

	// Set the content of the panel (a name and eventually a checkbox, checked or not, to indicate if the object corresponding to the menu tem is equiped
	public void UpdateContent(string nameChoice, string displayedText, bool hasCheckbox, bool isChecked){
		nameSubPanel = nameChoice;
		this.displayedText.text = displayedText;
		Rect rect = GetComponent<RectTransform> ().rect;
		float ratioText = 1f;
		if (hasCheckbox) {
			if (isChecked) {
				checkedBox.SetActive (true);
			}
			else {
				uncheckedBox.SetActive (true);
			}
			ratioText = 0.85f;
		}
		this.displayedText.transform.localScale = Vector3.one;
		this.displayedText.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width*ratioText, this.displayedText.GetComponent<RectTransform> ().sizeDelta.y);
		nonSelectedFontSize = this.displayedText.fontSize;
		selectedFontSize = (int)((float)this.displayedText.fontSize*1.2f);
	}

	public void Highlight(bool b){ // Used to highlight the menu item that is currently selected by changing the font size and background color
		if (b) {
			GetComponent<Image>().color = selectedColor;
			displayedText.fontSize = selectedFontSize;
		}
		else {
			GetComponent<Image>().color = nonSelectedColor;
			displayedText.fontSize = nonSelectedFontSize;
		}
	}
}
