// defines the BulletWeapon (that heritates from abstract Item class) class
// attached to every Bullet (weapons) objects

using UnityEngine;
using System.Collections;

public class BulletWeapon : Weapon {

	public GameObject bullet;

	private bool canGen = false;

	void Update(){
		if (canGen) {
			canGen = false;
			StartCoroutine (ShootBullet());
		}
	}

	override public void Initialize(){
		canGen = true;
	}

	void InstantiateParticle(Vector3 offsetPosition, Quaternion offsetRotation){
		GameObject particle;
		particle = (GameObject)Instantiate (bullet, transform.position + offsetPosition + new Vector3(1f,0,0), transform.rotation * offsetRotation);
		particle.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
	}

	IEnumerator ShootBullet() // shoot 3 bullet every 0.2 second
	{
		if (level == 0) {
			yield return new WaitForSeconds (0.1f);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.identity);
			InstantiateParticle(new Vector3(0.0f,0.0f,0.0f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.identity);
			canGen = true;
		}
		else if (level == 1) {
			yield return new WaitForSeconds (0.1f);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.Euler (0.0f, -5, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.identity);
			InstantiateParticle(new Vector3(0.0f,0.0f,0.0f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.Euler (0.0f, 5, 0.0f));
			canGen = true;
		}
		else if (level == 2) {
			yield return new WaitForSeconds (0.05f);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.Euler (0.0f, -10, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.Euler (0.0f, -5, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.identity);
			InstantiateParticle(new Vector3(0.0f,0.0f,0.0f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.Euler (0.0f, 5, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.Euler (0.0f, 10, 0.0f));
			canGen = true;
		}
		else if (level == 3) {
			yield return new WaitForSeconds (0.02f);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.Euler (0.0f, -15, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.Euler (0.0f, -10, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.Euler (0.0f, -5, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, 0.3f),Quaternion.identity);
			InstantiateParticle(new Vector3(0.0f,0.0f,0.0f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.Euler (0.0f, 5, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.Euler (0.0f, 10, 0.0f));
			InstantiateParticle(new Vector3(-0.2f, 0.0f, -0.3f),Quaternion.Euler (0.0f, 15, 0.0f));
			canGen = true;
		}
		else {
			yield return new WaitForSeconds (0f);
			canGen = true;
		}
	}

	override public bool CanBeSelected(){ // define the condition for the armor to be selected
		return (GameValuesContainer.container.combo >= comboThreshold);
	}

}
