// Used to make the HP bar of the boss folowing the boss object on the screen

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectLabel : MonoBehaviour {

	private Camera usedCamera;
	private Image image;
	private Transform target;
	private bool follow = false;

	void Start(){
		image = GetComponent<Image>();
		image.gameObject.SetActive (false);
		image.transform.SetParent (GameValuesContainer.container.labelsLayer.transform);
		image.rectTransform.localScale = Vector3.one;
		image.rectTransform.localRotation = Quaternion.Euler(Vector3.zero);
		usedCamera = GameValuesContainer.container.mainCamera.GetComponent<Camera>();
	}
	
	void Update(){
		UpdatePosition ();
	}

	public void SetTarget(Transform target){
		this.target = target;
	}

	public void SetFollow(bool follow){
		this.follow = follow;
		UpdatePosition ();
		image.gameObject.SetActive (follow);
	}

	void UpdatePosition(){
		if (follow) {
			Vector3 screenPos = target.transform.position - usedCamera.transform.position;
			float ratio = GameValuesContainer.container.labelsLayer.GetComponent<RectTransform> ().rect.height/(usedCamera.orthographicSize*2);
			image.rectTransform.localPosition = new Vector3 (-screenPos.z, screenPos.x, 0) * ratio;
		}
	}
}
