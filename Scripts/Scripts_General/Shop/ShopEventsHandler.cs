using UnityEngine;
using System.Collections;

public class ShopEventsHandler : MonoBehaviour {

	public GameObject container;

	void Awake(){
		Time.timeScale = 1.0f;
		Game.current = new Game (); // new Game if the load doesn't work
		SaveLoad.Load (); // loads the game save that contains the items that have to be attached to the player
		GameValuesContainer.container = new GameValuesContainer();
		GameValuesContainer.container.particlesContainer = container;
	}
}
