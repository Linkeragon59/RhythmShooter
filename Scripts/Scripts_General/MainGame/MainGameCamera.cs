using UnityEngine;
using System.Collections;

public class MainGameCamera : MonoBehaviour {

	void Awake () {
		if (GameValuesContainer.container == null) {
			GameValuesContainer.container = new GameValuesContainer ();
		}
		GameValuesContainer.container.mainCamera = this;
	}
}
