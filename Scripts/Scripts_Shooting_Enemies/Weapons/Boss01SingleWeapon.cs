// Weapon of the Boos that shot simple attacks

using UnityEngine;
using System.Collections;

public class Boss01SingleWeapon : MonoBehaviour {

	public EnemyLaser laser;

	private IEnumerator coroutine;

	void InstantiateParticle(Vector3 offsetPosition, Quaternion offsetRotation){
		EnemyLaser particle;
		particle = (EnemyLaser)Instantiate (laser, transform.position + offsetPosition, transform.rotation * offsetRotation);
		particle.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
	}

	public void WeakShoot(){
		if (coroutine == null) {
			coroutine = CoroutineShoot (3);
			StartCoroutine (coroutine);
		}
	}

	public void MediumShoot(){
		if (coroutine == null) {
			coroutine = CoroutineShoot (5);
			StartCoroutine (coroutine);
		}
	}

	public void StrongShoot(){
		if (coroutine == null) {
			coroutine = CoroutineShoot (10);
			StartCoroutine (coroutine);
		}
	}

	IEnumerator CoroutineShoot(int power){
		for (int j = 0; j < power; j++) {
			InstantiateParticle (new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity);
			yield return new WaitForSeconds (0.15f);
		}
		coroutine = null;
	}

	public void StopShoot(){
		if (coroutine != null) {
			StopCoroutine (coroutine);
			coroutine = null;
		}
	}
}
