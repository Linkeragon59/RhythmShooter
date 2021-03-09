// abstract class that defines the common behaviour of enemies, execpt boss (see BossOrEnemy.cs)

using UnityEngine;
using System.Collections;

abstract public class Enemy : BossOrEnemy {

	override public void TakeDamages(int damages){
		hp -= damages;
		if (hp <= 0) {
			GameValuesContainer.container.killedEnemies++;
			Explode ();
		}
	}

	void OnTriggerEnter(Collider other) { // effects of collision with other objects
		if (other.GetComponent<PlayerShip> () != null) {
			other.GetComponent<PlayerShip> ().Explode();
		}
		if (other.GetComponent<PlayerShip> () != null || other.GetComponent<Enemy> () != null) {
			Explode (); // destroy itself
		}
	}
}
