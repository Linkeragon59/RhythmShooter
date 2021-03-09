// Attached to the object that become parent of all moving objects (enemies, bullets, lasers, bubbles...)

using UnityEngine;
using System.Collections;

public class MovingObjectsContainer : MonoBehaviour {

	public GameObject particlesContainer;
	public GameObject enemiesContainer;
	public GameObject bubblesContainer;

	// Use this for initialization
	void Awake () {
		if (GameValuesContainer.container == null) {
			GameValuesContainer.container = new GameValuesContainer ();
		}
		GameValuesContainer.container.movingObjectsContainers = this;
		GameValuesContainer.container.particlesContainer = particlesContainer;
		GameValuesContainer.container.enemiesContainer = enemiesContainer;
		GameValuesContainer.container.bubblesContainer = bubblesContainer;
	}

	public void Clear(){
		ClearShooterObjects ();
		ClearRhythmObjects ();
	}

	public void ClearShooterObjects(){
		ClearWeaponParticles ();
		ClearEnemies ();
	}

	public void ClearRhythmObjects(){
		ClearBubbles ();
	}

	public void ClearWeaponParticles(){
		foreach (Transform child in particlesContainer.transform)
		{
			Destroy (child.gameObject);
		}
	}

	public void ClearEnemies(){
		foreach (Transform child in enemiesContainer.transform)
		{
			Destroy (child.gameObject);
		}
	}

	public void ClearBubbles(){
		foreach (Transform child in bubblesContainer.transform)
		{
			Destroy (child.gameObject);
		}
	}
}
