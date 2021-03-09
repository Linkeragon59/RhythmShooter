// script attached to the objects that reach the target of the rhythmgame to indicate to the player the moment he should press the "g" key
// see PlayerTitle.cs

using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	private float beatTime;
	private GameObject target;

	public void Initialize (GameObject target, float reachTime){ /// called by SpawnSpot
		this.target = target;
		float currentAudioTime = GameValuesContainer.container.rhythmHandler.spawnSpot.GetTime ();
		beatTime = currentAudioTime + reachTime;
		Vector3 distance = target.transform.position - this.transform.position;
		float floatDistance = Mathf.Sqrt (Mathf.Pow(distance.x,2)+Mathf.Pow(distance.y,2)+Mathf.Pow(distance.z,2));
		Vector3 direction = distance / floatDistance;
		if (reachTime > 0) {
			this.GetComponent<Rigidbody> ().velocity = direction * floatDistance / reachTime;
		}
	}

	public float GetBeatTime(){
		return beatTime;
	}

	public GameObject GetTarget(){
		return target;
	}
}
