// Defines the behaviour of the Boss1 (only boss in the game for now)

using UnityEngine;
using System.Collections;

public class Boss01 : BossOrEnemy {

	public GameObject finalExplosion;
	public ObjectLabel prefabHealthBar;

	private Boss01SingleWeapon[] singleWeapons;
	private Boss01ZoneWeapon zoneWeapon;

	private IEnumerator coroutinePosition;
	private IEnumerator coroutineFollow;
	private IEnumerator coroutineExplode;
	private Animator animator;
	private ObjectLabel healthBar;

	private int form;

	// Use this for initialization
	override protected void OnStart () {
		directionTarget = GameValuesContainer.container.shooterHandler.bossDirectionTarget;

		singleWeapons = GetComponentsInChildren<Boss01SingleWeapon> ();
		zoneWeapon = GetComponentInChildren<Boss01ZoneWeapon> ();
		weaponTarget = GameValuesContainer.container.shooterHandler.playerShip.gameObject;

		animator = GetComponent<Animator> ();
		healthBar = Instantiate (prefabHealthBar);
		healthBar.SetTarget (this.transform);
		healthBar.gameObject.GetComponent<ProgressBarCircle> ().SetMaximum (hpMax);
		healthBar.gameObject.GetComponent<ProgressBarCircle> ().SetValue (hpMax);

		coroutinePosition = GoToPosition();
		StartCoroutine (coroutinePosition);
	}

	void Update () {
		foreach (Boss01SingleWeapon weapon in singleWeapons) {
			weapon.transform.LookAt (weaponTarget.GetComponent<Collider>().bounds.center, vectorUp);
		}
		zoneWeapon.transform.LookAt (weaponTarget.GetComponent<Collider>().bounds.center, vectorUp);

		if (coroutinePosition == null) {
			if (coroutineFollow != null) {
				StopCoroutine (coroutineFollow);
			}
			coroutineFollow = Follow ();
			StartCoroutine (coroutineFollow);
		}
	}

	public void WeakCircleShoot(){
		zoneWeapon.WeakCircleShoot ();
	}

	public void WeakSlalomShoot(){
		zoneWeapon.WeakSlalomShoot ();
	}

	public void StrongCircleShoot(){
		zoneWeapon.StrongCircleShoot ();
	}

	public void StrongSlalomShoot(){
		zoneWeapon.StrongSlalomShoot ();
	}

	public void WeakShoot(){
		foreach (Boss01SingleWeapon weapon in singleWeapons) {
			weapon.WeakShoot ();
		}
	}

	public void MediumShoot(){
		foreach (Boss01SingleWeapon weapon in singleWeapons) {
			weapon.MediumShoot ();
		}
	}

	public void StrongShoot(){
		foreach (Boss01SingleWeapon weapon in singleWeapons) {
			weapon.StrongShoot ();
		}
	}

	public void StopWeapons(){
		foreach (Boss01SingleWeapon weapon in singleWeapons) {
			weapon.StopShoot ();
		}
		zoneWeapon.StopShoot ();
	}

	override public void TakeDamages(int damages){
		if (coroutinePosition == null && coroutineExplode == null) {
			hp -= damages;
			healthBar.gameObject.GetComponent<ProgressBarCircle> ().SetValue (hp);
			if (hp <= 0 && form < 4) {
				form = 4;
				animator.SetTrigger ("Stop");
				StopWeapons ();
				coroutineExplode = CoroutineExplode ();
				StartCoroutine(coroutineExplode);
			}
			else if (hp <= 1f * hpMax / 5f && form < 3) {
				form = 3;
				animator.SetTrigger ("Form3");
				coroutineExplode = CoroutineExplode ();
				StartCoroutine(coroutineExplode);
			}
			else if (hp <= 3f * hpMax / 5f && form < 2) {
				form = 2;
				animator.SetTrigger ("Form2");
				coroutineExplode = CoroutineExplode ();
				StartCoroutine(coroutineExplode);
			}
		}
	}

	IEnumerator GoToPosition(){
		this.transform.LookAt (directionTarget.transform);
		GetComponent<Rigidbody> ().velocity = Utility.GetDirection (this.transform, vectorForward) * speed;
		float distance = (directionTarget.transform.position - this.transform.position).magnitude;
		float minDistance = distance;
		while (distance <= minDistance) {
			yield return  null;
			distance = (directionTarget.transform.position - this.transform.position).magnitude;
			minDistance = Mathf.Min (minDistance,distance);
		}
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		form = 1;
		animator.SetTrigger("Form1");
		GameValuesContainer.container.audioHandler.PlayBossMusic ();
		healthBar.SetFollow (true);
		coroutinePosition = null;
	}

	IEnumerator Follow(){
		Vector3 directionOrigin = Utility.GetDirection(this.transform,vectorForward);
		Vector3 directionFinal = this.transform.position - weaponTarget.transform.position;
		directionFinal.Normalize ();
		float degree = 0;
		while (degree<1) {
			if (Time.timeScale > 0) {
				degree = Mathf.Min (1, degree + 0.05f);
				this.transform.LookAt (Utility.DirectionBetween (directionOrigin, directionFinal, degree) + this.transform.position, vectorUp);
			}
			yield return new WaitForSeconds(0.01f);
		}
		coroutineFollow = null;
	}

	IEnumerator CoroutineExplode(){
		GameValuesContainer.container.movingObjectsContainers.ClearWeaponParticles ();
		float start = Time.realtimeSinceStartup;
		GameObject explosionObject;
		Bounds bounds = GetComponentInChildren<Renderer> ().bounds;
		float posX;
		float posY = bounds.center.y + bounds.size.y / 2;
		float posZ;
		while (Time.realtimeSinceStartup < start + (float)form) {
			posX = Random.Range (bounds.center.x - bounds.size.x / 2, bounds.center.x + bounds.size.x / 2);
			posZ = Random.Range (bounds.center.z - bounds.size.z / 2, bounds.center.z + bounds.size.z / 2);
			Vector3 randPoint = new Vector3 (posX,posY,posZ);
			explosionObject = (GameObject)Instantiate (explosion, randPoint, Quaternion.identity);
			explosionObject.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
			yield return new WaitForSeconds (Random.Range(0.1f,0.5f));
		}
		if (form > 3) {
			Explode ();
		}
		coroutineExplode = null;
	}

	public new void Explode(){
		GameObject particle = (GameObject)Instantiate (finalExplosion, transform.position, Quaternion.identity);
		particle.transform.localScale *= 5;
		particle.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
		GameValuesContainer.container.shooterHandler.IncreaseScore (scoreUp);
		GameValuesContainer.container.menuWrapper.DisplayResults ();
		Destroy (healthBar.gameObject);
		Destroy(gameObject);
	}
}
