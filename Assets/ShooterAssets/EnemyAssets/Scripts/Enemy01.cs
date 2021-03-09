// defines the behaviour of enemies (ships)

using UnityEngine;
using System.Collections;

public class Enemy01 : Enemy {

	private GameObject weapon;

	private IEnumerator coroutine;

	// Use this for initialization
	override protected void OnStart () {
		directionTarget = GameValuesContainer.container.shooterHandler.enemyDirectionTarget;
		Vector3 direction = Utility.RandomPointOf (directionTarget.GetComponent<Renderer>());
		this.transform.LookAt (direction, vectorUp);

		if (GetComponentInChildren<EnemyLaserWeapon> () != null) {
			weapon = GetComponentInChildren<EnemyLaserWeapon> ().transform.parent.gameObject;
			weaponTarget = GameValuesContainer.container.shooterHandler.playerShip.gameObject;
		}

		if (GetComponentInChildren<Animator> () != null) {
			GetComponentInChildren<Animator> ().SetTrigger ("Shoot");
		}
	}

	// Update is called once per frame
	void Update (){
		if (weapon != null) {
			weapon.transform.LookAt (weaponTarget.GetComponent<Collider> ().bounds.center, vectorUp);
		}
		if (coroutine == null) {
			coroutine = CoroutineMove ();
			StartCoroutine (coroutine);
		}
	}

	public void Shoot(){
		if (GetComponentInChildren<EnemyLaserWeapon> () != null) {
			GetComponentInChildren<EnemyLaserWeapon> ().Shoot ();
		}
	}

	IEnumerator CoroutineMove(){
		Vector3 directionOrigin = Utility.GetDirection(this.transform,vectorForward);
		Vector3 directionFinal = Utility.RandomPointOf (directionTarget.GetComponent<Renderer>()) - this.transform.position;
		directionFinal.Normalize ();
		float degree = 0;
		while (degree<1) {
			if (Time.timeScale > 0) {
				degree = Mathf.Min (1, degree + 0.05f);
				this.transform.LookAt (Utility.DirectionBetween (directionOrigin, directionFinal, degree) + this.transform.position, vectorUp);
				GetComponent<Rigidbody> ().velocity = Utility.GetDirection (this.transform, vectorForward) * speed;
			}
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForSeconds (Random.Range(0,1));
		coroutine = null;
	}
}
