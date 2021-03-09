//Used to handle the events in the rhythm game

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RhythmEventsHandler : MonoBehaviour {

	public SpawnSpot spawnSpot;
	public Text[] appreciationsArray; // contains the different possible annotations
	public Text comboText;
	public GameObject rhythmEventsPanel; // the panel where the appreciations are displayed

	private Text currentAppreciation;
	private IEnumerator coroutine;

	void Awake(){
		if (GameValuesContainer.container == null) {
			GameValuesContainer.container = new GameValuesContainer ();
		}
		GameValuesContainer.container.rhythmHandler = this;
	}

	public void Initialize(){
		GameValuesContainer.container.audioHandler.PlayStageMusic ();
		DisplayCombo ();
	}

	public void Clear(){
		GameValuesContainer.container.movingObjectsContainers.ClearRhythmObjects ();
		GameValuesContainer.container.combo = 0;
		DisplayCombo ();
	}

	///////////////////////////////
	//      Updating Combo       //
	///////////////////////////////

	public void UpdateRhythmScore(int precision){
		DisplayAppreciation (precision);
		GameValuesContainer.container.rhythmScores [precision] += 1;
		if (precision < 3) {
			GameValuesContainer.container.combo = 0;
			gameObject.GetComponent<ShooterEventsHandler> ().playerShip.TakeRhythmDamage (1);
		}
		else {
			GameValuesContainer.container.combo += 1;
			gameObject.GetComponent<ShooterEventsHandler> ().playerShip.TakeRhythmDamage (0);
		}
		if (GameValuesContainer.container.combo > GameValuesContainer.container.maxCombo) {
			GameValuesContainer.container.maxCombo = GameValuesContainer.container.combo;
		}
		DisplayCombo ();
	}

	///////////////////////////////
	//     Displaying            //
	///////////////////////////////

	void DisplayCombo(){
		int combo = GameValuesContainer.container.combo;
		if (combo < 2) {
			comboText.text = "";
		}
		else {
			comboText.text = combo + " Combo";
		}
		comboText.color = Color.HSVToRGB(Mathf.Max(0f,(240f-2.4f*(float)combo)/360f),1f,1f); // From blue, get red when combo is high
	}
	
	void DisplayAppreciation(int appreciation){ // Displays an appreciation in the panel
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		coroutine = CoroutineDisplayAppreciation (appreciation);
		StartCoroutine(coroutine);
	}

	IEnumerator CoroutineDisplayAppreciation(int appreciation){
		if (currentAppreciation) {
			Destroy (currentAppreciation.gameObject); // Remove the appreciation to write the following
		}
		Quaternion rotation = Quaternion.identity;
		rotation.z += Random.Range(-0.25f,0.25f);
		RectTransform rectTransform = rhythmEventsPanel.GetComponent<RectTransform> ();
		Vector3 position = new Vector3 ((0.5f - rectTransform.pivot.x) * rectTransform.rect.width, (0.5f - rectTransform.pivot.y) * rectTransform.rect.height, 0);
		position.x += Random.Range (-20,20);
		position.z += Random.Range (-20,20);
		currentAppreciation = (Text)Instantiate (appreciationsArray [appreciation]);
		currentAppreciation.transform.SetParent (rhythmEventsPanel.transform);
		currentAppreciation.rectTransform.localScale = new Vector3 (1,1,1);
		currentAppreciation.rectTransform.localPosition = position;
		currentAppreciation.rectTransform.localRotation = rotation;


		yield return new WaitForSeconds (1.0f);
		Destroy (currentAppreciation.gameObject); // Remove the appreciation after 1 second
		currentAppreciation = null;
		coroutine = null;

	}
}
