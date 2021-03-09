// attached to the menu panels where the menu items (type of items, names of items) are displayed
// necessary to adapt the size and position of the sub Panel objects

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChoicesPanel : MonoBehaviour {

	public SubPanel subPanel;

	private List<SubPanel> listChoices = new List<SubPanel> (); // list of choices (menu items)
	private int size = 10; // max number of items displayed
	private int selectedChoice = 0; // currently selected item
	private int firstDisplayed = 0; // index of the item displayed in the first position of the menu (may b bigger than 0 if there are more than size possible choices)
	private bool hasFocus = false; // is focus on this menu?

	// Create a sub panel containing on possible choice (menu item), see SubPanel.cs
	public void CreateChoice(string nameChoice, string displayedText, bool hasCheckbox, bool isChecked){
		RectTransform transform = GetComponent<RectTransform> ();
		SubPanel sub = (SubPanel)Instantiate(subPanel);
		sub.transform.SetParent (transform);
		Vector2 sizePanel = new Vector2(transform.rect.width,transform.rect.height/size);
		sub.GetComponent<RectTransform> ().sizeDelta = sizePanel;
		sub.GetComponent<RectTransform> ().localScale = Vector3.one;
		sub.GetComponent<RectTransform> ().localRotation = Quaternion.Euler(Vector3.zero);
		sub.UpdateContent (nameChoice, displayedText, hasCheckbox, isChecked);
		listChoices.Add (sub);
	}


	//////////////////////////////
	/// Used by ListOfItems.cs ///
	//////////////////////////////

	public bool HasChoices(){ // Determines whether this instance has choices.
		return (listChoices.Count > 0);
	}

	public void SetSize(int size){ // Sets the size.
		this.size = size;
	}

	public string GetSelectedChoice(){ // return name of the currently selected choice
		return listChoices[selectedChoice].nameSubPanel;
	}

	public int GetIndexSelectedChoice(){ // return index of the currently selected choice
		return selectedChoice;
	}

	public void SetFocus(bool b){ // set or unset the focus and update highlighting
		hasFocus = b;
		HighlightChoice ();
	}

	public string ChangeChoice(int direction){ // Change the selected menu item when the user press up or down
												// called from ListOfItems.cs
		if (listChoices!=null && listChoices.Count>0) {
			selectedChoice = (int)Mathf.Repeat (selectedChoice + direction, listChoices.Count);
			if(selectedChoice>size-1){
				firstDisplayed = selectedChoice - size + 1;
				Display (firstDisplayed);
			}
			else if(selectedChoice<firstDisplayed){
				firstDisplayed = selectedChoice;
				Display (firstDisplayed);
			}
			HighlightChoice ();
			return GetSelectedChoice();
		}
		return "";
	}

	void HighlightChoice(){ // Highlight the choice by sending the information to the subPanel
		for (int i=0; i<listChoices.Count; i++){
			if (i == selectedChoice && hasFocus) {
				listChoices [i].Highlight (true);
			}
			else {
				listChoices [i].Highlight (false);
			}
		}
	}

	public void Display(int begining){ // Displays (and position) up to size items from the index begining
		foreach (Transform child in this.transform) {
			child.gameObject.SetActive (false);
		}
		float height = GetComponent<RectTransform> ().rect.height;
		float step = height/size;
		Vector2 offset = new Vector2(GetComponent<RectTransform> ().rect.center.x,GetComponent<RectTransform> ().rect.center.y);
		for (int i=begining; i<Mathf.Min(begining+size,listChoices.Count); i++){
			listChoices [i].gameObject.SetActive (true);
			Vector2 positionPanel = new Vector2 (0, (height-step) / 2 - (i-begining)*step) + offset;
			listChoices [i].GetComponent<RectTransform> ().localPosition = positionPanel;
		}
		HighlightChoice ();
	}

	public void ResetSelection(){ // reset the position of the user in the menu
		selectedChoice = 0;
		firstDisplayed = 0;
	}

	public void Clear(){ // Empty the menu
		foreach (Transform child in this.transform) {
			Destroy(child.gameObject);
		}
		listChoices = new List<SubPanel> ();
	}
}
