// attached to the particle used in the Bullet weapons (little sphere)
// defines the behaviour of the bullets particles

using UnityEngine;
using System.Collections;

public class WeaponParticle : MonoBehaviour {

	public float speed;
	public int damages;

	// Use this for initialization
	void Start () { // defines movement of the bullets, go straight with the specified speed
		Vector3 movement = new Vector3 (Mathf.Cos(transform.eulerAngles.y*Mathf.PI/180)*Mathf.Cos(transform.eulerAngles.z*Mathf.PI/180),Mathf.Sin(transform.eulerAngles.z*Mathf.PI/180),-Mathf.Sin(transform.eulerAngles.y*Mathf.PI/180)*Mathf.Cos(transform.eulerAngles.z*Mathf.PI/180));
		GetComponent<Rigidbody>().velocity = movement * speed;
	}

	void OnTriggerEnter(Collider other) { // defines the effects of collisions
		if (other.GetComponent<BossOrEnemy> () != null) {
			other.GetComponent<BossOrEnemy> ().TakeDamages(damages); // destroys colliding enemies (ships)
			Destroy(gameObject);
		}
	}

}
