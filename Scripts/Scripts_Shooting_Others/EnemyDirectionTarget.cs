// Used to destroy the enemies that leave the field (missed by the player)

using UnityEngine;
using System.Collections;

public class EnemyDirectionTarget : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		Destroy (other.gameObject);
	}
}
