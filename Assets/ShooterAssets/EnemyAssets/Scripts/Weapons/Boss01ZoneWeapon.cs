// Weapon of the Boss that shoot zone attacks (circular, ...)

using UnityEngine;
using System.Collections;

public class Boss01ZoneWeapon : MonoBehaviour {

	public EnemyLaser laser1;
	public EnemyLaser laser2;

	private IEnumerator coroutineCircle;
	private IEnumerator coroutineSlalom;

	void InstantiateParticle(EnemyLaser laser, Vector3 offsetPosition, Quaternion offsetRotation){
		EnemyLaser particle;
		particle = (EnemyLaser)Instantiate (laser, transform.position + offsetPosition, transform.rotation * offsetRotation);
		particle.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
	}

	public void WeakCircleShoot(){
		if (coroutineCircle == null) {
			coroutineCircle = CoroutineCircleShoot (3);
			StartCoroutine (coroutineCircle);
		}
	}

	public void StrongCircleShoot(){
		if (coroutineCircle == null) {
			coroutineCircle = CoroutineCircleShoot (5);
			StartCoroutine (coroutineCircle);
		}
	}

	IEnumerator CoroutineCircleShoot(int power){
		for (int j = 0; j < power; j++) {
			for (int i = 0; i < 40; i++) {
				InstantiateParticle (laser1, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f,360.0f*i/40.0f+360.0f*j/3.0f,0.0f));
			}
			yield return new WaitForSeconds (0.3f);
		}
		coroutineCircle = null;
	}

	public void WeakSlalomShoot(){
		if (coroutineSlalom == null) {
			coroutineSlalom = CoroutineSlalomShoot (5);
			StartCoroutine (coroutineSlalom);
		}
	}

	public void StrongSlalomShoot(){
		if (coroutineSlalom == null) {
			coroutineSlalom = CoroutineSlalomShoot (7);
			StartCoroutine (coroutineSlalom);
		}
	}

	IEnumerator CoroutineSlalomShoot(int power){
		float[] angles;
		if (power > 5) {
			angles = new float[]{ 30, -30, 20, -20, 10, -10, 0 };
		}
		else {
			angles = new float[]{ 30, -30, 15, -15, 0 };
		}
		for (int j = 0; j < angles.Length; j++) {
			for (int i = 0; i < 15; i++) {
				InstantiateParticle (laser2, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f,40.0f*(i-7)/15.0f+angles[j],0.0f));
			}
			yield return new WaitForSeconds (0.8f);
		}
		coroutineSlalom = null;
	}

	public void StopShoot(){
		if (coroutineCircle != null) {
			StopCoroutine (coroutineCircle);
			coroutineCircle = null;
		}
		if (coroutineSlalom != null) {
			StopCoroutine (coroutineSlalom);
			coroutineSlalom = null;
		}
	}
}
