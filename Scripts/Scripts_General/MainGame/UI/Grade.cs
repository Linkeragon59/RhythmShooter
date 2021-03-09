// Attached to the panel that will display the grade obtained by the player
// is called when the game ends (ResultCanvas.cs) or in the stage selection screen (StageSelectionHandler.cs)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Grade : MonoBehaviour {

	public Sprite[] gradeSprites;

	public void SetGrade(int grade){
		if (grade < 0) {
			GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		}
		else if (GetComponent<Image> () != null && gradeSprites.Length > 0) {
			Vector3 rotation = Vector3.zero;
			rotation.z = Random.Range (-25, 25);
			GetComponent<RectTransform> ().localRotation = Quaternion.Euler (rotation);
			GetComponent<Image> ().color = new Color (1, 1, 1, 1);
			GetComponent<Image> ().sprite = gradeSprites [Mathf.Max (0, Mathf.Min (gradeSprites.Length, grade))];
		}
	}
}
