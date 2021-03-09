// attached to the panel where the description of items is displayed
// necessary to adapt the size and position of the Text object containing the description

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DescriptionPanel : MonoBehaviour {

	public Text displayedText;

	public void SetDescription(string description){ // Position and size the Text object at the center of the panel
		RectTransform transform = GetComponent<RectTransform> ();
		Vector2 offset = new Vector2(transform.rect.center.x,transform.rect.center.y);
		Text sub = (Text)Instantiate(displayedText);
		sub.transform.SetParent (transform);
		Vector2 sizeSub = new Vector2(transform.rect.width*0.9f,transform.rect.height*0.9f);
		sub.GetComponent<RectTransform> ().sizeDelta = sizeSub;
		sub.GetComponent<RectTransform> ().localScale = Vector3.one;
		sub.GetComponent<RectTransform> ().localRotation = Quaternion.Euler(Vector3.zero);
		Vector2 positionPanel = offset;
		sub.GetComponent<RectTransform> ().localPosition = positionPanel;
		sub.text = description;
	}

	public void Clear(){ // clear the panel
		foreach (Transform child in this.transform) {
			Destroy(child.gameObject);
		}
	}
}
