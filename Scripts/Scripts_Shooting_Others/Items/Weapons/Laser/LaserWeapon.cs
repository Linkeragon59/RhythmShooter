// defines the LaserWeapon (that heritates from abstract Item class) class
// attached to every Laser (weapons) objects

using UnityEngine;
using System.Collections;

public class LaserWeapon : Weapon {

	public GameObject laser;

	private bool canGen = false;

	void Update(){ // the common characteristic : shoot when it can
		if (canGen) {
			canGen = false;
			StartCoroutine (ShootLaser ());
		}
	}

	override public void Initialize(){
		canGen = true;
	}

	void InstantiateParticle(Vector3 offsetPosition, Quaternion offsetRotation){
		GameObject particle;
		particle = (GameObject)Instantiate (laser, transform.position + offsetPosition + new Vector3(1f,0,0), transform.rotation * offsetRotation);
		particle.transform.SetParent (GameValuesContainer.container.particlesContainer.transform);
	}

	IEnumerator ShootLaser() // shoot laser in function of level
	{
		if (level == 0) {
			yield return new WaitForSeconds (0.1f);
			InstantiateParticle(new Vector3(-0.1f,0.0f,0.3f),Quaternion.identity);
			InstantiateParticle(new Vector3(0.0f,0.0f,0.0f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.1f,0.0f,-0.3f),Quaternion.identity);
			canGen = true;
		}
		else if (level == 1) {
			yield return new WaitForSeconds (0.1f);
			InstantiateParticle(new Vector3(-0.1f,0.0f,0.3f),Quaternion.Euler(0.0f,-7.5f,0.0f));
			InstantiateParticle(new Vector3(0.0f,0.0f,0.1f),Quaternion.identity);
			InstantiateParticle(new Vector3(0.0f,0.0f,-0.1f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.1f,0.0f,-0.3f),Quaternion.Euler(0.0f,7.5f,0.0f));
			canGen = true;
		}
		else if (level == 2) {
			yield return new WaitForSeconds (0.05f);
			InstantiateParticle(new Vector3(-0.3f,0.0f,-1f),Quaternion.Euler(0.0f,2f,0.0f));
			InstantiateParticle(new Vector3(-0.3f,0.0f,-1.2f),Quaternion.Euler(0.0f,2f,0.0f));
			InstantiateParticle(new Vector3(-0.1f,0.0f,0.3f),Quaternion.Euler(0.0f,-7.5f,0.0f));
			InstantiateParticle(new Vector3(0.0f,0.0f,0.1f),Quaternion.identity);
			InstantiateParticle(new Vector3(0.0f,0.0f,-0.1f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.1f,0.0f,-0.3f),Quaternion.Euler(0.0f,7.5f,0.0f));
			InstantiateParticle(new Vector3(-0.3f,0.0f,1f),Quaternion.Euler(0.0f,-2f,0.0f));
			InstantiateParticle(new Vector3(-0.3f,0.0f,1.2f),Quaternion.Euler(0.0f,-2f,0.0f));
			canGen = true;
		}
		else if (level == 3) {
			yield return new WaitForSeconds (0.05f);
			InstantiateParticle(new Vector3(-0.1f,0.0f,-1.4f),Quaternion.Euler(0.0f,9.5f,0.0f));
			InstantiateParticle(new Vector3(-0.3f,0.0f,-1.2f),Quaternion.Euler(0.0f,2f,0.0f));
			InstantiateParticle(new Vector3(-0.3f,0.0f,-1f),Quaternion.Euler(0.0f,2f,0.0f));
			InstantiateParticle(new Vector3(-0.1f,0.0f,-0.8f),Quaternion.Euler(0.0f,-5.5f,0.0f));
			InstantiateParticle(new Vector3(-0.1f,0.0f,0.3f),Quaternion.Euler(0.0f,-7.5f,0.0f));
			InstantiateParticle(new Vector3(0.0f,0.0f,0.1f),Quaternion.identity);
			InstantiateParticle(new Vector3(0.0f,0.0f,-0.1f),Quaternion.identity);
			InstantiateParticle(new Vector3(-0.1f,0.0f,-0.3f),Quaternion.Euler(0.0f,7.5f,0.0f));
			InstantiateParticle(new Vector3(-0.1f,0.0f,0.8f),Quaternion.Euler(0.0f,5.5f,0.0f));
			InstantiateParticle(new Vector3(-0.3f,0.0f,1f),Quaternion.Euler(0.0f,-2f,0.0f));
			InstantiateParticle(new Vector3(-0.3f,0.0f,1.2f),Quaternion.Euler(0.0f,-2f,0.0f));
			InstantiateParticle(new Vector3(-0.1f,0.0f,1.4f),Quaternion.Euler(0.0f,-9.5f,0.0f));
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
