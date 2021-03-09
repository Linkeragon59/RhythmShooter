// defines the behaviour of asteroids
// attached to every asteroid objects (to the prefab)

using UnityEngine;
using System.Collections;

public class Asteroid : Enemy {

	override protected void OnStart (){
		GetComponent<Rigidbody> ().angularVelocity = new Vector3 (Random.Range (-200, 200), Random.Range (-200, 200), Random.Range (-200, 200));

		directionTarget = GameValuesContainer.container.shooterHandler.enemyDirectionTarget;
		Vector3 target = Utility.RandomPointOf (directionTarget.GetComponent<Renderer>());
		Vector3 direction = target - this.transform.position;
		direction.Normalize ();
		GetComponent<Rigidbody> ().velocity = direction * speed;
	}
}
