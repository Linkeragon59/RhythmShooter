// Define the class of the weapons used by enemies

using UnityEngine;
using System.Collections;

public class EnemyLaserWeapon : MonoBehaviour {

	public EnemyLaser laser;

	private bool canGen = true;

	public void Initialize(){
		canGen = true;
	}

	void InstantiateParticle(Vector3 offsetPosition, Quaternion offsetRotation){
		EnemyLaser particle;
		particle = (EnemyLaser)Instantiate (laser, transform.position + offsetPosition, transform.rotation * offsetRotation);
		particle.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
	}

	public void Shoot(){
		if (canGen) {
			canGen = false;
			StartCoroutine (CoroutineShoot ());
		}
	}

	IEnumerator CoroutineShoot(){
		for (int i = 0; i < 3; i++) {
			InstantiateParticle (new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity);
			yield return new WaitForSeconds (0.2f);
		}
		canGen = true;
	}
}
