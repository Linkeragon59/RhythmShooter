// attached to every objects of the stage selection scene that represent a stage

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageSelectionItem : MonoBehaviour {

	public string stageName;
	public float scrollSpeed = 0.2f;

	private bool active = false;
	private GameObject texture;

	private AudioSource audioSource;
	private IEnumerator coroutineFade;

	void Awake(){
		audioSource = GetComponent<AudioSource> ();
	}

	void Start(){
		foreach (Transform child in transform) {
			texture = child.gameObject;
		}
	}

	void Update()
	{
		if (active) { // Make the background of the stage move
			texture.GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2 (Time.time * scrollSpeed, 0));
		}
	}

	public void SetActivation(bool active){
		this.active = active;
		if (coroutineFade != null) {
			StopCoroutine (coroutineFade);
		}
		if (active) {
			transform.rotation = Quaternion.Euler (20, 0, 0);
			if (audioSource != null) {
				coroutineFade = FadeIn ();
				StartCoroutine (coroutineFade);
			}
		}
		else {
			transform.rotation = Quaternion.Euler (0, 0, 0);
			if (audioSource != null) {
				coroutineFade = FadeOut ();
				StartCoroutine (coroutineFade);
			}
		}
	}

	IEnumerator FadeIn() {
		if (audioSource != null) {
			float f = 0f;
			audioSource.volume = f;
			audioSource.Play ();
			while (f != 1f) {
				f = Mathf.Min(1f,f+1f/20f);
				audioSource.volume = f;
				yield return new WaitForSeconds(0.05f);
			}
		}
		coroutineFade = null;
	}

	IEnumerator FadeOut() {
		if (audioSource != null) {
			float initialVolume = audioSource.volume;
			float f = initialVolume;
			while (f != 0f) {
				audioSource.volume = f;
				f = Mathf.Max(0,f-1f/15f);
				yield return new WaitForSeconds(0.05f);
			}
			audioSource.Stop ();
		}
		coroutineFade = null;
	}

	public void OnChosen(float delay=0f){
		StartCoroutine (DelayedEffect (delay));
	}

	private IEnumerator DelayedEffect(float delay)
	{
		float callTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < callTime + delay){
			yield return null;
		}
		SceneManager.LoadSceneAsync (stageName);
	}
}
