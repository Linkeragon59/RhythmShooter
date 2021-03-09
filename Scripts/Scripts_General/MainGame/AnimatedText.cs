// Used to imitates the fonctioning of a GIF format
// Used to display the 'The boss is approaching' animated text

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedText : MonoBehaviour {

	public Sprite[] frames;
	public int framesPerSecond = 15;

	private bool loop;
	private float duration;

	private Text text;
	private float startTime;

	void Awake(){
		startTime = Time.time;
		text = GetComponentInChildren<Text> ();
		SetColors(new Color(1,1,1,0.5f));
	}
	
	// Update is called once per frame
	void Update () {
		if (frames.Length > 0) {
			int index;
			if (loop) { // Is the animation a loop?
				index = (int)((Time.time-startTime) * framesPerSecond) % frames.Length;
			}
			else { // If not a loop
				index = (int)Mathf.Min(frames.Length-1,(Time.time-startTime) * framesPerSecond);
			}
			GetComponent<Image> ().sprite = frames [index];
			// If loop, when duration reached, if not loop, when animation finished
			if ((loop && Time.time>startTime+duration) || (!loop && (index == frames.Length-1 || Time.time>startTime+duration))) {
				Destroy(gameObject);
			}
		}
	}

	public void SetLoop(bool loop){
		this.loop = loop;
	}

	public void SetDuration(float duration){
		this.duration = duration;
	}

	public void SetColors(Color color, Color colorText){
		GetComponent<Image> ().color = color;
		text.color = colorText;
	}

	public void SetColors(Color color){
		SetColors (color, new Color (1,1,1,1));
	}

	public void SetText(string text){
		if (text != null) {
			this.text.text = text;
		}
	}
}
