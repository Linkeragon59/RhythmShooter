// attached to the background of the shooter game to give the illusion of movement by scrolling the background

using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

	public float scrollSpeed;

	void Update()
	{
		GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2 (Time.time * scrollSpeed / 10f,0));
	}
}
