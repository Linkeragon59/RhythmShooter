// attached to the particle used in the Laser weapons (little ray)
// defines the behaviour of the laser particles

using UnityEngine;
using System.Collections;

public class EnemyLaser : MonoBehaviour {

	public float speed = 7.0f;
	public Vector3 vectorForward = Vector3.forward;

	private bool playerLaser;

	// Use this for initialization
	void Awake () { // defines movement of the bullets, go straight with the specified speed
		GetComponent<Rigidbody>().velocity = getDirection() * speed;
	}

	Vector3 getDirection(){
		float sX = Mathf.Sin (transform.eulerAngles.x * Mathf.PI / 180);
		float cX = Mathf.Cos (transform.eulerAngles.x * Mathf.PI / 180);
		float sY = Mathf.Sin (transform.eulerAngles.y * Mathf.PI / 180);
		float cY = Mathf.Cos (transform.eulerAngles.y * Mathf.PI / 180);
		float sZ = Mathf.Sin (transform.eulerAngles.z * Mathf.PI / 180);
		float cZ = Mathf.Cos (transform.eulerAngles.z * Mathf.PI / 180);
		float a = vectorForward.x;
		float b = vectorForward.y;
		float c = vectorForward.z;
		float vx = a * (cY * cZ + sX * sY * sZ) + b * (sX * sY * cZ - cY * sZ) + c * (cX * sY);
		float vy = a * (cX * sZ) + b * (cX * cZ) - c * (sX);
		float vz = a * (sX * cY * sZ - sY * cZ) + b * (sY * sZ + sX * cY * cZ) + c * (cX * cY);
		return new Vector3 (vx,vy,vz);
	}

	void OnTriggerEnter(Collider other) { // defines the effects of collisions
		if (other.GetComponent<PlayerShip> () != null) {
			other.GetComponent<PlayerShip> ().TakeShootingDamage(1); // destroys colliding player
			Destroy(gameObject);
		} else if (other.GetComponent<Asteroid> () != null) {
			other.GetComponent<Asteroid> ().Explode (); // destroys colliding asteroids
			Destroy(gameObject);
		}
	}
}
