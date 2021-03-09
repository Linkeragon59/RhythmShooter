// static class to access some useful functions from anywhere

using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	// Used to get the direction in which an object should move when going straight
	static public Vector3 GetDirection(Transform transform, Vector3 vectorForward){
		float sX = Mathf.Sin (transform.eulerAngles.x * Mathf.PI / 180);
		float cX = Mathf.Cos (transform.eulerAngles.x * Mathf.PI / 180);
		float sY = Mathf.Sin (transform.eulerAngles.y * Mathf.PI / 180);
		float cY = Mathf.Cos (transform.eulerAngles.y * Mathf.PI / 180);
		float sZ = Mathf.Sin (transform.eulerAngles.z * Mathf.PI / 180);
		float cZ = Mathf.Cos (transform.eulerAngles.z * Mathf.PI / 180);
		float a = vectorForward.x;
		float b = vectorForward.y;
		float c = vectorForward.z;
		float vx = a * (cY * cZ + sX * sY * sZ) + b * (sX * sY * cZ - cY * sZ) + c * (cX * sY);
		float vy = a * (cX * sZ) + b * (cX * cZ) - c * (sX);
		float vz = a * (sX * cY * sZ - sY * cZ) + b * (sY * sZ + sX * cY * cZ) + c * (cX * cY);
		Vector3 result = new Vector3 (vx, vy, vz);
		result.Normalize ();
		return result;
	}

	static public Vector3 DirectionBetween(Vector3 origin, Vector3 target, float degree){
		if (degree >= 0 && degree <= 1) {
			float newX = (1 - degree) * origin.x + degree * target.x;
			float newY = (1 - degree) * origin.y + degree * target.y;
			float newZ = (1 - degree) * origin.z + degree * target.z;
			Vector3 result = new Vector3 (newX, newY, newZ);
			result.Normalize ();
			return result;
		}
		else {
			return target;
		}
	}

	static public Vector3 RandomPointOf(Renderer renderer){
		float randX = renderer.bounds.center.x + Random.Range (-renderer.bounds.size.x/2,renderer.bounds.size.x/2);
		float randY = renderer.bounds.center.y + Random.Range (-renderer.bounds.size.y/2,renderer.bounds.size.y/2);
		float randZ = renderer.bounds.center.z + Random.Range (-renderer.bounds.size.z/2,renderer.bounds.size.z/2);
		return new Vector3 (randX, randY, randZ);
	}

	// Logistic used by the Neural Network
	static public float Logistic(float x){
		return 2 / (1 + Mathf.Exp (-1*x)) - 1;
	}
}
