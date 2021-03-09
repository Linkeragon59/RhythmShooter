// Used to create a GIF-like animation of an explosion. (similar to AnimatedText.cs)
// Displayed when a ship/asteroid explodes, the boss takes important damages...

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Explosion : MonoBehaviour {

	public Sprite[] frames;

	public bool loop;
	public float duration;
	public AudioClip explosionClip;

	private SpriteRenderer child;
	private AudioSource audioSource;
	private float clipStart;

	void Start(){
		child = GetComponentInChildren<SpriteRenderer> ();
		audioSource = GetComponent<AudioSource> ();
		StartCoroutine (Animation ());
	}

	public void SetDuration(float duration){
		this.duration = duration;
	}

	public void SetLoop(bool loop){
		this.loop = loop;
	}

	public void SetAudioClip(AudioClip clip){
		explosionClip = clip;
	}

	IEnumerator Animation(){
		child.gameObject.transform.rotation = Quaternion.Euler(new Vector3(90f,0,Random.Range (0f, 360f)));
		int i = 0;
		while (loop || i < frames.Length) {
			if (i == 0) {
				audioSource.PlayOneShot (explosionClip);
				clipStart = Time.realtimeSinceStartup;
			}
			child.sprite = frames [i];
			yield return new WaitForSeconds (duration / (float)frames.Length);
			i++;
			if(loop){
				i = i % frames.Length;
			}
		}
		while(Time.realtimeSinceStartup<clipStart+explosionClip.length){
			yield return null;
		}
		Destroy (this.gameObject);
	}
}
