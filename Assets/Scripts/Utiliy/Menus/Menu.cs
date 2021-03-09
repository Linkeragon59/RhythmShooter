// abstract class used to defined some functions that are used by the menus in all the game (Main title, pause menu...)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public MenuChoice[] choices;
	public Color selectedColor;
	public Color NonSelectedColor;
	public AudioClip choiceChange;
	public AudioClip choiceChosen;
	private int selected;
	private AudioSource audioSource;

	delegate void FooDelegate(MenuChoice choice);

	// Use this for initialization
	void Start () {
		HighlightChoice ();
		audioSource = GetComponent<AudioSource> ();
		OnStart ();
	}

	void OnStart (){
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("down") || Input.GetKeyDown("right")) { // when the player press down or right
			ChangeChoice(1); // go down in the menu
		}
		else if (Input.GetKeyDown("up") || Input.GetKeyDown("left")) { // when the player press up or left
			ChangeChoice(-1); // go up in the menu
		}
		if (Input.GetKeyDown("return")) { // when the player press return/enter
			if (audioSource != null && choiceChosen!=null) {
				audioSource.PlayOneShot (choiceChosen);
				choices [selected].OnChosen (choiceChosen.length);
			}
			else{
				choices [selected].OnChosen (); // call the specific function attached to the selected menu item
			}
		}
	}

	void ChangeChoice(int direction){ // called from Update, change the selected item in the menu
		if (audioSource != null && choiceChange!=null) {
			audioSource.PlayOneShot (choiceChange);
		}
		selected = (int)Mathf.Repeat (selected + direction, choices.Length);
		HighlightChoice ();
	}

	void HighlightChoice(){ // highlight the selected menu item by changing the color of the displayed text
		for (int i = 0; i < choices.Length; i++) {
			if (i == selected) {
				choices [i].GetComponent<Text> ().color = selectedColor;
			}
			else {
				choices [i].GetComponent<Text> ().color = NonSelectedColor;
			}
		}
	}
}
