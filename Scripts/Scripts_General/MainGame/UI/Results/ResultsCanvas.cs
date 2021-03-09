// Abstract class that defines the common properties of the canvas that display results (game's end and stage selection) 

using UnityEngine;
using System.Collections;

abstract public class ResultsCanvas : MonoBehaviour {

	public AudioClip calculationClip;
	public AudioClip endCalculationClip;

	protected AudioSource audioSource;

	abstract public void StartDisplaying ();
	abstract protected void DisplayGraph ();
	abstract public void OnGraphDisplayed ();
}
