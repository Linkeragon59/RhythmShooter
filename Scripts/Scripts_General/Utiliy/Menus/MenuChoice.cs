// abstract class use to attached a function to an item of a menu
// the OnChoosen function is called when the item is selected and the user press return/enter (see Menu.cs)

using UnityEngine;
using System.Collections;

abstract public class MenuChoice : MonoBehaviour {

	public void OnChosen(float delay=0f){
		StartCoroutine (DelayedEffect (delay));
	}

	private IEnumerator DelayedEffect(float delay)
	{
		float callTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < callTime + delay){
			yield return null;
		}
		Effect();
	}

	abstract protected void Effect (); // has to be redefined for each menu item, as the function of each item is different
}
