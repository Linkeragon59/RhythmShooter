// Attached to the borders of the game, destroy any colliding objects (in particular the bullets, laser, asteroid any enemies that are not destroyed by the player)
// avoid objects from becoming too numerous by destroying the objects that are not on the screen anymore)

using UnityEngine;
using System.Collections;

public class GameBorder : MonoBehaviour {

	void OnTriggerExit(Collider other) {
		Destroy (other.gameObject);
	}
}
