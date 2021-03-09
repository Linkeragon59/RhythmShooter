// Hanle the events in the stage selection menu (player selecting stage, playing music of the selected stage, displaying recorded results...)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelectionHandler : MonoBehaviour {

	public GameObject stagesSelectionObject;
	public DescriptionCanvas descriptionCanvas;
	public float displayRatioDepth = 10;
	public float displayRatioWidth = 15;

	public AudioClip choiceChange;
	public AudioClip choiceSelect;

	private StageSelectionItem[] stages;
	private int selected;
	private float selectionLocation;
	private IEnumerator coroutine;

	private AudioSource audioSource;

	void Awake(){
		Game.current = new Game (); // new Game if the load doesn't work
		SaveLoad.Load (); // loads the game save that contains the items that have to be attached to the player
		audioSource = GetComponent<AudioSource> ();
		GameValuesContainer.container = new GameValuesContainer ();
		Time.timeScale = 1.0f;
	}

	void Start(){
		List<StageSelectionItem> listStages = new List<StageSelectionItem> ();
		foreach (Transform child in stagesSelectionObject.transform) {
			if (child.gameObject.GetComponent<StageSelectionItem> () != null) {
				listStages.Add (child.gameObject.GetComponent<StageSelectionItem> ());
			}
		}
		stages = listStages.ToArray ();
		selected = 0;
		selectionLocation = 0;
		Positioning (0);
		stages [0].SetActivation (true);
		descriptionCanvas.Display (true,stages[0].stageName);
	}

	void Positioning(float location){
		int stagesNumber = stages.Length;
		Vector3 position;
		for (int i = 0; i < stagesNumber; i++) {
			position = new Vector3 (displayRatioWidth*Mathf.Cos(2*(i-location)*Mathf.PI/stagesNumber-Mathf.PI/2),0,displayRatioDepth*Mathf.Sin(2*(i-location)*Mathf.PI/stagesNumber-Mathf.PI/2));
			stages [i].transform.localPosition = position;
		}
	}

	void Update(){
		if (Input.GetKeyDown("right")) { // when the player press down or right
			ChangeChoice(1); // go down in the menu
		}
		else if (Input.GetKeyDown("left")) { // when the player press up or left
			ChangeChoice(-1); // go up in the menu
		}
		if (Input.GetKeyDown("return")) { // when the player press return/enter
			if (audioSource != null && choiceChange != null) {
				audioSource.PlayOneShot (choiceSelect);
				stages [selected].OnChosen (choiceSelect.length);
			}
			else {
				stages [selected].OnChosen ();
			}
		}
	}

	void ChangeChoice(int direction){ // called from Update, change the selected item in the menu
		if(audioSource!=null && choiceChange!=null){
			audioSource.PlayOneShot (choiceChange);
		}
		selected = (int)Mathf.Repeat (selected + direction, stages.Length);
		bool clockWise = selected>selectionLocation;
		float distance = Mathf.Abs (selected - selectionLocation);
		if ((selectionLocation < selected && selectionLocation-(selected-stages.Length)<selected-selectionLocation) || (selectionLocation>selected && selected-(selectionLocation-stages.Length)<selectionLocation-selected)) {
			clockWise = !clockWise;
			distance = stages.Length - distance;
		}
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		coroutine = Shift(clockWise,distance);
		StartCoroutine (coroutine);
		selectionLocation = selected;
	}

	IEnumerator Shift(bool clockWise,float distance) {
		descriptionCanvas.Display (false);
		for (int i = 0; i < stages.Length; i++) {
			stages [i].SetActivation (false);
		}
		float location = 0;
		float buffer = selectionLocation;
		while (location!=distance) {
			location = Mathf.Min(location+0.1f*distance,distance);
			if (clockWise) {
				selectionLocation = Mathf.Repeat (buffer+location, stages.Length);
			}
			else {
				selectionLocation = Mathf.Repeat (buffer-location, stages.Length);
			}
			Positioning (selectionLocation);
			yield return new WaitForSeconds(0.01f);
		}
		string stageName = "";
		for (int i = 0; i < stages.Length; i++) {
			if (i == selected) {
				stages [i].SetActivation (true);
				stageName = stages [i].stageName;
			}
			else {
				stages [i].SetActivation (false);
			}
		}
		descriptionCanvas.Display (true,stageName);
		coroutine = null;
	}
		
}
