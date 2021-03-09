// abstract class that defines the common behaviour of enemies and bosses

using UnityEngine;
using System.Collections;

abstract public class BossOrEnemy : MonoBehaviour {

	public Vector3 vectorForward = Vector3.forward;
	public Vector3 vectorUp = Vector3.up;
	public float speed;
	public int hpMax;
	public int scoreUp;
	public GameObject explosion; // explosion animation prefab for enemies

	protected GameObject weaponTarget;
	protected GameObject directionTarget;

	protected int hp;

	void Start(){
		hp = hpMax;
		OnStart ();
	}

	abstract protected void OnStart ();

	abstract public void TakeDamages (int damages);

	public void Explode() { // Destroys itself and makes the explosion animation
		GameObject particle = (GameObject)Instantiate (explosion, transform.position, Quaternion.identity);
		particle.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
		GameValuesContainer.container.shooterHandler.IncreaseScore (scoreUp);
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other) { // effects of collision with other objects
		if (other.GetComponent<PlayerShip> () != null) {
			other.GetComponent<PlayerShip> ().Explode(); // destroy colliding player
		}
	}
}
