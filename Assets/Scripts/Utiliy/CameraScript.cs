//Script for controlling the Aspect ratio, to add to the main camera.
//from http://gamedesigntheory.blogspot.jp/2010/09/controlling-aspect-ratio-in-unity.html
//modified by Dumont Corentin 

using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// set the desired aspect ratio
	public float ratio = 1024f / 576f;

	// Use this for initialization
	void Start () {
		// determine the game window's current aspect ratio
		float windowRatio = (float)Screen.width / (float)Screen.height;

		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();

		// if window's ratio < desired ratio, add letterbox
		if (windowRatio < ratio)
		{  
			Rect rect = camera.rect;

			rect.width = 1.0f;
			rect.height = windowRatio/ratio;
			rect.x = 0;
			rect.y = (1.0f - windowRatio/ratio) / 2.0f;

			camera.rect = rect;
		}
		else // add pillarbox
		{
			Rect rect = camera.rect;

			rect.width = ratio/windowRatio;
			rect.height = 1.0f;
			rect.x = (1.0f - ratio/windowRatio) / 2.0f;
			rect.y = 0;

			camera.rect = rect;
		}
	}

}
