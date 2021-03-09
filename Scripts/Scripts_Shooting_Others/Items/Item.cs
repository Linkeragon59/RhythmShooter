// abstract class from which all the items (weapons and armors) heritates
// defines the properties that are common to all items

using UnityEngine;
using System.Collections;

abstract public class Item : MonoBehaviour {

	public string itemName; // name of the item
	public int level; // level of the item
	public int comboThreshold; // combo that is needed to be able to use the item
	public int breakResistance; // number of hit the armor can take before breaking
	protected int damage = 0; // number of hit taken in shooting mode

	public string description; // description of the item displayed in the shop

	abstract public bool CanBeSelected (); // has to be redefined for each item, says if the item can be used in function of the comboThreshold, the damages...
}
